using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 9;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool _hasBrella = false;
    private bool _hasClock = false;
    private bool _hasPants = false;

    private Vector2 newBrellaPos;
    private Vector2[] pantsPos;
    private float pantsTimer;
    public float pantsInterval = 1;
    private int pantsIndex = 0;
    private int pantsSize = 4;

    private bool bounce = false;

    public GameObject BrellaAnim;
    public GameObject GapAnim;

    public static PlayerPlatformerController Instance;

    void Start()
    {
        pantsPos = new Vector2[pantsSize];

        for (int i = 0; i < pantsPos.Length; i++)
        {
            pantsPos[i] = this.transform.position;
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> (); 
        animator = GetComponent<Animator> ();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (alive)
        {
            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded || bounce)
            {
                velocity.y = jumpTakeOffSpeed;
                bounce = false;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (_hasBrella && Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position + Vector3.right, Vector2.right, 15f);

                foreach (var hit in hits)
                {
                    Debug.Log(hit.collider.tag);
                    if (hit.collider.tag == "Platforms")
                    {
                        _hasBrella = false;
                        Instantiate(BrellaAnim, this.transform.position, Quaternion.identity);
                        allowMovement = false;

                        newBrellaPos = hit.point + Vector2.left;
                    }
                }
            }
            if (_hasClock && Input.GetButtonDown("Fire2"))
            {
                _hasClock = false;
                GameController.Instance.ScaleTime();
            }
            if (_hasPants && Input.GetButtonDown("Fire3"))
            {
                _hasPants = false;
            }
        }

        if (move.x > 0.01f)
        {
            if(spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        } 
        else if (move.x < -0.01f)
        {
            if(spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);

        targetVelocity = move * maxSpeed;
    }

    public void Kill()
    {
        if (_hasPants)
        {
            _hasPants = false;
            this.transform.position = pantsPos[pantsIndex];
        }
        else
        {
            alive = false;
            animator.SetTrigger("death");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Death")
        {
            Kill();
        }

        if (collider.tag == "Item")
        {
            if (collider.name.Contains("Clock_Item"))
            {
                if (!_hasClock)
                {
                    Destroy(collider.gameObject);
                    _hasClock = true;
                }
            }
            else if (collider.name.Contains("Pants_Item"))
            {
                if (!_hasPants)
                {
                    Destroy(collider.gameObject);
                    _hasPants = true;
                }
            }
            else if (collider.name.Contains("Brella_Item"))
            {
                if (!_hasBrella)
                {
                    Destroy(collider.gameObject);
                    _hasBrella = true;
                }
            }
        }
    }

    void ReloadLevel()
    {
        GameController.Instance.Reload();
    }

    public void SpriteHide()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SpriteShow()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        allowMovement = true;
    }

    public void Teleport()
    {
        Instantiate(GapAnim, newBrellaPos, Quaternion.identity);
        this.transform.position = newBrellaPos;
    }

    protected override void CheckPants()
    {
        if (pantsTimer <= 0)
        {
            pantsTimer = pantsInterval;

            pantsPos[pantsIndex] = this.transform.position;

            pantsIndex++;
            if (pantsIndex >= pantsPos.Length)
            {
                pantsIndex = 0;
            }
        }
        else
        {
            pantsTimer -= Time.unscaledDeltaTime;
        }
    }

    public void Bump()
    {
        bounce = true;
    }
}
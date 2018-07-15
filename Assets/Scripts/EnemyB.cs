using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : MonoBehaviour
{
    public float KillAt;

    void OnCollisionEnter2D(Collision2D collison)
    {
        Debug.Log(collison.contacts[0].point.y - this.transform.position.y);

        if (collison.contacts[0].point.y - this.transform.position.y > KillAt)
        {
            Destroy(this.gameObject);
            PlayerPlatformerController.Instance.Bump();
        }
        else
        {
            PlayerPlatformerController.Instance.Kill();
        }
    }
}

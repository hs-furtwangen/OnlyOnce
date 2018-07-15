using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public AnimationCurve ScaleTimeCurve;
    private float scaleTimer;

	// Use this for initialization
	void Start () {
	    if (Instance == null)
	    {
	        Instance = this;
	    }
	}

    public void Reload()
    {
        Debug.Log("Reloading Level");
        SceneManager.LoadScene(0);
    }

    public void ScaleTime()
    {
        scaleTimer = ScaleTimeCurve.keys[ScaleTimeCurve.length -1].time;
        Debug.Log(scaleTimer);
    }

    void Update()
    {
        if (scaleTimer > 0)
        {
            Debug.Log(ScaleTimeCurve.length - scaleTimer);

            Time.timeScale = ScaleTimeCurve.Evaluate(ScaleTimeCurve.keys[ScaleTimeCurve.length -1].time - scaleTimer);
            scaleTimer -= Time.unscaledDeltaTime;
        }
    }


}

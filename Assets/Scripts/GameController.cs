using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

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

}

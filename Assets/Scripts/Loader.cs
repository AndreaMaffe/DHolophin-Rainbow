using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public GameObject GameManager;
    public GameObject AudioManager;
    public GameObject InputHandler;
    public GameObject DolphinManager;

	// Use this for initialization
	void Awake ()
    {
        Instantiate(GameManager);
        Instantiate(AudioManager);
        Instantiate(InputHandler);
        Instantiate(DolphinManager);

        SceneManager.LoadScene(1);
	}
}

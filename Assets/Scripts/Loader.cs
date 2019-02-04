using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public GameObject GameManager;
    public GameObject AudioManager;
    public GameObject InputHandler;

	// Use this for initialization
	void Start ()
    {
        Instantiate(GameManager);
        Instantiate(AudioManager);
        Instantiate(InputHandler);

        SceneManager.LoadScene(1);
	}
}

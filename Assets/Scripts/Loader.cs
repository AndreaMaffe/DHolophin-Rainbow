using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    private GameObject GameManager;

	// Use this for initialization
	void Start ()
    {
        GameManager = Resources.Load<GameObject>("Prefabs/GameManager");
        Instantiate(GameManager);
        SceneManager.LoadScene(1);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

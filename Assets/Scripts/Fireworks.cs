using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour {

    //private static GameObject fireworks;
    public GameObject fireworks;
    
	void Start ()
    {
        fireworks.SetActive(false);
	}
	
	public void Fire()
    {
        fireworks.SetActive(true);
    }
}

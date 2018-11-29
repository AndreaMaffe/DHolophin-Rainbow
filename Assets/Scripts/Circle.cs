using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(Color color)
    {
        transform.Find("CircleObject").GetComponent<MeshRenderer>().material.color = color;
    }

    public void ResetColor()
    {
        transform.Find("CircleObject").GetComponent<MeshRenderer>().material.color = Color.grey;
    }

}

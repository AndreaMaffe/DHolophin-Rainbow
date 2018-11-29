using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class Sphere : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler {


    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("ciao");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

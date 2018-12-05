using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour, IFocusable
{
    private bool focused;
    private bool active; //true if game is started and it's possible to change color

    void Update()
    {
        //All these functions are temporary: they will be substituted by dolphin inputs
        if (Input.GetKeyDown("r") && focused && active)
        {
            SetColor(Color.red);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.red);

        }
        if (Input.GetKeyDown("g") && focused && active)
        {
            SetColor(Color.green);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.green);

        }
        if (Input.GetKeyDown("b") && focused && active)
        {
            SetColor(Color.blue);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.blue);

        }
        if (Input.GetKeyDown("w") && focused && active)
        {
            SetColor(Color.white);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.white);

        }
        if (Input.GetKeyDown("c") && focused && active)
        {
            SetColor(Color.cyan);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.cyan);
        }
            
        if (Input.GetKeyDown("y") && focused && active)
        {
            SetColor(Color.yellow);
            transform.parent.GetComponent<CirclePanel>().OnCircleColored(this, Color.yellow);
        }
    }

    public void SetColor(Color color)
    {
        transform.Find("CircleObject").GetComponent<MeshRenderer>().material.color = color;
    }

    public void ResetColor()
    {
        transform.Find("CircleObject").GetComponent<MeshRenderer>().material.color = Color.grey;
    }

    public void OnFocusEnter()
    {
        focused = true;
    }

    public void OnFocusExit()
    {
        focused = false;
    }

    public void SetActive(bool value)
    {
        active = value;
    }


}

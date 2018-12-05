using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour, IFocusable
{
    private CirclePanel circlePanel;
    private bool focused; //true if the player is currently looking at the circle
    private bool active; //true if game is started and it's possible to change color

    void Start()
    {
        DolphinManager.OnColorSubmitted += OnColorSubmitted;
        circlePanel = transform.parent.GetComponent<CirclePanel>();
    }

    void OnColorSubmitted()
    {
        if (active && focused)
        {
            SetColor(DolphinManager.CurrentDoplhinColor);
            circlePanel.OnCircleColored(this, DolphinManager.CurrentDoplhinColor);
        }
    }

    public void SetColor(Color color)
    {
        transform.Find("CircleObject").GetComponent<MeshRenderer>().material.color = color;
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

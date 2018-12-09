using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : FocusableButton, IInputClickHandler 
{
    private Text text;
    private CirclePanel circlePanel;

    void Start()
    {
        text = transform.Find("Button_Visual").Find("Description").GetComponent<Text>();
        circlePanel = transform.root.GetComponent<CirclePanel>();

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        AudioManager.PlayPop2();

        if (text.text == "PLAY!")
        {
            circlePanel.PlayGame();
            DolphinManager.SwitchDolphinOn();
            text.text = "STOP";
        }

        else
        {
            circlePanel.SwitchCirclesOff();
            DolphinManager.SwitchDolphinOff();
            text.text = "PLAY!";
        }    
    }


    
}




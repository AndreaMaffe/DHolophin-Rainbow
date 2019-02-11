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

    //called when player click on the button
    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();
    }

    protected override void OnClick()
    {
       if (focused)
        {
            AudioManager.instance.PlayPop2();

            if (text.text == "PLAY!")
            {
                GameManager.instance.PlayAGame();
                GameManager.instance.GameStarted = true;
                text.text = "STOP";
            }

            else
            {
                GameManager.instance.GameStarted = false;
                circlePanel.SwitchCirclesOff();
                text.text = "PLAY!";
            }
        }
    }
}




using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{

    public Text text;

    private void Start()
    {
        text = transform.Find("Button_Visual").Find("Text").GetComponent<Text>();
        text.text = "MANUAL";
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (text.text == "MANUAL")
        {
            text.fontSize = 60;
            text.text = "AUTO";
            GameManager.instance.Mode = GameMode.AUTOMATIC;
        }

        else
        {
            text.fontSize = 50;
            text.text = "MANUAL";
            GameManager.instance.Mode = GameMode.MANUAL;

        }
    }
}

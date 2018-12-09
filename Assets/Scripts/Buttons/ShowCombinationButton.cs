using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class ShowCombinationButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    private CirclePanel circlePanel;
    private Text text;

    void Start()
    {
        circlePanel = transform.root.GetComponent<CirclePanel>();
        text = transform.Find("Button_Visual").Find("Description").GetComponent<Text>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        AudioManager.PlayPop2();

        if (text.text == "Show combination")
        {
            circlePanel.SetCirclesActive(false);
            circlePanel.ShowCombination();
            text.text = "Hide combination";
        }

        else
        {
            circlePanel.SwitchCirclesOff();
            circlePanel.SetCirclesActive(true);
            text.text = "Show combination";
        }
    }
}
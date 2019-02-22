using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class ShowCombinationButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    private CirclePanel circlePanel;
    private Text text;
    private bool clickable;

    void Start()
    {
        circlePanel = transform.root.GetComponent<CirclePanel>();
        text = transform.Find("Button_Visual").Find("Description").GetComponent<Text>();
        clickable = true;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();
    }

    void HideCombination()
    {
        GameManager.instance.circlePanel.SwitchCirclesOn(GameManager.instance.playerGuess);
        clickable = true;
    }

        protected override void OnClick()
    {
        if (focused)
        {
            if (clickable == true)
            {
                AudioManager.instance.PlayPop2();
                GameManager.instance.ShowCombination();
                Invoke("HideCombination", 2f);
                clickable = false;
            }
        }
    }
}
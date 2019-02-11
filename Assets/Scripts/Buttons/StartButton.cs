using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class StartButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();
    }

    protected override void OnClick()
    {
        if (focused)
        {
            AudioManager.instance.PlayPop2();
            GameManager.instance.StartNewGame();
            DolphinManager.instance.Connect();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenuButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();  
    }

    protected override void OnClick()
    {
        if (focused)
        {
            GameManager.instance.GameStarted = false;
            AudioManager.instance.PlayPop2();
            GameManager.instance.LoadScene(1);
        }
    }
}
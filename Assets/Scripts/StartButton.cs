using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class StartButton : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
{

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameManager.StartNewGame();
    }
}

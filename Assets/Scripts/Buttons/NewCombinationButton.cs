using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class NewCombinationButton : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameManager.GenerateNewColorsCombination();
    }
}
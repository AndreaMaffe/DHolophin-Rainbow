// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;


public class PanelNumberButton : FocusableButton, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    public int Number { get; private set; }
    public int minNumber;
    public int maxNumber;


    private Text text;

    void Start()
    {
        text = transform.Find("Button_Visual").Find("Number").GetComponent<Text>();
        Number = minNumber;
    }

    private void Update()
    {
        text.text = Number.ToString();
    }

    //called when player clicks on the button
    public void OnInputClicked(InputClickedEventData eventData)
    {
        AudioManager.instance.PlayPop1();

        Number++;
        if (Number > maxNumber)
            Number = minNumber;

        eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
    }
}
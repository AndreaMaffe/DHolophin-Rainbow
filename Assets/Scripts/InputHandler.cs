using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;
    public static Color CurrentColor { get; private set; }

    // Use this for initialization
    void Start ()
    {
        CurrentColor = GameManager.PossibleColors[0];
    }


    private void Update()
    {
        if (Input.GetKeyDown("r"))
            CurrentColor = Color.red;

        if (Input.GetKeyDown("g"))
            CurrentColor = Color.green;

        if (Input.GetKeyDown("b"))
            CurrentColor = Color.blue;

        if (Input.GetKeyDown("w"))
            CurrentColor = Color.white;

        if (Input.GetKeyDown("c"))
            CurrentColor = Color.cyan;

        if (Input.GetKeyDown("y"))
            CurrentColor = Color.yellow;

        if (Input.GetKeyDown("l") && GameManager.Mode == GameMode.MANUAL)
            OnNextColor();

        if (Input.GetKeyDown("k") && GameManager.Mode == GameMode.MANUAL)
            OnPreviousColor();

        if (Input.GetKeyDown(KeyCode.Space))
            OnColorSubmitted();
    }

    public static void OnNextColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) + 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[0]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());

        //SwitchDolphinOn();
    }

    public static void OnPreviousColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) - 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[GameManager.PossibleColors.Length - 1]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());

        //SwitchDolphinOn();
    }

    public static void SubmitColor()
    {
        OnColorSubmitted();
    }


}

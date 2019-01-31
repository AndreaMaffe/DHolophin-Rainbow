using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;
    public static Color CurrentColor { get; private set; }

    private float timer;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
        CurrentColor = GameManager.PossibleColors[0];
        timer = GameManager.TimeOn;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.red;

        if (Input.GetKeyDown("g") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.green;

        if (Input.GetKeyDown("b") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.blue;

        if (Input.GetKeyDown("w") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.white;

        if (Input.GetKeyDown("c") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.cyan;

        if (Input.GetKeyDown("y") && GameManager.Mode == GameMode.MANUAL)
            CurrentColor = Color.yellow;

        if (Input.GetKeyDown("l") && GameManager.Mode == GameMode.MANUAL)
            OnNextColor();

        if (Input.GetKeyDown("k") && GameManager.Mode == GameMode.MANUAL)
            OnPreviousColor();

        if (Input.GetKeyDown(KeyCode.Space))
            OnColorSubmitted();

        if (GameManager.Mode == GameMode.AUTOMATIC)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                OnNextColor();
                timer = GameManager.TimeOn;
            }
        }
    }

    public static void OnNextColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) + 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[0]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());
    }

    public static void OnPreviousColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) - 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[GameManager.PossibleColors.Length - 1]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());
    }

    public static void SubmitColor()
    {
        OnColorSubmitted();
    }




}

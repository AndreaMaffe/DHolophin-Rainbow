using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public static InputHandler instance = null;

    public delegate void ColorSubmittedEvent();
    public event ColorSubmittedEvent OnColorSubmitted;
    public Color CurrentColor { get; private set; }

    private float timer;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
        CurrentColor = GameManager.instance.allColors[0];
        timer = GameManager.instance.TimeOn;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.red;

        if (Input.GetKeyDown("g") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.green;

        if (Input.GetKeyDown("b") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.blue;

        if (Input.GetKeyDown("w") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.white;

        if (Input.GetKeyDown("c") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.cyan;

        if (Input.GetKeyDown("y") && GameManager.instance.Mode == GameMode.MANUAL)
            CurrentColor = Color.yellow;

        if (Input.GetKeyDown("l") && GameManager.instance.Mode == GameMode.MANUAL)
            OnNextColor();

        if (Input.GetKeyDown("k") && GameManager.instance.Mode == GameMode.MANUAL)
            OnPreviousColor();

        if (Input.GetKeyDown(KeyCode.Space))
            OnColorSubmitted();

        if (GameManager.instance.Mode == GameMode.AUTOMATIC)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                OnNextColor();
                timer = GameManager.instance.TimeOn;
            }
        }
    }

    public void OnNextColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) + 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[0]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());
    }

    public void OnPreviousColor()
    {
        try
        {
            CurrentColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentColor) - 1];
        } catch (IndexOutOfRangeException e) { CurrentColor = GameManager.PossibleColors[GameManager.PossibleColors.Length - 1]; }

        Debug.Log("New color selected: " + CurrentColor.ToString());
    }

    public void SubmitColor()
    {
        OnColorSubmitted();
    }




}

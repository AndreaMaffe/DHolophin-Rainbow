using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public static InputHandler instance = null;

    public delegate void ColorSubmittedEvent();
    public event ColorSubmittedEvent OnColorSubmitted;

    public delegate void ColorChangedEvent();
    public event ColorSubmittedEvent OnColorChanged;

    private float timer;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
        timer = 6;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            OnNextColor();

        if (Input.GetKeyDown(KeyCode.A))
            OnNextColor();

        if (Input.GetKeyDown(KeyCode.S))
            OnColorSubmitted();

        if (GameManager.instance.Mode == GameMode.AUTOMATIC && GameManager.instance.GameStarted)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                try
                {
                    GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[Array.IndexOf(GameManager.instance.PossibleColors, GameManager.instance.CurrentColor) + 1];
                }
                catch (IndexOutOfRangeException e) { GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[0]; }

                OnColorChanged();
                timer = 6;
            }
        }
    }

    public void OnNextColor()
    {
        if (GameManager.instance.Mode == GameMode.MANUAL && GameManager.instance.GameStarted)
        {
            try
            {
                GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[Array.IndexOf(GameManager.instance.PossibleColors, GameManager.instance.CurrentColor) + 1];
            }
            catch (IndexOutOfRangeException e) { GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[0]; }

            OnColorChanged();
        }
    }

    public void OnPreviousColor()
    {
        if (GameManager.instance.Mode == GameMode.MANUAL && GameManager.instance.GameStarted)
        {
            try
            {
                GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[Array.IndexOf(GameManager.instance.PossibleColors, GameManager.instance.CurrentColor) - 1];
            }
            catch (IndexOutOfRangeException e) { GameManager.instance.CurrentColor = GameManager.instance.PossibleColors[GameManager.instance.PossibleColors.Length - 1]; }

            OnColorChanged();
        }

    }

    public void SubmitColor()
    {
        OnColorSubmitted();
    }




}

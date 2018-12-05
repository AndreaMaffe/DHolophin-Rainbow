using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinManager : MonoBehaviour
{
    private static string ipAddr = ""; //Dolphin IP address
    private static int port = 0; //Dolphin port

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;

    public static Color CurrentDoplhinColor { get; private set; }

    void Start()
    {
        CurrentDoplhinColor = GameManager.PossibleColors[0];
    }

    private void Update()
    {
        //temporary, they will substituted by dolphin inputs
        if (Input.GetKeyDown("r"))        
            SetColor(Color.red);
        
        if (Input.GetKeyDown("g"))        
            SetColor(Color.green);
        
        if (Input.GetKeyDown("b"))        
            SetColor(Color.blue);
        
        if (Input.GetKeyDown("w"))        
            SetColor(Color.white);         

        if (Input.GetKeyDown("c"))        
            SetColor(Color.cyan);        

        if (Input.GetKeyDown("y"))        
            SetColor(Color.yellow);

        if (Input.GetKeyDown("l"))
            OnNextColor();

        if (Input.GetKeyDown("k"))
            OnPreviousColor();

        if (Input.GetKeyDown(KeyCode.Space))
            OnColorSubmitted();
    }

    void OnNextColor()
    {
        try
        {
            CurrentDoplhinColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentDoplhinColor) + 1];
        }  catch (IndexOutOfRangeException e) { CurrentDoplhinColor = GameManager.PossibleColors[0]; }

        Debug.Log("New color selected: " + CurrentDoplhinColor.ToString());
    }

    void OnPreviousColor()
    {
        try
        {
            CurrentDoplhinColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentDoplhinColor) - 1];
        }   catch (IndexOutOfRangeException e) { CurrentDoplhinColor = GameManager.PossibleColors[GameManager.PossibleColors.Length-1]; }

        Debug.Log("New color selected: " + CurrentDoplhinColor.ToString());
    }

    void SetColor(Color color)
    {
        CurrentDoplhinColor = color;
    }

}

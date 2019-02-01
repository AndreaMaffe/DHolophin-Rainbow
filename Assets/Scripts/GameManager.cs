using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { MANUAL, AUTOMATIC }

public class GameManager : MonoBehaviour
{
    private static GameObject dolphinManager;
    private static GameObject inputHandler;

    public static GameMode Mode { get; private set; }

    public static string stringa = "not working!";

    public static int NumberOfCircles { get; private set; }
    public static int NumberOfColors { get; private set; }
    public static float TimeOn { get; private set; }

    //all possible colors for all games
    private static Color[] allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.white };
    //all possible color for this game
    public static Color[] PossibleColors { get; private set; }
    //actual combination
    public static Color[] ColorCombination { get; private set; }

    private static Fireworks f;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
        AudioManager.PlayBackgroundMusic();
	}

    public static void StartNewGame()
    {
        Mode = GameMode.MANUAL;
        //get game parameters from the panel
        NumberOfCircles = GameObject.Find("CircleNumberButton").GetComponent<PanelNumberButton>().Number;
        NumberOfColors = GameObject.Find("ColorNumberButton").GetComponent<PanelNumberButton>().Number;
        TimeOn = 3; //temporary

        //load the scene
        LoadScene(2);

        //initialize game data
        PossibleColors = new Color[NumberOfColors];
        for (int i = 0; i < NumberOfColors; i++)
            PossibleColors[i] = allColors[i];
        GenerateNewColorsCombination();

        if (dolphinManager == null)
            dolphinManager = Instantiate(Resources.Load<GameObject>("Prefabs/DolphinManager"));
        if (inputHandler == null)
            inputHandler = Instantiate(Resources.Load<GameObject>("Prefabs/InputHandler"));
    }

    public static void LoadScene (int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").rotation = Quaternion.identity;
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").position = Vector3.zero;
    }

    public static void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        //gives each circle a random color taken from the possible ones
        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[UnityEngine.Random.Range(0, NumberOfColors)];        
    }

    //return true if the guess corresponds to the combination, false otherwise
    public static bool CheckPlayerGuess(Color[] playerGuess)
    {
        bool guessIsCorrect = true;

        for (int i = 0; i < NumberOfCircles; i++)
            if (playerGuess[i] != ColorCombination[i])
                guessIsCorrect = false;

        if (guessIsCorrect)
        {
            AudioManager.PlayCorrectAnswerSound();
            ShootFireworks();
            return true;
        }

        else
        {
            AudioManager.PlayWrongAnswerSound();
            return false;
        }
    }

    public static void ShootFireworks()
    {
        f.Fire();
    }
}

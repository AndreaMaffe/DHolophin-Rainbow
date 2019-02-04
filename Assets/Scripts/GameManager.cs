using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { MANUAL, AUTOMATIC }

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameMode Mode { get; private set; }

    public int NumberOfCircles { get; private set; } 
    public int NumberOfColors { get; private set; }
    public float TimeOn { get; private set; } //time given to the player to memorize the combination

    //all possible colors for all games
    public Color[] allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.white };
    //all possible color for this game
    public Color[] PossibleColors { get; private set; }
    //actual combination
    public Color[] ColorCombination { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    void Start ()
    {
        DontDestroyOnLoad(this);
        AudioManager.instance.PlayBackgroundMusic();
    }

    public void StartNewGame()
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
    }

    public void LoadScene (int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").rotation = Quaternion.identity;
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").position = Vector3.zero;
    }

    public void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        //gives each circle a random color taken from the possible ones
        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[UnityEngine.Random.Range(0, NumberOfColors)];        
    }

    //return true if the guess corresponds to the combination, false otherwise
    public bool CheckPlayerGuess(Color[] playerGuess)
    {
        bool guessIsCorrect = true;

        for (int i = 0; i < NumberOfCircles; i++)
            if (playerGuess[i] != ColorCombination[i])
                guessIsCorrect = false;

        if (guessIsCorrect)
        {
            AudioManager.instance.PlayCorrectAnswerSound();
            ShootFireworks();
            return true;
        }

        else
        {
            AudioManager.instance.PlayWrongAnswerSound();
            return false;
        }
    }

    public void ShootFireworks()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Fireworks"));
        AudioManager.instance.PlayFireworksSound();
    }
}

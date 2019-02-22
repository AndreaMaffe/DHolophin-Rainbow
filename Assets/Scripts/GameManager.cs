using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { MANUAL, AUTOMATIC }

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameMode Mode { get; set; }  //AUTOMATIC or MANUAL
    public bool GameStarted { get; set; } 
    public float TimeOn { get; private set; } //time given to the player to memorize the combination

    public int NumberOfCircles { get; private set; } 
    public int NumberOfColors { get; private set; }

    //all possible colors for all games
    public Color[] allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.white };
    //all possible color for this game
    public Color[] PossibleColors { get; private set; }
    //actual combination
    public Color[] ColorCombination { get; private set; }
    //player combination
    public Color[] playerGuess;

    public Color CurrentColor { get; set; }

    public Dolphin dolphin;
    public CirclePanel circlePanel;

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
        Mode = GameMode.MANUAL;
        GameStarted = false;
        DontDestroyOnLoad(this);
        CurrentColor = allColors[0];
        AudioManager.instance.PlayBackgroundMusic();
    }

    public void StartNewGame()
    {
        //get game parameters from the panel
        NumberOfCircles = GameObject.Find("CircleNumberButton").GetComponent<PanelNumberButton>().Number;
        NumberOfColors = GameObject.Find("ColorNumberButton").GetComponent<PanelNumberButton>().Number;
        TimeOn = 3; //temporary

        //load the scene
        LoadScene(2);

        //initialize game data
        playerGuess = new Color[NumberOfCircles];
        PossibleColors = new Color[NumberOfColors];
        for (int i = 0; i < NumberOfColors; i++)
            PossibleColors[i] = allColors[i];

        GenerateNewColorsCombination();
    }

    public void PlayAGame()
    {
        //set player guess to default gray
        for (int i = 0; i < NumberOfCircles; i++)
            playerGuess[i] = Color.gray;

        Invoke("ShowCombination", 2f);
        Invoke("SwitchCirclesOff", 2 + TimeOn);
        circlePanel.SetCirclesActive(true);
    }

    //called when player submits one color to a circle
    public void OnCircleColored(Circle circle, Color circleColor)
    {
        //insert player choice in the combination
        int index = Array.IndexOf(circlePanel.Circles, circle);
        playerGuess[index] = circleColor;

        //check if all the circles are colored
        bool allCirclesAreColored = true;

        for (int i = 0; i < NumberOfCircles; i++)
            if (playerGuess[i] == Color.gray)
                allCirclesAreColored = false;

        //if so, check if the combination is correct
        if (allCirclesAreColored)
        {
            CheckPlayerGuess();
            circlePanel.SetCirclesActive(false);
        }
    }

        public void LoadScene (int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").rotation = Quaternion.identity;
        GameObject.Find("MixedRealityCameraParent").transform.Find("MixedRealityCamera").position = Vector3.zero;
    }

    public void ShowCombination()
    {
        circlePanel.SwitchCirclesOn(ColorCombination);
    }

    public void SwitchCirclesOff()
    {
        circlePanel.SwitchCirclesOff();
    }

    public void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        //gives each circle a random color taken from the possible ones
        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[UnityEngine.Random.Range(0, NumberOfColors)];        
    }

    //return true if the guess corresponds to the combination, false otherwise
    public void CheckPlayerGuess()
    {
        bool guessIsCorrect = true;

        for (int i = 0; i < NumberOfCircles; i++)
            if (playerGuess[i] != ColorCombination[i])
                guessIsCorrect = false;

        if (guessIsCorrect)
        {
            AudioManager.instance.PlayCorrectAnswerSound();
            DolphinManager.instance.MakeDolphinHappy();
            GenerateNewColorsCombination();
            dolphin.SetHappySprite();
            //ShootFireworks();
            Invoke("SwitchCirclesOff", 5f);
            Invoke("PlayAGame", 4f);
        }

        else
        {
            AudioManager.instance.PlayWrongAnswerSound();
            DolphinManager.instance.MakeDolphinSad();
            dolphin.SetAngrySprite();
            Invoke("SwitchCirclesOff", 1f);
            Invoke("PlayAGame", 0.5f);
        }

    }

    public void ShootFireworks()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Fireworks"), new Vector3(circlePanel.transform.position.x, circlePanel.transform.position.y +1, circlePanel.transform.position.z +2), Quaternion.identity);
        AudioManager.instance.PlayFireworksSound();
    }
}

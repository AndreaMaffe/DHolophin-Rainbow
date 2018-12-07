using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { MANUAL, AUTOMATIC }

public class GameManager : MonoBehaviour
{
    private static GameObject dolphinManager;
    public List<GameObject> dontDestroyOnLoadObjects;

    public static GameMode Mode { get; private set; }

    public static int NumberOfCircles { get; private set; }
    public static int NumberOfColors { get; private set; }
    public static float TimeOn { get; private set; }

    //all possible colors for all games
    private static Color[] allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.white };
    //all possible color for this game
    public static Color[] PossibleColors { get; private set; }
    //actual combination
    public static Color[] ColorCombination { get; private set; }

    // Use this for initialization
    void Start ()
    {
        /*foreach (GameObject gameObj in dontDestroyOnLoadObjects)
            DontDestroyOnLoad(gameObj);*/

        AudioManager.PlayBackgroundMusic();
	}

    public static void StartNewGame()
    {
        if (DolphinManager.EstablishCommunication())
        {
            Mode = GameMode.MANUAL;
            //get game parameters from the panel
            NumberOfCircles = GameObject.Find("CircleNumberButton").GetComponent<PanelNumberButton>().Number;
            NumberOfColors = GameObject.Find("ColorNumberButton").GetComponent<PanelNumberButton>().Number;
            TimeOn = 3; //temporary

            //load the scene
            //SceneManager.LoadScene(1);

            Instantiate(Resources.Load<GameObject>("Prefabs/CirclePanel"), new Vector3(0, 0, 1.5f), Quaternion.identity);
            Destroy(GameObject.Find("MainPanel"));
            Destroy(GameObject.Find("MainPanel(Clone)"));

            //initialize game data
            PossibleColors = new Color[NumberOfColors];
            for (int i = 0; i < NumberOfColors; i++)
                PossibleColors[i] = allColors[i];
            GenerateNewColorsCombination();

            if (dolphinManager == null)
                dolphinManager = Instantiate(Resources.Load<GameObject>("Prefabs/DolphinManager"));
        }

        else Debug.Log("IMPOSSIBILE CONNETTERSI AL DELFINO!");

    }

    public static void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        //gives each circle a random color taken from the possible ones
        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[UnityEngine.Random.Range(0, NumberOfColors)];        
    }

    public static bool CheckPlayerGuess(Color[] playerGuess)
    {
        bool guessIsCorrect = true;

        for (int i = 0; i < NumberOfCircles; i++)
            if (playerGuess[i] != ColorCombination[i])
                guessIsCorrect = false;

        if (guessIsCorrect)
        {
            AudioManager.PlayCorrectAnswerSound();
            Debug.Log("*** BRAVO! Hai indovinato! ***");
            return true;
        }

        else
        {
            AudioManager.PlayWrongAnswerSound();
            Debug.Log("*** ERRORE! Combinazione sbagliata! ***");
            return false;
        }
    }


}

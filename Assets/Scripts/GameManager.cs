using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> dontDestroyOnLoadObjects;

    public static int NumberOfCircles { get; private set; }
    public static int NumberOfColors { get; private set; }
    public static float TimeOn { get; private set; }

    //all possible colors
    private static Color[] allColors;
    //actual combination
    public static Color[] ColorCombination { get; private set; }

    // Use this for initialization
    void Start ()
    {
        //set all the possible colors for the game
        allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.cyan };

        foreach (GameObject gameObj in dontDestroyOnLoadObjects)
            DontDestroyOnLoad(gameObj);
	}

    public static void StartNewGame()
    {
        NumberOfCircles = GameObject.Find("CircleNumberButton").GetComponent<PanelNumberButton>().Number;
        NumberOfColors = GameObject.Find("ColorNumberButton").GetComponent<PanelNumberButton>().Number;
        TimeOn = 5; //temporary

        SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single);

        GenerateNewColorsCombination();
    }

    public static void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        //gives each circle a random color taken from the possible ones
        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[Random.Range(0, NumberOfColors)];        
    }


}

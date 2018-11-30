using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> dontDestroyOnLoadObjects;

    public static int NumberOfCircles { get; private set; }
    public static int NumberOfColors { get; private set; }

    private static Color[] allColors; 
    public static Color[] ColorCombination { get; private set; }

    // Use this for initialization
    void Start ()
    {
        allColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.cyan };
        foreach (GameObject gameObj in dontDestroyOnLoadObjects)
            DontDestroyOnLoad(gameObj);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static void StartNewGame()
    {
        NumberOfCircles = GameObject.Find("CircleNumberButton").GetComponent<PanelNumberButton>().Number;
        NumberOfColors = GameObject.Find("ColorNumberButton").GetComponent<PanelNumberButton>().Number;

        SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single);

        GenerateNewColorsCombination();
    }

    static void GenerateNewColorsCombination()
    {
        ColorCombination = new Color[NumberOfCircles];

        for (int i = 0; i < NumberOfCircles; i++)
            ColorCombination[i] = allColors[Random.Range(0, NumberOfColors)];        
    }


}

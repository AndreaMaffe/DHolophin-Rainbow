using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<GameObject> dontDestroyOnLoadObjects;

    public static int NumberOfCircles { get; private set; }
    public static int NumberOfColors { get; private set; }

    public static Color[] ColorCombination { get; private set; }

    // Use this for initialization
    void Start ()
    {
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

        for (int i=0; i<NumberOfCircles; i++)
        {
            float r = Random.Range(0, 255);
            float g = Random.Range(0, 255);
            float b = Random.Range(0, 255);

            Color color = new Color(r, g, b);

            ColorCombination[i] = color;
        }
    }


}

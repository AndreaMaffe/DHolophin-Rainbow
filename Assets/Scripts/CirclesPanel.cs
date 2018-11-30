using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclesPanel : MonoBehaviour {

    private RectTransform rectTransform;
    private int numberOfCircles;
    private Color[] colorCombination;
    private Circle[] circles;

	// Use this for initialization
	void Start ()
    {
        rectTransform = transform.Find("Panel").GetComponent<RectTransform>();
        numberOfCircles = GameManager.NumberOfCircles;
        colorCombination = GameManager.ColorCombination;

        CreateCircles(numberOfCircles);
        SwitchColorsOff();
	}

    public void CreateCircles(int numberOfCircles)
    {
        circles = new Circle[numberOfCircles];

        //scale the panel according to the number of circles
        float unit = rectTransform.localScale.x;
        rectTransform.localScale += new Vector3(numberOfCircles*unit, 0, 0);     

        for (int i=0; i<numberOfCircles; i++)
        {
            //calculate circle position based on panel scale and number of circles
            Vector3 newCirclePosition = new Vector3(rectTransform.position.x - (numberOfCircles-1) * unit / 2 + i*unit, rectTransform.position.y + 0.025f, rectTransform.position.z);

            //generate circle and add it to the list
            GameObject circle = Instantiate(Resources.Load<GameObject>("Prefabs/Circle"), newCirclePosition, Quaternion.identity);
            circle.transform.parent = this.transform;
            circles[i] = circle.GetComponent<Circle>();
        }                       
    }

    public void SwitchColorsOn()
    {
        for (int i = 0; i < numberOfCircles; i++) 
            circles[i].SetColor(colorCombination[i]);
    }

    public void SwitchColorsOff()
    {
        for (int i = 0; i < numberOfCircles; i++)
            circles[i].SetColor(Color.grey);
    }

    public void Play()
    {
        Invoke("SwitchColorOn", 2f);
        Invoke("SwitchColorsOff", GameManager.TimeOn);
    }



}

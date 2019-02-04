using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePanel : MonoBehaviour {

    private RectTransform rectTransform;
    private int numberOfCircles;
    public Circle[] Circles { get; private set; }
    public GameObject dolphin;

	// Use this for initialization
	void Start ()
    {
        rectTransform = transform.Find("Panel").GetComponent<RectTransform>();
        numberOfCircles = GameManager.instance.NumberOfCircles;
        GameManager.instance.circlePanel = this;
        CreateCircles(numberOfCircles);
	}

    private void CreateCircles(int numberOfCircles)
    {
        Circles = new Circle[numberOfCircles];

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
            Circles[i] = circle.GetComponent<Circle>();
        }

        //create the dolphin, scale it and add it as child
        Vector3 dolphinPosition = new Vector3(rectTransform.position.x + unit*numberOfCircles/2, rectTransform.position.y + rectTransform.localScale.y/2, rectTransform.position.z-0.02f);
        dolphin = Instantiate(Resources.Load<GameObject>("Prefabs/Dolphin"), dolphinPosition, Quaternion.identity);
        dolphin.transform.localScale *= 0.6f;
        dolphin.transform.parent = this.transform;

        SwitchCirclesOff();
    }

    //color circles with the actual combination
    public void SwitchCirclesOn(Color[] colorCombination)
    {
        for (int i = 0; i < numberOfCircles; i++) 
            Circles[i].SetColor(colorCombination[i]);
    }

    //set circles to default grey color
    public void SwitchCirclesOff()
    {
        for (int i = 0; i < numberOfCircles; i++)
            Circles[i].SetColor(Color.grey);
    }

    //mark the circles as active
    public void SetCirclesActive(bool value)
    {
        for (int i = 0; i < numberOfCircles; i++)
            Circles[i].SetActive(value);
    }


}

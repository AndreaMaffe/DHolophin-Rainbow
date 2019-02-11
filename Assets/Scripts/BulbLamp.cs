using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbLamp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GameManager.instance.CurrentColor;
        InputHandler.instance.OnColorChanged += ChangeSpriteColor;
    }

    private void ChangeSpriteColor()
    {
        spriteRenderer.color = GameManager.instance.CurrentColor;
    }

    private void OnDestroy()
    {
        InputHandler.instance.OnColorChanged -= ChangeSpriteColor;
    }
}

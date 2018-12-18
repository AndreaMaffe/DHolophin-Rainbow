using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dolphin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite happySprite, defaultSprite;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        happySprite = Resources.Load<Sprite>("Sprites/DolphinHappy");
        defaultSprite = Resources.Load<Sprite>("Sprites/DolphinDefault");
    }

    public void SetHappySprite()
    {
        spriteRenderer.sprite = happySprite;
        Invoke("SetDefaultSprite", 1f);
    }

    public void SetDefaultSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }

}

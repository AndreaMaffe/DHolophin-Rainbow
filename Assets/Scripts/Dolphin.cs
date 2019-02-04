using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dolphin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite happySprite, angrySprite, defaultSprite;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        happySprite = Resources.Load<Sprite>("Sprites/DolphinHappy");
        angrySprite = Resources.Load<Sprite>("Sprites/DolphinAngry");
        defaultSprite = Resources.Load<Sprite>("Sprites/DolphinDefault");

        GameManager.instance.dolphin = this;
    }

    public void SetHappySprite()
    {
        spriteRenderer.sprite = happySprite;
        Invoke("SetDefaultSprite", 5f);

    }

    public void SetAngrySprite()
    {
        spriteRenderer.sprite = angrySprite;
        Invoke("SetDefaultSprite", 1.5f);
    }

    public void SetDefaultSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }

}

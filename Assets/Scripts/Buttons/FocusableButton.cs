using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FocusableButton : MonoBehaviour, IFocusable
{
    public Animator animator;
    protected bool focused;

    private void Start()
    {
        InputHandler.instance.OnColorSubmitted += OnClick;
    }

    protected abstract void OnClick();

    //called when player focuses on the button
    public void OnFocusEnter()
    {
        animator.SetBool("Focused", true);
        focused = true;
    }

    //called when player exits the focus on the button
    public void OnFocusExit()
    {
        animator.SetBool("Focused", false);
        focused = false;
    }
}

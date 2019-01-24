using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusableButton : MonoBehaviour, IFocusable
{
    public Animator animator;

    //called when player focuses on the button
    public void OnFocusEnter()
    {
        animator.SetBool("Focused", true);

    }

    //called when player exits the focus on the button
    public void OnFocusExit()
    {
        animator.SetBool("Focused", false);
    }
}

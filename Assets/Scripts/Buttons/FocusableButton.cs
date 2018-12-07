using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusableButton : MonoBehaviour, IFocusable
{
    public Animator animator;

    public void OnFocusEnter()
    {
        animator.SetBool("Focused", true);

    }

    public void OnFocusExit()
    {
        animator.SetBool("Focused", false);
    }
}

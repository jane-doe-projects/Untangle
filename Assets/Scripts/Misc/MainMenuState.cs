using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : MonoBehaviour
{
    static Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public static bool MenuHidden()
    {
        return anim.GetBool("MenuHidden");
    }
}

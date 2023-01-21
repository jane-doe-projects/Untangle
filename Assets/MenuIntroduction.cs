using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIntroduction : MonoBehaviour
{
    // display of bouncing indicator for where to find the menu
    // only displayed on the first two starts (when/while the set playerprefs variable is < 2)

    int currentDisplayValue = 0;

    [SerializeField] GameObject indicator;
    CanvasGroup cGroup;
    float fadeSpeed = 0.5f;

    void Start()
    {
        MenuAnimation.menuHasOpenedDelegate += DeactivateIndication;
        InitializeIndicator();
    }

    void InitializeIndicator()
    {
        currentDisplayValue = PlayerPrefs.GetInt("MenuIndicator");

        if (currentDisplayValue < 2)
            Indicate();
    }

    void Indicate()
    {
        currentDisplayValue++;
        PlayerPrefs.SetInt("MenuIndicator", currentDisplayValue);
        indicator.SetActive(true);
        cGroup = indicator.GetComponent<CanvasGroup>();
    }

    void DeactivateIndication()
    {
        MenuAnimation.menuHasOpenedDelegate -= DeactivateIndication;
        Animator anim = indicator.GetComponent<Animator>();
        anim.SetTrigger("Fade");
    }
}

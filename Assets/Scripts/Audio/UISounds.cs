using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] AudioSource menu;
    [SerializeField] AudioSource button;
    [SerializeField] AudioSource buttonHover;
    [SerializeField] AudioSource popupMessage;

    public void Hover()
    {
        buttonHover.PlayOneShot(buttonHover.clip);
    }

    public void Click()
    {
        button.PlayOneShot(button.clip);
    }

    public void PlayMenu()
    {
        menu.PlayOneShot(menu.clip);
    }

    public void PlayPopup()
    {
        popupMessage.PlayOneShot(popupMessage.clip);
    }
}

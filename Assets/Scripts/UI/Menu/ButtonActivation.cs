using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonActivation : MonoBehaviour
{
    [SerializeField] GameObject confirmButton;

    private void Start()
    {
        confirmButton.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("Toggling");
        confirmButton.SetActive(!confirmButton.activeSelf);
    }

    /*
    private void OnDisable()
    {
        Debug.Log("Disabling");
        confirmButton.SetActive(false);
    } */
}

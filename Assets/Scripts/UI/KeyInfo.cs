using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keyLabel;
    [SerializeField] TextMeshProUGUI functionLabel;

    [SerializeField] string key;
    [SerializeField] string function;


    void Start()
    {
        InitInfo();
    }

    void InitInfo()
    {
        keyLabel.text = key;
        functionLabel.text = function;
    }

}

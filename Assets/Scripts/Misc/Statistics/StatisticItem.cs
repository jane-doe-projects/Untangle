using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI valueLabel;

    string value;

    public void SetLabels(string lab, string valLab)
    {
        label.text = lab;
        valueLabel.text = valLab;
    }
}

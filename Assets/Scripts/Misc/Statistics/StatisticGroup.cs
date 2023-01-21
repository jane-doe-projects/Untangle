using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticGroup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    public GameObject itemParent;

    public void SetLabel(string label)
    {
        title.text = label;
    }
}

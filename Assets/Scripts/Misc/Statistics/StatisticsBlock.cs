using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsBlock : MonoBehaviour
{
    [SerializeField] string labelText;
    [SerializeField] TextMeshProUGUI label;
    public GameObject statsParent;

    [SerializeField] List<StatisticItem> stastisticsList;
    // Start is called before the first frame update
    void Start()
    {
        label.text = labelText;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoItem : MonoBehaviour
{
    [SerializeField] public InfoType type;
    [SerializeField] public TextMeshProUGUI value;
    [SerializeField] TextMeshProUGUI infoNameinfoTitel;

    private void Awake()
    {
        infoNameinfoTitel.text = type.ToString();
    }

}

public enum InfoType
{
    Nodes,
    Sets,
    Mode,
    Progress
}

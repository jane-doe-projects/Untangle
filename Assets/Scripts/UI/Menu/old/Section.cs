using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Section : MonoBehaviour
{
    [SerializeField] string description;
    [SerializeField] GameObject sectionContent;
    [SerializeField] TextMeshProUGUI titel;


    // Start is called before the first frame update
    void Start()
    {
        titel.text = description;
    }

}

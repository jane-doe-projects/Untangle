using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[RequireComponent(typeof(Image))]
public class ContentBox : MonoBehaviour
{
    Image img;

    [SerializeField] string titel;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] TextMeshProUGUI uiTitel;
    [SerializeField] bool descriptionOverridable = false;
    [SerializeField] TextMeshProUGUI uiDescription;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        uiTitel.text = titel;
        if (!descriptionOverridable)
            uiDescription.text = description;
    }

}

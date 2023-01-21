using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadedText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void OnEnable()
    {
        FontSetter.Fade(text);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoverPopout : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Image labelIcon;

    public void Show()
    {
        anim.SetBool("hoverOn", true);
    }

    public void Hide()
    {
        anim.SetBool("hoverOn", false);
    }

    public void SetLabel(string text, Sprite icon)
    {
        label.text = text;
        labelIcon.sprite = icon;
    }
}

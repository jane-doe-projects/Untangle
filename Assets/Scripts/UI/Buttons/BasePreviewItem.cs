using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasePreviewItem : MonoBehaviour
{
    public string label;
    Image img;

    [SerializeField] public Button btn;
    [SerializeField] public TextMeshProUGUI btnLabel;

    public void SetState()
    {
        img = GetComponent<Image>();
        //img.sprite = GameManager.Instance.currentTheme.nodeOptions[0];
        //img.sprite = GameManager.Instance.gameElements.nodeSprite;
        SetOpacity();

        btnLabel.fontSize = 12;
        btnLabel.enabled = true;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickAction);
    }

    public virtual void OnClickAction()
    {
        Debug.Log("Button was pressed. No action set as for now.");
    }

    void Selected()
    {
        ResetOpacity();
    }

    public void Deselect()
    {
        SetOpacity();
    }

    void SetOpacity()
    {
        Color tempCol = img.color;
        tempCol.a = GameManager.Instance.gameElements.opacityValue;
        img.color = tempCol;
    }

    void ResetOpacity()
    {
        img.color = Color.white;
    }
}

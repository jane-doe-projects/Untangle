using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontSetter : MonoBehaviour
{
    List<TextMeshProUGUI> fontElements;
    [SerializeField] TMP_FontAsset defaultFont;

    [Header("Color Sensitive Texts")]
    [SerializeField] List<TextMeshProUGUI> texts;
    [SerializeField] Color alternativeColor;
    static float cutOff = 0.87f;

    [Header("Lightly faded texts")]
    [SerializeField] List<TextMeshProUGUI> fadedTexts;
    [SerializeField] Color fadeAlpha;
    static Color globalfadeAlpha;

    // Start is called before the first frame update
    void Start()
    {
        globalfadeAlpha = fadeAlpha;
        DelegateHandler.onBackgroundChangeDelegate += SetFontColor;

        GetAllFontElements();
        SetDefaultFont();
    }

    void GetAllFontElements()
    {
        fontElements = new List<TextMeshProUGUI>();
        fontElements.AddRange(Resources.FindObjectsOfTypeAll<TextMeshProUGUI>());
    }

    public void UpdateAllFontElements()
    {
        SetFontColor();
    }

    void SetDefaultFont()
    {
        foreach (TextMeshProUGUI element in fontElements)
            element.font = defaultFont;
    }

    void SetFontColor()
    {
        Color col = GameManager.Instance.currentTheme.fontColor;

        foreach (TextMeshProUGUI element in fontElements)
            element.color = col;

        // update special texts to different color
        UpdateColor();
    }

    void UpdateColor()
    {
        Color colorToSet = Color.white;
        Color currentBG = GameManager.Instance.currentTheme.background;
        if (IsTooLight(currentBG))
            colorToSet = alternativeColor;

        foreach (TextMeshProUGUI textEle in texts)
        {
            textEle.color = colorToSet;
            if (fadedTexts.Contains(textEle))
            {
                Color adaptedColor = textEle.color;
                adaptedColor.a = fadeAlpha.a;
                textEle.color = adaptedColor;
            }
        }
    }

    public static bool IsTooLight(Color col)
    {
        if (col.r > cutOff && col.b > cutOff && col.g > cutOff)
            return true;
        return false;
    }

    public static void Fade(TextMeshProUGUI text)
    {
        Color adaptedColor = text.color;
        adaptedColor.a = globalfadeAlpha.a;
        text.color = adaptedColor;
    }
}

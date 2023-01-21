using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinUpdater : MonoBehaviour
{
    [SerializeField] bool isParticleSystem = false;
    [SerializeField] bool isDropDown = false;
    float mouseTrailAlpha = 0.7f;
    [SerializeField] bool opacityOnly;

    void Awake()
    {
        if (!isParticleSystem)
            DelegateHandler.onBackgroundChangeDelegate += UpdateImageColor;
        else
            DelegateHandler.onBackgroundChangeDelegate += UpdatePSColor;


        if (isDropDown)
            DropDownUpdate();
    }

    void UpdateImageColor()
    {
        if (isDropDown)
            DropDownUpdate();
        else
            GetComponent<Image>().color = GameManager.Instance.currentTheme.nodeColor1;
    }

    void DropDownUpdate()
    {
        Color aspectColorForDropdown = GameManager.Instance.currentTheme.background;
        if (FontSetter.IsTooLight(aspectColorForDropdown))
            ColorUtility.TryParseHtmlString("#7E7E7E", out aspectColorForDropdown);
        aspectColorForDropdown.a = 0.7f;

        if (opacityOnly)
        {
            aspectColorForDropdown = Color.white;
            aspectColorForDropdown.a = 0.7f;
        }

        GetComponent<Image>().color = aspectColorForDropdown;
    }


    void UpdatePSColor()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps)
        {
            Color col1 = GameManager.Instance.currentTheme.background;
            col1.a = mouseTrailAlpha;
            Color col2 = Color.white;
            col2.a = mouseTrailAlpha;

            // check if color is too light - if so - replace second color with grey - so mouse trail will be visilbe on light background
            if (FontSetter.IsTooLight(col1))
            {
                col2 = Color.grey;
                col2.a = mouseTrailAlpha;
            }

            ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(col1, col2);
            gradient.mode = ParticleSystemGradientMode.TwoColors;

            ParticleSystem.MainModule psMain = ps.main;
            psMain.startColor = gradient;
        }
    }

    public void IsParticleSystem(bool isPS)
    {
        isParticleSystem = isPS;
        UpdatePSColor();
    }

    private void OnDestroy()
    {
        if (!isParticleSystem)
            DelegateHandler.onBackgroundChangeDelegate -= UpdateImageColor;
        else
            DelegateHandler.onBackgroundChangeDelegate -= UpdatePSColor;
    }
}

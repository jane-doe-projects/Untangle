using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkinElement : MonoBehaviour
{
    [SerializeField] SkinType type;
    [SerializeField] string descriptionName;
    [SerializeField] string descriptionName2;

    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI description2;
    [SerializeField] GameObject selectionFrame;

    [SerializeField] SpriteRenderer previewSpriteRenderer;

    SkinElementSelection elementSelection;
    ColorPickerHandler pickerHandler;

    public string labelColor1;
    public string labelColor2;
    public SkinElementColors skinElementColors;
    Material elementMaterial;

    string colorString1 = "";
    string colorString2 = "";

    GameElementSettings settings;

    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickAction);

        elementSelection = GameManager.Instance.skinElementSelection;
        pickerHandler = GameManager.Instance.colorPickerHandler;

        settings = GameManager.Instance.gameElements;

        description.text = descriptionName;
        description2.text = descriptionName2;
        selectionFrame.SetActive(false);

        InitColorsFromMaterial();
    }

    void OnClickAction()
    {
        elementSelection.SelectElement(this);
    }

    public void Deselect()
    {
        selectionFrame.SetActive(false);
    }

    public void Select()
    {
        selectionFrame.SetActive(true);
    }

    void InitColorsFromMaterial()
    {
        if (skinElementColors == null)
            skinElementColors = new SkinElementColors();

        switch (type)
        {
            case SkinType.Orb:
                elementMaterial = settings.nodeRuntimeMaterials[0];
                colorString1 = "_MainColor";
                colorString2 = "_AltColor";
                break;
            case SkinType.Uncrossed:
                elementMaterial = settings.uncrossedLineMaterialRuntime;
                colorString1 = "_StartColor";
                colorString2 = "_EndColor";
                break;
            case SkinType.Crossed:
                elementMaterial = settings.crossedLineMaterialRuntime;
                colorString1 = "_StartColor";
                colorString2 = "_EndColor";
                break;
            case SkinType.Background:
                elementMaterial = settings.backgroundMaterialRuntime;
                colorString1 = "_Color1";
                colorString2 = "_Color2";
                break;
        }

        skinElementColors.color1 = elementMaterial.GetColor(colorString1);
        skinElementColors.color2 = elementMaterial.GetColor(colorString2);

        previewSpriteRenderer.material = elementMaterial;

    }

    public void UpdateColorFromTheme()
    {
        Theme current = GameManager.Instance.currentTheme;

        Color updateColor1 = Color.white;
        Color updateColor2 = Color.white;

        switch (type)
        {
            case SkinType.Orb:
                updateColor1 = current.nodeColor1;
                updateColor2 = current.nodeColor2;
                break;
            case SkinType.Uncrossed:
                updateColor1 = current.lineColorStart;
                updateColor2 = current.lineColorEnd;
                break;
            case SkinType.Crossed:
                updateColor1 = current.lineColorStartCrossed;
                updateColor2 = current.lineColorEndCrossed;
                break;
            case SkinType.Background:
                updateColor1 = current.background;
                updateColor2 = current.particleBackground;
                break;
        }

        UpdateColor(updateColor1, 1);
        UpdateColor(updateColor2, 2);
    }

    public void UpdateColor(Color col, int refIndex)
    {
        // update skin element color info
        if (refIndex == 1)
            skinElementColors.color1 = col;
        else if (refIndex == 2)
            skinElementColors.color2 = col;

        // update materials
        UpdateMaterial(col, refIndex);

        // persistantly save color
        SaveToPlayerPrefs(col, refIndex);

        CallSkinChangeDelegate();
    }

    void UpdateMaterial(Color col, int refIndex)
    {
        string targetColor = colorString1;
        if (refIndex == 2)
            targetColor = colorString2;

        if (type == SkinType.Orb)
        {
            // update both runtime materials if this is an orb skin element
            List<Material> orbMaterials = settings.nodeRuntimeMaterials;
            foreach (Material mat in orbMaterials)
                mat.SetColor(targetColor, col);
        } else if (type != SkinType.Background)
        {
            elementMaterial.SetColor(targetColor, col);
        } else
        {
            elementMaterial.SetColor(targetColor, col);
            // update gameobjects accordingly
            GameManager.Instance.gameElements.SetBackground(elementMaterial);
        }
    }

    void SaveToPlayerPrefs(Color col, int refIndex)
    {
        ColorSaveName saveName = 0;

        if (type == SkinType.Orb)
        {
            saveName = ColorSaveName.Orb1;
            if (refIndex == 2)
                saveName = ColorSaveName.Orb2;
        } else if (type == SkinType.Uncrossed)
        {
            saveName = ColorSaveName.Uncrossed1;
            if (refIndex == 2)
                saveName = ColorSaveName.Uncrossed2;
        } else if (type == SkinType.Crossed)
        {
            saveName = ColorSaveName.Crossed1;
            if (refIndex == 2)
                saveName = ColorSaveName.Crossed2;
        } else if (type == SkinType.Background)
        {
            saveName = ColorSaveName.Background;
            if (refIndex == 2)
                saveName = ColorSaveName.BackgroundParticle;
        }

        GameManager.Instance.prefs.SaveColor(saveName, col);
    }

    public void CallSkinChangeDelegate()
    {
        if (type == SkinType.Orb)
            DelegateHandler.onNodeChangeDelegate?.Invoke();
        else if (type == SkinType.Uncrossed || type == SkinType.Crossed)
            DelegateHandler.onLineChangeDelegate?.Invoke();
        else
            DelegateHandler.onBackgroundChangeDelegate?.Invoke();
    }
}

[System.Serializable]
public class SkinElementColors
{
    public Color color1;
    public Color color2;
}

public enum SkinType
{
    Orb,
    Uncrossed,
    Crossed,
    Background
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    PlayerSettings prefs;
    Theme defaultTheme;

    private void Start()
    {
        prefs = GameManager.Instance.prefs;
        defaultTheme = GameManager.Instance.defaultTheme;
    }

    public void LoadColorPrefs()
    {
        if (prefs == null)
            prefs = GameManager.Instance.prefs;
        Theme loadedTheme = ScriptableObject.CreateInstance<Theme>();

        loadedTheme.background = prefs.LoadColor(ColorSaveName.Background);
        loadedTheme.particleBackground = prefs.LoadColor(ColorSaveName.BackgroundParticle);
        loadedTheme.nodeColor1 = prefs.LoadColor(ColorSaveName.Orb1);
        loadedTheme.nodeColor2 = prefs.LoadColor(ColorSaveName.Orb2);
        loadedTheme.lineColorStart = prefs.LoadColor(ColorSaveName.Uncrossed1);
        loadedTheme.lineColorEnd = prefs.LoadColor(ColorSaveName.Uncrossed2);
        loadedTheme.lineColorStartCrossed = prefs.LoadColor(ColorSaveName.Crossed1);
        loadedTheme.lineColorEndCrossed = prefs.LoadColor(ColorSaveName.Crossed2);


        if (defaultTheme == null)
            defaultTheme = GameManager.Instance.defaultTheme;
        loadedTheme.fontColor = defaultTheme.fontColor;

        GameManager.Instance.currentTheme = loadedTheme;
    }

    public string GetValueFromDefault(ColorSaveName saveName)
    {
        if (defaultTheme == null)
            defaultTheme = GameManager.Instance.defaultTheme;
        Color color = Color.white;
        switch (saveName)
        {
            case ColorSaveName.Orb1:
                color = defaultTheme.nodeColor1;
                break;
            case ColorSaveName.Orb2:
                color = defaultTheme.nodeColor2;
                break;
            case ColorSaveName.Uncrossed1:
                color = defaultTheme.lineColorStart;
                break;
            case ColorSaveName.Uncrossed2:
                color = defaultTheme.lineColorEnd;
                break;
            case ColorSaveName.Crossed1:
                color = defaultTheme.lineColorStartCrossed;
                break;
            case ColorSaveName.Crossed2:
                color = defaultTheme.lineColorEndCrossed;
                break;
            case ColorSaveName.Background:
                color = defaultTheme.background;
                break;
            case ColorSaveName.BackgroundParticle:
                color = defaultTheme.particleBackground;
                break;
        }

        return ColorUtility.ToHtmlStringRGBA(color);
    }
}

public enum ColorSaveName
{
    None,
    Background,
    BackgroundParticle,
    Orb1,
    Orb2,
    Uncrossed1,
    Uncrossed2,
    Crossed1,
    Crossed2,
}

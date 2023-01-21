using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkinElementSelection : MonoBehaviour
{
    public static SkinElement selected;
    public static List<SkinElement> elements;
    [SerializeField] ColorPickerHandler handler;

    public void SelectElement(SkinElement ele)
    {
        if (ele == null)
        {
            handler.ShowPickers(false);
            selected = null;
        }

        foreach (SkinElement element in elements)
        {
            if (element != ele)
                element.Deselect();
            else
            {
                element.Select();
                selected = element;
                handler.ShowPickers(true, selected);
            }
        }
    }

    private void OnEnable()
    {
        if (elements == null)
            elements = new List<SkinElement> (GetComponentsInChildren<SkinElement>());

        if (selected == null)
            handler.ShowPickers(false);
        else
            handler.ShowPickers(true, selected);
    }

    public void RevertToDefaultSkin()
    {
        // deselect all
        SelectElement(null);

        GameElementSettings settings = GameManager.Instance.gameElements;

        Theme tCurrent = GameManager.Instance.currentTheme;
        Theme tDefault = GameManager.Instance.defaultTheme;

        tCurrent.background = tDefault.background;
        tCurrent.particleBackground = tDefault.particleBackground;
        tCurrent.nodeColor1 = tDefault.nodeColor1;
        tCurrent.nodeColor2 = tDefault.nodeColor2;
        tCurrent.lineColorStart = tDefault.lineColorStart;
        tCurrent.lineColorEnd = tDefault.lineColorEnd;
        tCurrent.lineColorStartCrossed = tDefault.lineColorStartCrossed;
        tCurrent.lineColorEndCrossed = tDefault.lineColorEndCrossed;

        foreach (SkinElement ele in elements)
            ele.UpdateColorFromTheme();   
    }


    
}

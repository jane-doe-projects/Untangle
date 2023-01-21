using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerHandler : MonoBehaviour
{
    [SerializeField] ReferenceColorPicker ref1;
    [SerializeField] ReferenceColorPicker ref2;

    [SerializeField] GameObject pickerParent;
    [SerializeField] GameObject infoMessageParent;

    public void ShowPickers(bool show, SkinElement element = null)
    {
        pickerParent.SetActive(show);
        infoMessageParent.SetActive(!show);

        if (show && element != null)
            InitPickers(element);
    }

    void InitPickers(SkinElement ele)
    {
        // set current color of materials in the respective reference color preview
        ref1.SetColor(ele.skinElementColors.color1);
        ref1.SetLabel(ele.labelColor1);

        ref2.SetColor(ele.skinElementColors.color2);
        ref2.SetLabel(ele.labelColor2);
    }

}

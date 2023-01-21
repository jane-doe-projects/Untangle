using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReferenceColorPicker : MonoBehaviour
{
    [SerializeField] int index = 0;
    [SerializeField] ColorPicker picker;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image pickedColorPreview;

    public void SetColor(Color col)
    {
        pickedColorPreview.color = col;
        if (index == 0)
            return;
        SkinElementSelection.selected.UpdateColor(col, index);
    }

    public void SetLabel(string label)
    {
        description.text = label;
    }
}

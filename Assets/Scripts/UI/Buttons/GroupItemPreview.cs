using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupItemPreview : BasePreviewItem
{
    public LevelSet set;
    public DisplayWindow targetDisplay;
    public bool isEditable = false;

    public override void OnClickAction()
    {
        DisplaySet();
        targetDisplay.SetSelectedIndex(gameObject);
    }

    void DisplaySet()
    {
        // instantiate sets in target preview
        targetDisplay.AddLevelItems(set, this, isEditable: isEditable);
    }



}

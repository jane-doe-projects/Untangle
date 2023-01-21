using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    [SerializeField] bool disabledNavigation;
    List<Button> allButtons;

    // Start is called before the first frame update
    void Start()
    {
        allButtons = new List<Button>(Resources.FindObjectsOfTypeAll<Button>());
        Navigation newMode = new Navigation();

        if (disabledNavigation)
            newMode.mode = Navigation.Mode.None;
        else
            newMode.mode = Navigation.Mode.Automatic;

        foreach (Button btn in allButtons)
        {
            btn.navigation = newMode;
        }
    }

}

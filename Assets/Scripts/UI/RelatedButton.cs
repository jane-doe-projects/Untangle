using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelatedButton : MonoBehaviour
{
    public ButtonEffects buttonEffects;

    private void OnEnable()
    {
        if (buttonEffects != null)
            buttonEffects.ShowAsActive();
    }

    private void OnDisable()
    {
        if (buttonEffects != null)
            buttonEffects.DisableActive();
    }

}

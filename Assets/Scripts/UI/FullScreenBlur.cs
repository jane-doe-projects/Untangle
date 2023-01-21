using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenBlur : MonoBehaviour
{
    Image img;
    RectTransform rt;

    private void Start()
    {
        AddFullBackgroundBlur();
    }


    void AddFullBackgroundBlur()
    {
        if (img == null)
            img = gameObject.AddComponent<Image>();

        // set recttransform pivot and anchor to stretch over screen (over whole parent)
        rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);

        img.material = GameManager.Instance.gameElements.blurMaterial;
    }
}

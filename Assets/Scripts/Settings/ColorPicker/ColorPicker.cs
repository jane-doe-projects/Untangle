using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorPicker : MonoBehaviour
{
    public Color pickedColor;
    public UnityColorEvent onColorPicked;

    [SerializeField] Image colorPalette;
    int size;

    private void Start()
    {
        size = colorPalette.sprite.texture.width;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && MouseInPicker())
        if (Input.GetMouseButton(0) && MouseInPicker())
        {
            pickedColor = GetColor();
            onColorPicked.Invoke(pickedColor);
        }
            
    }

    Color GetColor()
    {
        Color picked = Color.white;

        Vector2 localPosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPalette.rectTransform, Input.mousePosition, Camera.main, out localPosition);

        Rect.PointToNormalized(colorPalette.rectTransform.rect, localPosition);

        localPosition /= colorPalette.rectTransform.sizeDelta.x;
        localPosition *= size;

        picked = colorPalette.sprite.texture.GetPixel((int)localPosition.x, (int)localPosition.y);

        return picked;
    }

    bool MouseInPicker()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(colorPalette.rectTransform, Input.mousePosition, Camera.main))
            return true;

        return false;
    }
}

[System.Serializable]
public class UnityColorEvent : UnityEvent<Color> { }

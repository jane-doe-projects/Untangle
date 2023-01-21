using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingCoordinate : MonoBehaviour
{
    public delegate void OnAddCoordinate(Vector2 coordinate);
    public static event OnAddCoordinate onAddCoordinateDelegate;

    SpriteRenderer sRenderer;
    Color originalColor;

    bool added = false;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        originalColor = sRenderer.color;
    }

    public void Record()
    {
        added = true;
        Vector2 coord = gameObject.transform.position;

        sRenderer.color = GameManager.Instance.levelMaker.GetSetColor();
        // Debug.Log("Added " + coord);
        onAddCoordinateDelegate(coord);

    }

    private void OnMouseDown()
    {
        if (!added)
            Record();
    }

    public void Reset()
    {
        if (added == true)
        {
            added = false;
            sRenderer.color = originalColor;
        }
    }

}

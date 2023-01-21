using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LinePreview : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    float width = 0.2f;

    private void Start()
    {
        lr.startWidth = width;
        lr.endWidth = width;

        lr.startColor = GameManager.Instance.levelMaker.GetSetColor();
        lr.endColor = GameManager.Instance.levelMaker.GetSetColor();
    }

    public void AddPosition(Vector2 coord)
    {
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, coord);
    }
}

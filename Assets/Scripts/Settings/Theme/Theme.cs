using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Game Theme/New Theme", order = 1)]
public class Theme : ScriptableObject
{
    public Color nodeColor1;
    public Color nodeColor2;

    public Color background;
    public Color particleBackground;

    public Color fontColor;

    [Header("Line Properties")]
    public Color lineColorStart;
    public Color lineColorEnd;

    public Color lineColorStartCrossed;
    public Color lineColorEndCrossed;
}

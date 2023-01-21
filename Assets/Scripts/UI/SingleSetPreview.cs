using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SingleSetPreview : MonoBehaviour
{
    float coordinateScale = 2.5f;

    LineRenderer lr;
    [SerializeField] Material previewLineMaterial;
    Color whiteColor = Color.white;

    bool easeIn = false;
    bool easeOut = false;
    float speed = 0.8f;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        EaseInOut.easeInDelegate += EaseInLine;
        EaseInOut.easeOutDelegate += EaseOutLine;
    }

    private void Update()
    {
        if (easeIn)
        {
            whiteColor.a += Time.deltaTime * speed;
            previewLineMaterial.color = whiteColor;
            if (whiteColor.a == 1)
                easeIn = false;
        }
        else if (easeOut)
        {
            whiteColor.a -= Time.deltaTime * speed;
            if (whiteColor.a == 0)
                easeOut = false;
        }
    }

    public void SetLinesAndNodes(SingleSetCoordinates set)
    {
        // initialize line renderer
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        lr.useWorldSpace = false;
        lr.material = GameManager.Instance.gameElements.linePreviewMaterial;
        lr.textureMode = LineTextureMode.Tile;
        lr.alignment = LineAlignment.TransformZ;
        lr.widthMultiplier = 1.2f;
        lr.numCornerVertices = 2;
        int positionCount = 0;
        lr.positionCount = set.coordsForSet.Count;
        lr.loop = true;
        lr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        foreach (Vector2 vec in set.coordsForSet)
        {
            lr.SetPosition(positionCount, vec * coordinateScale);
            positionCount++;
            // set node at each point TODO

            GameObject ob = Instantiate(GameManager.Instance.gameElements.nodePreviewPrefab, this.transform, false);
            ob.transform.localPosition = vec * coordinateScale;

        }

    }

    void EaseInLine()
    {
        easeIn = true;
        easeOut = false;
    }

    void EaseOutLine()
    {
        easeIn = false;
        easeOut = true;
    }

    private void OnEnable()
    {
        easeIn = true;
        whiteColor.a = 0;
        previewLineMaterial.color = whiteColor;
    }

    private void OnDisable()
    {
        whiteColor.a = 1;
        previewLineMaterial.color = whiteColor;
    }
}

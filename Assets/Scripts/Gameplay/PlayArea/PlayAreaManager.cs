using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaManager : MonoBehaviour
{
    [SerializeField] GameObject playAreaPanel;
    RectTransform rt;

    NodeGrid nodeGrid;

    float xBound, yBound;
    Vector2 currentCoords;


    // Start is called before the first frame update
    void Start()
    {
        nodeGrid = GetComponent<NodeGrid>();

        // get bounds
        DetermineBounds();
        nodeGrid.CreateCoordinateList(xBound, yBound);
    }


    void DetermineBounds()
    {
        rt = playAreaPanel.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        xBound = Mathf.Abs(corners[0].x);
        yBound = Mathf.Abs(corners[0].y);

        //PrintCoordinates(corners);
    }

    void PrintCoordinates(Vector3[] coords)
    {
        foreach (Vector3 vec in coords)
        {
            Debug.Log(vec);
        }
    }

    public void ResetGrid()
    {
        nodeGrid.CreateCoordinateList(xBound, yBound);
    }

    public Vector2 GetAvailableCoordinates()
    {
        currentCoords = nodeGrid.GetCoordinate();

        return currentCoords;
    }

    public Vector2 GetPlayAreaBounds()
    {
        return new Vector2(xBound, yBound);
    }

}

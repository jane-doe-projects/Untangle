using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    List<Vector2> gridCoordinates;

    [SerializeField] int minGridResolution = 2;
    [SerializeField] GameObject testPrefab;
    [SerializeField] bool showGridSpots;
    [SerializeField] bool recordCoordinates;

    private void Start()
    {
        if (recordCoordinates)
            GameManager.Instance.levelMaker.enabled = true;
    }

    public void CreateCoordinateList(float xBound, float yBound)
    {
        gridCoordinates = new List<Vector2>();
        int xB = Mathf.FloorToInt(xBound);
        int yB = Mathf.FloorToInt(yBound);

        for (int i = -xB; i <= xB; i = i + minGridResolution)
        {
            for (int j = -yB; j <= yB; j = j + minGridResolution)
            {
                Vector2 newVec = new Vector2(i, j);

                if (showGridSpots)
                    ShowSpots(newVec);

                gridCoordinates.Add(newVec);
            }
        }
    }

    public Vector2 GetCoordinate()
    {
        int rndIndx = Random.Range(0, gridCoordinates.Count-1);
        //int rndIndx = Random.Range(0, gridCoordinates.Count);
        //Debug.Log(rndIndx);
        //Debug.Log(gridCoordinates.Count);
        Vector2 randomCoord = gridCoordinates[rndIndx];
        gridCoordinates.RemoveAt(rndIndx);
 
        return randomCoord;
    }

    void ShowSpots(Vector2 newVec)
    {
        // instantiate a dot at every posision for testing purposes
        GameObject curr = Instantiate(testPrefab, GameManager.Instance.levelMaker.coordParent.transform);
        curr.transform.position = newVec;
    }

}

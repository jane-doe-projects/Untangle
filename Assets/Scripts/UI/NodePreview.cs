using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePreview : MonoBehaviour
{
    [SerializeField] LevelItemPreview preview;
    [SerializeField] Transform container;

    Level level;
    List<GameObject> lineSets;

    public void CreateNodePreview()
    {
        level = preview.correspondingLevel;
        lineSets = new List<GameObject>();

        // create lines for each set
        foreach (SingleSetCoordinates set in level.nodeSetsCoordinates)
        {
            GameObject newSet = CreateLineSet(set);
            lineSets.Add(newSet);
        }
    }
    
    GameObject CreateLineSet(SingleSetCoordinates set)
    {
        GameObject newPreviewSet = Instantiate(GameManager.Instance.gameElements.singleSetPreviewPrefab, container);
        newPreviewSet.GetComponent<SingleSetPreview>().SetLinesAndNodes(set);

        return newPreviewSet;
    }

}

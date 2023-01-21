using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSet : MonoBehaviour
{
    public List<GameObject> nodes;

    public List<GameObject> GenerateSet(int nodeCount)
    {
        nodes = new List<GameObject>();
        Vector2 bounds = GameManager.Instance.playAreaManager.GetPlayAreaBounds();

        for (int i = 0; i < nodeCount; i++)
        {
            // spawn node and add to list
            Vector2 spawnPosition = GameManager.Instance.playAreaManager.GetAvailableCoordinates();

            GameObject newNode = Instantiate(GameManager.Instance.gameElements.nodePrefab, spawnPosition, Quaternion.identity, this.transform);
            newNode.GetComponent<Node>().nodeset = this;

            nodes.Add(newNode);

        }

        SetPredSuccessors();
        return nodes;
    }

    public List<GameObject> GenerateSet(SingleSetCoordinates set)
    {
        nodes = new List<GameObject>();

        foreach (Vector2 coord in set.coordsForSet)
        {
            Vector2 spawnPosition = coord;
            GameObject newNode = Instantiate(GameManager.Instance.gameElements.nodePrefab, spawnPosition, Quaternion.identity, this.transform);
            newNode.GetComponent<Node>().nodeset = this;

            nodes.Add(newNode);
        }

        SetPredSuccessors();
        return nodes;
    }

    void SetPredSuccessors()
    {
        List<Node> nodeComponents = GetNodeComponents(nodes);

        for (int i = 0; i < nodes.Count; i++)
        {
            // set pred
            if (i == 0)
                nodeComponents[i].predecessor = nodeComponents[nodes.Count - 1];
            else
                nodeComponents[i].predecessor = nodeComponents[i - 1];

            // set succ
            if (i == nodes.Count - 1)
                nodeComponents[i].successor = nodeComponents[0];
            else
                nodeComponents[i].successor = nodeComponents[i + 1];

        }
       
    }

    public List<Node> GetNodeComponents(List<GameObject> list)
    {
        List<Node> retrievedComponents = new List<Node>();

        foreach (GameObject obj in list)
        {
            retrievedComponents.Add(obj.GetComponent<Node>());
        }

        return retrievedComponents;
    }

    public List<ColliderLine> GetAllLinesFromSet()
    {
        List<Node> nodeComponents = GetNodeComponents(nodes);
        List<ColliderLine> retrievedComponents = new List<ColliderLine>();


        foreach (Node obj in nodeComponents)
        {
            retrievedComponents.Add(obj.GetLine());
        }

        return retrievedComponents;
    }

}

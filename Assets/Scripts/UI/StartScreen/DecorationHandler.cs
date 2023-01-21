using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecorationHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> decorationSets;

    void Start()
    {
        foreach (GameObject obj in decorationSets)
            obj.SetActive(false);
        int randomIndex = Random.Range(0, decorationSets.Count);
        decorationSets[randomIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInteraction();
        }

    }

    private void OnDisable()
    {
        // clear selected decoration orb
        if (SelectedDecorationOrb.selected != null)
            SelectedDecorationOrb.selected.Deselect();
        SelectedDecorationOrb.selected = null;
    }

    void HandleInteraction()
    {
        if (SelectedDecorationOrb.hovering == null)
        {
            if (SelectedDecorationOrb.selected != null)
                SelectedDecorationOrb.selected.Deselect();
        }
    }
}


public class SelectedDecorationOrb
{
    public static TitelOrb selected;
    public static TitelOrb hovering;
}
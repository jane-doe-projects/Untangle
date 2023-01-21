using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContent : MonoBehaviour
{
    [SerializeField] List<Section> sections;

    void Start()
    {
        foreach (Section sec in sections)
        {
            Instantiate(sec, this.transform);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOnStart : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.windowControl.AddWindow(gameObject);
    }
}

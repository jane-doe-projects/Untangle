using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivation : MonoBehaviour
{
    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}

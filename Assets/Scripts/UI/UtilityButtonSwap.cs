using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityButtonSwap : MonoBehaviour
{
    [SerializeField] GameObject activationTarget;

    public void ActivateDeletion()
    {
        DeletionHandler.ActivateDeletionMode();
        gameObject.SetActive(false);
    }

    public void DeactivateDeletion()
    {
        DeletionHandler.DeactivateDeletionMode();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        activationTarget.SetActive(true);
    }
}

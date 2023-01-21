using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionHandler : MonoBehaviour
{
    public delegate void OnDeleteActive();
    public static OnDeleteActive deleteActivationDelegate;

    public delegate void OnDeleteDeactivation();
    public static OnDeleteDeactivation deleteDeactivationDelegate;

    public static void ActivateDeletionMode()
    {
        if (deleteActivationDelegate != null)
            deleteActivationDelegate();
    }

    public static void DeactivateDeletionMode()
    {
        if(deleteDeactivationDelegate != null)
            deleteDeactivationDelegate();
    }
}

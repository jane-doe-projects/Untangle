using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] Camera effectsCam;
    float defaultSize = 11;

    // Start is called before the first frame update
    void Start()
    {
        ResolutionHandler.resolutionChangeDelegate += AdaptCameras;
    }

    void AdaptCameras()
    {
        // start all of this as a coroutine after a very short waiting time since Screen.width and height dont return the correct value right after changing the resolutions
        // need to report this as a bug to unity with a minimal example project
        StartCoroutine(DelayedAdapt());
    }

    IEnumerator DelayedAdapt()
    {
        yield return new WaitForSeconds(0.0001f);

        float ratioValue = (float)Screen.height / (float)Screen.width;
        float newOrthoSize = defaultSize;
        if (ratioValue >= 0.78)
            newOrthoSize = 16;
        else if (ratioValue >= 0.75)
            newOrthoSize = 15;
        else if (ratioValue >= 0.6)
            newOrthoSize = 13;
        else if (ratioValue > 0.5625)
            newOrthoSize = 12;
        // else (ratioValue <= 0.5625)

        mainCam.orthographicSize = newOrthoSize;
        effectsCam.orthographicSize = newOrthoSize;
    }
}

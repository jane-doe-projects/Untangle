using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] RectTransform targetRectTransform;
    [SerializeField] RectTransform referenceRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        AdaptScale();
        ResolutionHandler.resolutionChangeDelegate += AdaptScale;
    }

    void AdaptScale()
    {
        //Debug.Log("Adapting scale");

        //Debug.Log(targetRectTransform.localScale.y);
        Vector3 tempScale = targetRectTransform.localScale;
        tempScale.y = referenceRectTransform.rect.height;
        targetRectTransform.localScale = tempScale;
        //Debug.Log(targetRectTransform.localScale.y);

        //Debug.Log(referenceRectTransform.rect.height);
    }

}

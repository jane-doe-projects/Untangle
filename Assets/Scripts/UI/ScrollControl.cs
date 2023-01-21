using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;

    public void ActivateScroll()
    {
        scrollRect.enabled = true;
    }
}

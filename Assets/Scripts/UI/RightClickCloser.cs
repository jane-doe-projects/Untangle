using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickCloser : MonoBehaviour
{
    [SerializeField] string closingTag = "Close";
    WindowControl windowControl;

    void Start()
    {
        windowControl = GetComponent<WindowControl>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (PointerOverTag(closingTag))
                windowControl.CloseAllWithCheck();   
        }
    }

    bool PointerOverTag(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            //Debug.Log(raycastResults[0].gameObject.tag);
            if (raycastResults[0].gameObject.tag == tag)
                return true;
        }
        return false;
    }
}

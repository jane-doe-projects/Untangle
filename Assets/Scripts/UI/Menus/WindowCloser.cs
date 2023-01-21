using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowCloser : MonoBehaviour
{
    [SerializeField] bool showStartAfterClose = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (!PointerOverWindow())
                HideWindow();
        }
    }

    bool PointerOverWindow()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
            if (raycastResults[0].gameObject.tag == gameObject.tag)
                return true;
        return false;
    }


    void HideWindow()
    {
        if (showStartAfterClose)
        {
            GameManager.Instance.currentSession.ResetSessionState();
            GameManager.Instance.windowControl.CloseAll();
            GameManager.Instance.windowControl.ShowStart();
            GameManager.Instance.sessionInfo.HideValues();
        }
        gameObject.SetActive(false);
    }
}

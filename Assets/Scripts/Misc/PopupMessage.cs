using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupMessage : MonoBehaviour
{
    public TextMeshProUGUI popupMessage;
    public float fadeSpeed = 0.5f;
    [SerializeField] Transform topPosition;
    [SerializeField] Transform bottomPosition;
    CanvasGroup cGroup;

    bool fadeOut = false;

    private void Start()
    {
        cGroup = GetComponent<CanvasGroup>();
        cGroup.alpha = 0;
    }

    void Update()
    {
        if (fadeOut && cGroup.alpha > 0)
        {
            cGroup.alpha -= Time.deltaTime * fadeSpeed;
            if (cGroup.alpha == 0)
            {
                fadeOut = false;
                popupMessage.gameObject.SetActive(false);
            }
        }
    }

    public void DisplayMessage(string messageText)
    {
        popupMessage.gameObject.transform.position = Vector3.zero;
        GameManager.Instance.soundControl.uiSounds.PlayPopup();
        popupMessage.text = messageText;
        popupMessage.gameObject.SetActive(true);
        cGroup.alpha = 1;
        fadeOut = true;
    }

    public void DisplayMessage(string messageText, Vector3 position)
    {
        DisplayMessage(messageText);
        popupMessage.transform.position = position;
    }

    public void DisplayMessage(string messageText, DisplayPosition pos = DisplayPosition.Default)
    {
        DisplayMessage(messageText);

        switch (pos)
        {
            case DisplayPosition.Top:
                popupMessage.transform.position = topPosition.position;
                break;
            case DisplayPosition.Bottom:
                popupMessage.transform.position = bottomPosition.position;
                break;
            default:
                popupMessage.transform.position = Vector3.zero;
                break;
        }

    }


}

public enum DisplayPosition
{
    Default,
    Top,
    Bottom
}

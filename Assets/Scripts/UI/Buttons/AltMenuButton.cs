using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltMenuButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] TextMeshProUGUI btnText;
    [SerializeField] string text;

    Image img;

    // Start is called before the first frame update
    void Start()
    {
        DelegateHandler.onBackgroundChangeDelegate += SetAspectColor;

        btnText.text = text.ToString();

        img = GetComponent<Image>();
    }

    public void LeaveGame()
    {
        GameManager.Instance.ExitGame();
    }

    public void CancelLeaveGame()
    {
        GameManager.Instance.windowControl.CloseAll();
        if (!GameManager.Instance.currentSession.initialized)
            GameManager.Instance.windowControl.ShowStart();
    }

    public void CampaignSelect()
    {
        // show campaign select
        GameManager.Instance.windowControl.ShowCampaignSelect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    void SetAspectColor()
    {
        img.color = GameManager.Instance.currentTheme.nodeColor1;
    }

    private void OnEnable()
    {
        img = GetComponent<Image>();
        SetAspectColor();
    }

}

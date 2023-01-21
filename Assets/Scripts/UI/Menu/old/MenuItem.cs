using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] string description;
    [SerializeField] Sprite menuIcon;
    [SerializeField] GameObject itemContent;

    MenuItemType type;

    private void Start()
    {
        GetComponent<Image>().sprite = menuIcon;
    }

    public void ExitGame()
    {
        // save current state of session

        // exit application
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CampaignSelect()
    {
        // show campaign select
        GameManager.Instance.windowControl.ShowCampaignSelect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GameManager.Instance.soundControl.uiSounds.PlayButtonHover();
    }

}

public enum MenuItemType
{
    None,
    Resume,
    Settings,
    Mode,
    Exit,
    Pause
}
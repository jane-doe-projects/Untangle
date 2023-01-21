using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class WindowControl : MonoBehaviour
{
    [SerializeField] GameObject modes;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject info;
    [SerializeField] GameObject exit;

    [SerializeField] GameObject statistics;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject favoritesPanel;
    [SerializeField] GameObject favoritesComplete;
    [SerializeField] GameObject campaignSelect;
    [SerializeField] GameObject campaignComplete;

    [SerializeField] GameObject screenCheckWindow;

    public List<GameObject> openWindows;

    public delegate void OnStatisticsOpen();
    public static event OnStatisticsOpen onStatisticsOpen;



    private void Start()
    {
        if (openWindows == null)
            openWindows = new List<GameObject>();

        Session.onLevelStartDelegate += CloseAll;
    }

    private void Update()
    {
        if (screenCheckWindow.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && openWindows.Count == 0)
        {
            ShowExit();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !startScreen.activeSelf)
        {
            CloseAllWithCheck();
        }
    }

    public void AddWindow(GameObject obj)
    {
        openWindows.Add(obj);
    }

    public void ShowCampaignSelect()
    {
        ShowWindow(campaignSelect);
    }

    public void ShowCampaignComplete()
    {
        ShowWindow(campaignComplete);
        GameManager.Instance.soundControl.PlayCompleted();
    }

    public void ShowWindow(GameObject obj)
    {
        if (obj.activeSelf)
        {
            CloseAll();
            if (!GameManager.Instance.currentSession.initialized)
            {
                startScreen.SetActive(true);
                openWindows.Add(startScreen);
            }
            return;
        }
        CloseAll();
        GameManager.Instance.currentSession.DeactiveInteractionForNodes();
        obj.SetActive(true);
        openWindows.Add(obj);        
    }

    public void CloseAll()
    {
        foreach (GameObject obj in openWindows)
            obj.SetActive(false);
        openWindows.Clear();

        ClearButtonSelection();

        if (GameManager.Instance.currentSession.initialized)
            GameManager.Instance.currentSession.ActiveInteractionForNodes();
    }

    void ClearButtonSelection()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            if (!EventSystem.current.currentSelectedGameObject.activeInHierarchy) // if last selected button is disabled - clear it
                EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ShowModes()
    {
        ShowWindow(modes);
    }

    public void ShowSettings()
    {
        ShowWindow(settings);
    }

    public void ShowExit()
    {
        ShowWindow(exit);
    }

    public void ShowInfo()
    {
        ShowWindow(info);
    }

    public void ShowStart()
    {
        ShowWindow(startScreen);
    }

    public void ShowFavorites()
    {
        ShowWindow(favoritesPanel);
    }

    public void ShowFavoritesComplete()
    {
        ShowWindow(favoritesComplete);
        GameManager.Instance.soundControl.PlayCompleted();
    }

    public void ShowStatistics()
    {
        onStatisticsOpen();
        ShowWindow(statistics);
    }

    public void CloseAllWithCheck()
    {
        bool favs = favoritesPanel.activeSelf;
        bool camp = campaignSelect.activeSelf;

        CloseAll();
        if (favs || camp)
            ShowWindow(modes);
        else if (!GameManager.Instance.currentSession.initialized)
            ShowWindow(startScreen);
    }

    public bool IsModesWindowOpen()
    {
        return modes.activeSelf;
    }

    public bool AreSettingsOpen()
    {
        return settings.activeSelf;
    }
}

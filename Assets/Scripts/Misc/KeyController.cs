using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyController : MonoBehaviour
{
    Session session;
    SolvedManager solvedManager;
    WindowControl windowControl;

    [SerializeField] Button navigationStartButton;

    private void Start()
    {
        session = GameManager.Instance.currentSession;
        solvedManager = GameManager.Instance.solvedManager;
        windowControl = GameManager.Instance.windowControl;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (!selected)
            {
                EventSystem.current.SetSelectedGameObject(navigationStartButton.gameObject);
            } else if (selected.tag == "LastSavedButton")
            {
                EventSystem.current.SetSelectedGameObject(navigationStartButton.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2) && session.currentType == SessionType.Random)
            session.NextLevel();

        else if (Input.GetKeyDown(KeyCode.P) && session.initialized)
        {
            if (solvedManager.IsPaused())
                windowControl.CloseAll();
            solvedManager.PauseGame();
        } else if (Input.GetKeyDown(KeyCode.M))
        {
            bool menuOpen = windowControl.IsModesWindowOpen();
            windowControl.CloseAll();
            if (!menuOpen)
                windowControl.ShowModes();
            else if (!session.initialized)
                windowControl.ShowStart();
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            bool settingsOpen = windowControl.AreSettingsOpen();
            windowControl.CloseAll();
            if (!settingsOpen)
                windowControl.ShowSettings();
            else if (!session.initialized)
                windowControl.ShowStart();
        }

    }
}

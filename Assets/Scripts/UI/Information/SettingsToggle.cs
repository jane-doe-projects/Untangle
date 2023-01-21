using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] SettingsToggleType type;
    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] string description;

    private void Start()
    {
        AddListener();
        label.text = description;
    }

    void ToggleSessionInfo()
    {
        if (toggle.isOn)
        {
            GameManager.Instance.prefs.SaveInfoDisplay(true);
            GameManager.Instance.sessionInfo.show = true;
            bool hideProgress = true;
            if (GameManager.Instance.currentSession.currentType != SessionType.Random)
                hideProgress = false;
            GameManager.Instance.sessionInfo.ShowValues(hideProgress);
        }
        else
        {
            GameManager.Instance.prefs.SaveInfoDisplay(false);
            GameManager.Instance.sessionInfo.show = false;
            GameManager.Instance.sessionInfo.HideValues();
        }
    }

    void ToggleLevelLoad()
    {
        if (toggle.isOn)
            GameManager.Instance.solvedManager.AutomaticNext(true);
        else
            GameManager.Instance.solvedManager.AutomaticNext(false);
        GameManager.Instance.prefs.SaveAutomaticNext(toggle.isOn);
    }

    void ToggleSaveLevel()
    {
        GameManager.Instance.prefs.SaveSaveOnExitState(toggle.isOn);
    }

    public void SetState(bool isOn)
    {
        toggle.isOn = isOn;
    }

    void AddListener()
    {
        toggle.onValueChanged.AddListener(delegate { PlaySound(); });

        switch (type)
        {
            case SettingsToggleType.SessionInfo:
                toggle.onValueChanged.AddListener(delegate { ToggleSessionInfo(); } );
                break;
            case SettingsToggleType.LevelLoad:
                toggle.onValueChanged.AddListener(delegate { ToggleLevelLoad(); } );
                break;
            case SettingsToggleType.SaveLevel:
                toggle.onValueChanged.AddListener(delegate { ToggleSaveLevel(); } );
                break;
        }
    }

    void PlaySound()
    {
        GameManager.Instance.soundControl.uiSounds.Click();
    }

}

public enum SettingsToggleType
{
    SessionInfo,
    LevelLoad,
    SaveLevel
}

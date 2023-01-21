using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class PlayerSettings : MonoBehaviour
{
    bool showInfo = false;
    string displayInfo = "DisplayInfo";
    [SerializeField] SettingsToggle infoToggle;

    bool saveOnExit = false;
    string saveOnExitSetting = "SaveOnExit";

    bool autoNext = false;
    string autoNextSetting = "AutoLoad";
    [SerializeField] SettingsToggle autoNextToggle;

    [SerializeField] Resolution mode;
    [SerializeField] Resolution size;

    public void LoadAll()
    {
        StatisticsCollection.InitializeStatsRecordingDate();
        LoadSkin();
        SetAndLoadInfoDisplay();
        SetAndLoadAutoContinue();
        Vector4 resolutionSettings = LoadResolutionSettings();
        mode.InitializeLastSelected((int) resolutionSettings.z);
        size.InitializeLastSelected((int) resolutionSettings.w);
    }

    public void SaveResolutionMode(ResolutionSetting mode)
    {
        PlayerPrefs.SetInt("ResolutionMode", (int) mode);
        PlayerPrefs.Save();
    }

    public void SaveResolutionSize(int x, int y, int index)
    {
        PlayerPrefs.SetInt("ResolutionWasSet", 1);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.SetInt("ResolutionX", x);
        PlayerPrefs.SetInt("ResolutionY", y);
        PlayerPrefs.Save();
    }

    public Vector4 LoadResolutionSettings()
    {
        Vector4 settings = new Vector4();
        settings.x = PlayerPrefs.GetInt("ResolutionX", -1);
        settings.y = PlayerPrefs.GetInt("ResolutionY", -1);
        settings.z = PlayerPrefs.GetInt("ResolutionMode");
        settings.w = PlayerPrefs.GetInt("ResolutionIndex");

        if (settings.x == -1 || settings.y == -1)
        {
            PlayerPrefs.SetInt("ResolutionX", Screen.width);
            PlayerPrefs.SetInt("ResolutionY", Screen.height);
            PlayerPrefs.Save();
            settings.x = Screen.width;
            settings.y = Screen.height;
        }

        // to prevent setting and loading with the smallest resolution on the first start
        if (PlayerPrefs.GetInt("ResolutionWasSet") == 0)
            settings.w = -1;

        return settings;
    }

    void LoadSkin()
    {
        GameManager.Instance.colorControl.LoadColorPrefs();
        // GameManager.Instance.LoadTheme();
    }

    public void SaveInfoDisplay(bool isOn)
    {
        int value = 0;
        if (isOn)
            value = 1;
        PlayerPrefs.SetInt(displayInfo, value);
        PlayerPrefs.Save();

    }

    public void SaveAutomaticNext(bool isOn)
    {
        int value = 0;
        if (isOn)
            value = 1;
        PlayerPrefs.SetInt(autoNextSetting, value);
        PlayerPrefs.Save();
    }

    void SetAndLoadInfoDisplay()
    {
        int value = PlayerPrefs.GetInt(displayInfo);
        if (value == 1)
            showInfo = true;
        else
            showInfo = false;

        infoToggle.SetState(showInfo);
        GameManager.Instance.sessionInfo.show = showInfo;
    }

    void SetAndLoadAutoContinue()
    {
        int value = PlayerPrefs.GetInt(autoNextSetting);
        if (value == 1)
            autoNext = true;
        else
            autoNext = false;

        GameManager.Instance.solvedManager.AutomaticNext(autoNext);
        autoNextToggle.SetState(autoNext);
    }

    public bool LoadSaveOnExitState()
    {
        saveOnExit = Convert.ToBoolean(PlayerPrefs.GetInt(saveOnExitSetting));
        return saveOnExit;
    }

    public void SaveSaveOnExitState(bool toggleState)
    {
        int value = Convert.ToInt32(toggleState);
        PlayerPrefs.SetInt(saveOnExitSetting, value);
        PlayerPrefs.Save();
        saveOnExit = toggleState;
    }

    public Resolution GetResolutionSizeDropDown()
    {
        return size;
    }

    public void SaveColor(ColorSaveName saveName, Color col)
    {

        PlayerPrefs.SetString(saveName.ToString(), ColorUtility.ToHtmlStringRGB(col));
        PlayerPrefs.Save();
    }

    public Color LoadColor(ColorSaveName saveName)
    {
        string loadedColorString = PlayerPrefs.GetString(saveName.ToString(), null);
        if (loadedColorString == null || loadedColorString == "")
            loadedColorString = GameManager.Instance.colorControl.GetValueFromDefault(saveName);
        Color retCol = Color.red;

        // make hex
        string finalColorString = "#" + loadedColorString;

        ColorUtility.TryParseHtmlString(finalColorString, out retCol);
        return retCol;
    }

    public void SaveLanguage(int id)
    {
        List<Locale> list = LocalizationSettings.AvailableLocales.Locales;
        Locale target = list[id];

        PlayerPrefs.SetString("Language", target.Identifier.Code);
        Debug.Log(target.LocaleName);

        //PlayerPrefs.SetInt("Language", id);
        PlayerPrefs.Save();
    }

    public int LoadLanguage()
    {
        int localeIndex = 0;

        string localeCode = PlayerPrefs.GetString("Language", "");

        foreach (Locale loc in LocalizationSettings.AvailableLocales.Locales)
        {
            if (loc.Identifier.Code == localeCode)
                break;
            localeIndex++;
        }

        return localeIndex;
        //return PlayerPrefs.GetInt("Language", 0);
    }

}

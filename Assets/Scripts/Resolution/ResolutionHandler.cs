using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionHandler : MonoBehaviour
{
    public delegate void OnResolutionChange();
    public static OnResolutionChange resolutionChangeDelegate;

    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] TMP_Dropdown sizeDropdown;

    public float screenTimeOut = 5f;
    public GameObject screenChangeCheckDialog;

    public int lastMode;
    public int lastSize;

    public int currentMode;
    public int currentSize;

    bool reverting = false;

    private void Start()
    {
        if (resolutionChangeDelegate != null)
            resolutionChangeDelegate();
    }

    public List<string> GetAvailableResolutions()
    {
        List<string> stringResults = new List<string>();
        var resolutions = Screen.resolutions;
        
        foreach (var res in resolutions)
        {
            /*
            Debug.Log(res.height + " " + res.width);
            float ratioValue = (float)res.height / (float)res.width;
            Debug.Log(ratioValue); */
            /*
            if (ratioValue == 0.5625) // 0.5622255
            {
                // only add sizes that match 16:9 ratio
                string newRes = res.width + " x " + res.height;
                stringResults.Add(newRes);
            } */

            string newRes = res.width + " x " + res.height;
            stringResults.Add(newRes);
        }

        // remove duplicates in case of double resolutions for different frequencies
        ICollection<string> noDups = new HashSet<string>(stringResults);
        stringResults = new List<string>(noDups);

        return stringResults;
    }

    void ScreenChangeCheck()
    {
        screenChangeCheckDialog.SetActive(true);
        reverting = true;
    }

    public void ReturnToLast()
    {
        modeDropdown.value = lastMode;
        sizeDropdown.value = lastSize;

        currentMode = lastMode;
        currentSize = lastSize;

        reverting = false;
        screenChangeCheckDialog.SetActive(false);
    }

    // only call this when the user accepts the changes through the dialog window
    public void SetCurrentAsLast()
    {
        reverting = false;
        //lastMode = currentMode;
        //lastSize = currentSize;

        lastMode = modeDropdown.value;
        lastSize = sizeDropdown.value;
        screenChangeCheckDialog.SetActive(false);
    }

    public void SetResolutionMode(int mode)
    {
        if (!reverting)
            ScreenChangeCheck();

        currentMode = mode;

        ResolutionSetting resSetting = (ResolutionSetting) mode;

        switch (resSetting)
        {
            case ResolutionSetting.Fullscreen:
                SetFullscreenWindow();
                break;
            case ResolutionSetting.Exclusive:
                SetExclusive();
                break;
            case ResolutionSetting.Windowed:
                SetWindowed();
                break;
        }
    }

    public void SetResolutionSize(int sizeIndex, List<TMP_Dropdown.OptionData> options)
    {
        if (!reverting)
            ScreenChangeCheck();

        currentSize = sizeIndex;

        Vector2 res = ParseResolution(options[sizeIndex].text);
        Screen.SetResolution((int) res.x, (int) res.y, Screen.fullScreenMode);

        GameManager.Instance.prefs.SaveResolutionSize((int) res.x, (int) res.y, sizeIndex);

        resolutionChangeDelegate();
        Debug.Log("ResSize: " + res.x + " " + res.y);
        Debug.Log(Screen.fullScreenMode);
    }

    void SetFullscreenWindow()
    {
        Debug.Log("Fullscreen Windowed");

        GameManager.Instance.prefs.SaveResolutionMode(ResolutionSetting.Fullscreen);
        SetModeWithSavedResolution(FullScreenMode.FullScreenWindow);
    }

    void SetExclusive()
    {
        Debug.Log("Exclusive Fullscreen");

        GameManager.Instance.prefs.SaveResolutionMode(ResolutionSetting.Exclusive);
        SetModeWithSavedResolution(FullScreenMode.ExclusiveFullScreen);
    }

    void SetWindowed()
    {
        Debug.Log("Windowed");

        GameManager.Instance.prefs.SaveResolutionMode(ResolutionSetting.Windowed);
        SetModeWithSavedResolution(FullScreenMode.Windowed);
        // need to reload the window or something so the cursor image shows in the right position
    }

    public static Vector2 ParseResolution(string resolutionStr)
    {
        Vector2 resolution = new Vector2(1280, 720);

        string[] splitString = resolutionStr.Split(" ");
        resolution.x = float.Parse(splitString[0]);
        resolution.y = float.Parse(splitString[2]);

        return resolution;
    }

    void SetModeWithSavedResolution(FullScreenMode mode)
    {
        Vector4 resSettings = GameManager.Instance.prefs.LoadResolutionSettings();
        int x = (int)resSettings.x;
        int y = (int)resSettings.y;

        Screen.SetResolution(x, y, mode);
        resolutionChangeDelegate();
    }
}

public enum ResolutionSetting
{
    Fullscreen,
    Exclusive,
    Windowed
}

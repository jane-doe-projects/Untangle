using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LanguageControl : MonoBehaviour
{

    public void PickLanguage(int index)
    {
        StartCoroutine(ChangeLanguage(index));
    }

    public List<Locale> GetAvailableLocales()
    {
        return LocalizationSettings.AvailableLocales.Locales;
    }

    IEnumerator ChangeLanguage(int index)
    {
        yield return LocalizationSettings.InitializationOperation; // make sure localization is loaded
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        GameManager.Instance.prefs.SaveLanguage(index);
        Debug.Log("Changed language to index " + index);
    }
}

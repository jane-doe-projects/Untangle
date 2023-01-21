using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;


public class Language : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropDown;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] string labelText;

    LanguageControl langControl;

    private void Start()
    {
        label.text = labelText;
        langControl = GameManager.Instance.langControl;

        dropDown.onValueChanged.AddListener(delegate { PlaySound(); });
        dropDown.onValueChanged.AddListener(delegate { langControl.PickLanguage(dropDown.value); });

        InitLanguageChoices();
    }

    public void InitLanguageChoices()
    {
        List<Locale> languages = langControl.GetAvailableLocales();
        List<string> langOptions = new List<string>();
        foreach (Locale lang in languages)
        {
            langOptions.Add(lang.ToString());
        }
        dropDown.AddOptions(langOptions);

        // set last selected
        dropDown.value = GameManager.Instance.prefs.LoadLanguage();
    }

    void PlaySound()
    {
        GameManager.Instance.soundControl.uiSounds.Click();
    }
}

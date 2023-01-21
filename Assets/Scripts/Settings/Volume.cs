using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    [SerializeField] string description;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI titel;
    public VolumeGroup group;

    [Header("Skin Elements")]
    [SerializeField] Image fillArea;
    [SerializeField] Image handle;


    float volume;

    void Start()
    {
        DelegateHandler.onBackgroundChangeDelegate += SetSkin;

        titel.text = description;
        // set scroll bar to value;

        float volume = GameManager.Instance.soundControl.LoadSoundSettings(group);
        slider.value = volume;

        //SetSkin();
    }


    public void OnChangeSlider()
    {
        //Mathf.Log10(value) * 20;

        if (group == VolumeGroup.Background)
            GameManager.Instance.soundControl.SetVolumeMusic(slider.value);
        else if (group == VolumeGroup.SFX)
            GameManager.Instance.soundControl.SetVolumeSfx(slider.value);
    }

    void SetSkin()
    {
        Color accent = GameManager.Instance.currentTheme.background;
        Color accent2 = accent;
        accent.a = GameManager.Instance.gameElements.opacityValue;
        fillArea.color = accent;
        handle.color = accent2;
    }

    private void OnEnable()
    {
        SetSkin();
    }
}

public enum VolumeGroup
{
    Background,
    SFX
}

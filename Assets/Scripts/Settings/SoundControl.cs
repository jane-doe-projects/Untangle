using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundControl : MonoBehaviour
{
    public delegate void OnSwap();
    public static OnSwap onSwapDelegate;

    public delegate void OnNotify();
    public static OnNotify onNotifyDelegate;

    [SerializeField] AudioSource background;
    [SerializeField] AudioSource sfxWin;
    [SerializeField] AudioSource sfxSwap;
    [SerializeField] AudioSource sfxNew;
    [SerializeField] AudioSource sfxCompleted;
    [SerializeField] AudioMixer mainMixer;

    [SerializeField] public UISounds uiSounds;

    string sfxPref = "SfxVolume";
    string musicPref = "MusicVolume";

    private void Start()
    {
        onSwapDelegate += PlaySwap;
        onNotifyDelegate += PlayNotify;
    }

    public void SetBackground(AudioClip clip)
    {
        background.clip = clip;
    }

    public void SetWinAndSwap(AudioClip clipWin, AudioClip clipSwap)
    {
        sfxWin.clip = clipWin;
        sfxSwap.clip = clipSwap;
    }

    public void SetThemeAudio(AudioClip clipBackground, AudioClip clipWin, AudioClip clipSwap)
    {
        SetBackground(clipBackground);
        SetWinAndSwap(clipWin, clipSwap);
    }

    public void SetThemeAudio()
    {
        /*
        AudioClip clipBackground = GameManager.Instance.currentTheme.backgroundMusic;
        AudioClip clipWin = GameManager.Instance.currentTheme.sfxSolved;
        AudioClip clipSwap = GameManager.Instance.currentTheme.sfxSwapped;

        SetBackground(clipBackground);
        SetWinAndSwap(clipWin, clipSwap); */
    }

    void PlaySwap()
    {
        sfxSwap.PlayOneShot(sfxSwap.clip);
    }

    public void PlaySolved()
    {
        sfxWin.PlayOneShot(sfxWin.clip);
    }

    public void PlayNew()
    {
        sfxNew.PlayOneShot(sfxNew.clip);
    }

    public void PlayCompleted()
    {
        sfxCompleted.PlayOneShot(sfxCompleted.clip);
    }

    public void PlayNotify()
    {
        //sfxNotify.PlayOneShot(sfxNotify.clip);
    }

    public void PlayBackground()
    {
        if (background.isPlaying == false)
            background.Play();
    }

    public void SetVolumeSfx(float value)
    {
        mainMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(sfxPref, value);
        PlayerPrefs.Save();
    }

    public void SetVolumeMusic(float value)
    {
        mainMixer.SetFloat("backgroundVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(musicPref, value);
        PlayerPrefs.Save();
    }

    public void LoadSoundSettings(ref float music, ref float sfx)
    {
        if (PlayerPrefs.HasKey(musicPref))
            music = PlayerPrefs.GetFloat(musicPref);
        else
            music = 1;

        if (PlayerPrefs.HasKey(sfxPref))
            sfx = PlayerPrefs.GetFloat(sfxPref);
        else
            sfx = 1;
    }

    public float LoadSoundSettings(VolumeGroup group)
    {
        float volume = 1;
        if (group == VolumeGroup.Background)
            volume = PlayerPrefs.GetFloat(musicPref);
        else if (group == VolumeGroup.SFX)
            volume = PlayerPrefs.GetFloat(sfxPref);

        return volume;
    }

    public void LoadSoundPrefs()
    {
        float music = 0;
        float sfx = 0;
        LoadSoundSettings(ref music, ref sfx);

        SetVolumeMusic(music);
        SetVolumeSfx(sfx);
    }
}

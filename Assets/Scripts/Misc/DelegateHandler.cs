using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    // main load delegate
    public delegate void OnSkinLoad();
    public static OnSkinLoad onSkinLoadDelegate;

    // skin delegates
    public delegate void OnBackgroundChange();
    public static OnBackgroundChange onBackgroundChangeDelegate;

    public delegate void OnNodeChange();
    public static OnNodeChange onNodeChangeDelegate;

    public delegate void OnLineChange();
    public static OnLineChange onLineChangeDelegate;

    private void Start()
    {
        onSkinLoadDelegate += LoadSkin;

    }

    void LoadSkin()
    {
        Debug.Log("Load skin getting called.");
        onBackgroundChangeDelegate?.Invoke();
        onNodeChangeDelegate?.Invoke();
        onLineChangeDelegate?.Invoke();
    }

}

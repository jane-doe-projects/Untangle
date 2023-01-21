using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendInCanvasGroup : MonoBehaviour
{
    [SerializeField] CanvasGroup cGroup;
    [SerializeField] bool playSoundOnStart = false;
    [SerializeField] bool blendOut;

    bool startFade = false;
    [SerializeField] float fadeSpeed = 5;

    private void Update()
    {
        if (startFade)
        {
            if (blendOut)
                cGroup.alpha -= Time.deltaTime * fadeSpeed;
            else
                cGroup.alpha += Time.deltaTime * fadeSpeed;
        }
    }

    private void OnEnable()
    {
        startFade = true;

        if (playSoundOnStart)
            SoundControl.onSwapDelegate();
    }

    private void OnDisable()
    {
        startFade = false;
        if (blendOut)
            cGroup.alpha = 1;
        else
            cGroup.alpha = 0;
    }
}

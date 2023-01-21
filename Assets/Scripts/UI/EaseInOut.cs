using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EaseInOut : MonoBehaviour
{
    public delegate void OnEaseIn();
    public static OnEaseIn easeInDelegate;

    public delegate void OnEaseOut();
    public static OnEaseOut easeOutDelegate;

    CanvasGroup cGroup;
    float speed = 0.9f;
    bool easeIn = false;
    bool easeOut = false;

    private void Start()
    {
        cGroup = gameObject.GetComponent<CanvasGroup>();
        cGroup.alpha = 0;
    }

    private void Update()
    {
        if (easeIn && cGroup.alpha < 1)
        {
            cGroup.alpha += Time.deltaTime * speed;
            if (cGroup.alpha == 1)
                easeIn = false;
        } else if (easeOut && cGroup.alpha > 0)
        {
            cGroup.alpha -= Time.deltaTime * speed;
            if (cGroup.alpha == 0)
                gameObject.SetActive(false); // should disable the gameobject
        }
    }

    private void OnEnable()
    {
        // easy in
        easeIn = true;
        //GameManager.Instance.soundControl.uiSounds.PlayMenu();
        if (easeInDelegate != null)
            easeInDelegate();
    }

    private void OnDisable()
    {

        // this is tricky since it will disable the object and it wont do anything.... 

        // set alpha to 0 right away
        easeIn = false;
        cGroup.alpha = 0;
        if (easeOutDelegate != null)
            easeOutDelegate();
    }

    public void DeactivateGOWithFade()
    {
        //ease out
        StartCoroutine("EaseOutAndDisable");
    }

    IEnumerator EaseOutAndDisable()
    {
        easeIn = false;
        easeOut = true;
        yield return null;
    }

}

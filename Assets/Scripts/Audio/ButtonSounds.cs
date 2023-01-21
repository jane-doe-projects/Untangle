using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayOnClick);
    }

    void PlayOnClick()
    {
        GameManager.Instance.soundControl.uiSounds.Click();
    }

    void PlayOnHover()
    {
        GameManager.Instance.soundControl.uiSounds.Hover();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayOnHover();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverTarget;
    [SerializeField] HoverPopout hoverPopout;

    [SerializeField] Sprite icon;
    [SerializeField] string infoText;

    [SerializeField] Animator anim;

    private void Start()
    {
        hoverPopout.SetLabel(infoText, icon);
    }

    public void Deactivate()
    {
        hoverTarget.gameObject.SetActive(false);
        hoverPopout.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverPopout.Show();
        GameManager.Instance.soundControl.uiSounds.Hover();
        anim.SetBool("hoverOn", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverPopout.Hide();
        anim.SetBool("hoverOn", false);
    }
}

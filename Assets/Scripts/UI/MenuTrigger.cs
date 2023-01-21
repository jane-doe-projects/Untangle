using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTrigger : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] MenuTriggerType type;
    [SerializeField] MenuTrigger counterPart;
    [SerializeField] MenuAnimation menuAnimation;

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool noSuccess = false;
        //Debug.Log("Entered " + gameObject.name);
        if (type == MenuTriggerType.Open)
        {
            // open menu
            noSuccess =  menuAnimation.OpenMenu();
        } else
        {
            // close menu
            noSuccess = menuAnimation.CloseMenu();
        }

        // disable object
        if (!noSuccess)
            return;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // activate counterpart
        counterPart.gameObject.SetActive(true);
    }
}

public enum MenuTriggerType
{
    Open,
    Close
}

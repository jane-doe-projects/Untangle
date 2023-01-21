using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionNotifyer : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public delegate void OnMainMenuButtonSelect(bool isSave);
    public static event OnMainMenuButtonSelect onMainMenuButtonSelectDelegate;

    public delegate void OnMainMenuButtonDeselect(bool isSave);
    public static event OnMainMenuButtonDeselect onMainMenuButtonDeselectDelegate;

    [SerializeField] bool isSaveButton;

    public void OnSelect(BaseEventData eventData)
    {
        onMainMenuButtonSelectDelegate(isSaveButton);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onMainMenuButtonDeselectDelegate(isSaveButton);
    }

}

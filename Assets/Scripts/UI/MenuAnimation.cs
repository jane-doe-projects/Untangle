using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAnimation : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator anim;
    Button btn;
    [SerializeField] bool isHidden;

    bool aButtonIsSelected;
    Coroutine counterRoutine;

    [SerializeField] MenuTrigger closeTrigger;
    [SerializeField] MenuButton lastSaveButton;
    bool saveButtonSelected;

    public delegate void OnMenuHasOpened();
    public static event OnMenuHasOpened menuHasOpenedDelegate;

    // Start is called before the first frame update
    void Start()
    {
        Session.onLevelStartDelegate += CloseIfOpen;
        ButtonSelectionNotifyer.onMainMenuButtonSelectDelegate += ButtonSelected;
        ButtonSelectionNotifyer.onMainMenuButtonDeselectDelegate += ButtonDeselected;

        // Disable button functionality - this was used for when we needed to open the menu manually. Now it opens automatically when coming close to the bottom of the screen
        /*
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleMenu); */
        //CloseIfOpen();
    }

    void ButtonSelected(bool isSaveButton)
    {
        saveButtonSelected = isSaveButton;
        // start a coroutine and check if the a button is actively selected after a few seconds - close menu then if thats not the case
        aButtonIsSelected = true;
        if (isHidden)
            OpenMenu();
    }

    void ButtonDeselected(bool isSaveButton)
    {
        if (isSaveButton)
            saveButtonSelected = false;
        aButtonIsSelected = false;

        if (counterRoutine != null)
            StopCoroutine(counterRoutine); // cancel current countdown 
        counterRoutine = StartCoroutine(MenuHideCountdown()); // start new countdown
    }

    IEnumerator MenuHideCountdown()
    {
        yield return new WaitForSeconds(2);
        if (!aButtonIsSelected || saveButtonSelected)
        {
            CloseMenu();
        }
    }

    /*
    public void ToggleMenu()
    {
        bool current = anim.GetBool("MenuHidden");

        isHidden = !current;
        anim.SetBool("MenuHidden", !current);
    }
    */

    public bool OpenMenu()
    {
        bool current = anim.GetBool("MenuHidden");
        if (current)
        {
            menuHasOpenedDelegate?.Invoke();
            isHidden = !current;
            anim.SetBool("MenuHidden", isHidden);
            return true;
        }
        return false;
    }

    bool PointerOverMainMenu()
    {
        // do raycast to check if MOUSE pointer is still over the menu
        Vector3 cursorPos = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = cursorPos;

        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult res in results)
        {
            if (res.gameObject.tag == "MainMenu")
                return true;
        }
        return false;
    }

    public bool CloseMenu()
    {
        if (PointerOverMainMenu()) // if pointer is over the main menu, then do not close it
            return false;

        //Debug.Log("Close being called");
        if (!aButtonIsSelected || (saveButtonSelected && !lastSaveButton.gameObject.activeSelf) ) // close menu if no button in main menu is selected, or if last savebutton is marked as selected but is deactivated
        {
            //Debug.Log("In between");
            bool current = anim.GetBool("MenuHidden");
            if (!current)
            {
                isHidden = !current;
                anim.SetBool("MenuHidden", isHidden);
                //Debug.Log("Actually being closed.");
                closeTrigger.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    void CloseIfOpen()
    {
        if (!aButtonIsSelected)
        {
            if (!isHidden)
                CloseMenu();
                //ToggleMenu();
        }
    }

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("MouseOverBar", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("MouseOverBar", false);
    }
    */
}

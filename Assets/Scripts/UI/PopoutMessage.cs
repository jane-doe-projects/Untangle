using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopoutMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageTextLabel;
    [SerializeField] Animator anim;

    public void DisplayMessage(string text)
    {
        messageTextLabel.text = text;
        anim.SetTrigger("Popout");
        GameManager.Instance.soundControl.uiSounds.PlayPopup();
    }


}

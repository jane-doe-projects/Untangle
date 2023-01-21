using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreNotification : MonoBehaviour
{
    CanvasGroup cGroup;
    [SerializeField] ParticleSystem ps;
    [SerializeField] TextMeshProUGUI label;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        cGroup = GetComponent<CanvasGroup>();
        cGroup.alpha = 0;
    }

    public void Notify()
    {
        anim.SetTrigger("Fade");
        if (ps != null)
            ps.Play(true);
            
    }

    public void SetLabel(string lab)
    {
        label.text = "best " + lab;
    }
}

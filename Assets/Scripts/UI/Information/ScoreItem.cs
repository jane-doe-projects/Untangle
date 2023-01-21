using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI current;
    [SerializeField] public TextMeshProUGUI best;

    [SerializeField] string label;
    [SerializeField] TextMeshProUGUI textLabel;

    [SerializeField] ScoreNotification notification;

    bool displayed;

    private void Start()
    {
        textLabel.text = label;
        notification.SetLabel(label);
    }

    public void Notify()
    {
        if (displayed)
            notification.Notify();
    }


    private void OnEnable()
    {
        displayed = true;
    }

    private void OnDisable()
    {
        displayed = false;
    }

}

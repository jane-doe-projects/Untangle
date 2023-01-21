using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleConfirm : MonoBehaviour
{
    [SerializeField] GameObject confirmButton;
    Button btn;

    private void Start()
    {
        confirmButton.SetActive(false);
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleConfirm);
    }

    public void ToggleConfirm()
    {
        confirmButton.SetActive(!confirmButton.activeSelf);
    }
}

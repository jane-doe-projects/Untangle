using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(DisableOnClick);
    }

    public void DisableOnClick()
    {
        gameObject.SetActive(false);
    }
}

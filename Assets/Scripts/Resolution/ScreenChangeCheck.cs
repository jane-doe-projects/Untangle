using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenChangeCheck : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownLabel;
    [SerializeField] ResolutionHandler resHandler;

    float countDown = 0;
    bool counting = false;

    // Update is called once per frame
    void Update()
    {
        if (counting && countDown > 0)
        {
            countDownLabel.text = countDown.ToString("0");
            countDown -= Time.deltaTime;
        } else if (counting && countDown <= 0)
        {
            counting = false;
            resHandler.ReturnToLast();
        }
    }

    private void OnEnable()
    {
        countDown = resHandler.screenTimeOut;
        counting = true;
    }

    private void OnDisable()
    {
        countDown = resHandler.screenTimeOut;
        counting = false;
    }
}

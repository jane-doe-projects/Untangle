using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelTitle;
    [SerializeField] bool showTitle = false;

    [SerializeField] InfoItem nodes;
    [SerializeField] InfoItem sets;
    [SerializeField] InfoItem mode;
    [SerializeField] InfoItem progress;

    [SerializeField] GameObject tracker;

    public bool show;

    public void HideValues()
    {
        levelTitle.gameObject.SetActive(false);
        nodes.gameObject.SetActive(false);
        sets.gameObject.SetActive(false);
        mode.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);

        tracker.SetActive(false);
    }

    public void ShowValues(bool hideProgress = false)
    {
        if (show)
        {
            if (GameManager.Instance.currentSession.initialized)
            {
                if (showTitle)
                    levelTitle.gameObject.SetActive(true);
                tracker.SetActive(true);

                nodes.gameObject.SetActive(true);
                sets.gameObject.SetActive(true);
                mode.gameObject.SetActive(true);
                if (!hideProgress)
                    progress.gameObject.SetActive(true);
                else
                    progress.gameObject.SetActive(false);
            }
        }
    }

    public void SetValues()
    {
        SessionDetails details = GameManager.Instance.currentSession.GetSessionDetails();

        levelTitle.text = details.levelName;
        nodes.value.text = details.nodeCount.ToString();
        sets.value.text = details.setCount.ToString();
        mode.value.text = details.mode;

        if (details.currentCount != -1)
        {
            string formatting = "00";
            if (details.totalCount > 99)
                formatting = "000";
            progress.value.text = details.currentCount.ToString(formatting) + "/" + details.totalCount.ToString(formatting);
            ShowValues(hideProgress: false);
        } else
            ShowValues(hideProgress: true);

    }
}

public class SessionDetails
{
    public string levelName;
    public int nodeCount;
    public int setCount;
    public string mode;
    public int currentCount;
    public int totalCount;
}

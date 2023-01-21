using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PreviewHandler : MonoBehaviour
{
    [SerializeField] GameObject previewContent;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI nodeInfo;
    [SerializeField] NodePreview nodePreview;

    [SerializeField] GameObject lockedOverlay;

    public LevelItemPreview item;
    [SerializeField] ButtonSounds btnSounds;

    // Start is called before the first frame update
    void Start()
    {
        item = GetComponent<LevelItemPreview>();
        SetNodeInfo();
    }


    public void Unlock()
    {
        if (!(item.correspondingLevel.type == LevelType.Favorite))
        {
            lockedOverlay.SetActive(false);
            ShowInfo(true);
            item.btn.enabled = true;
            btnSounds.enabled = true;
        }
    }
            

    public void Lock()
    {
        if (!(item.correspondingLevel.type == LevelType.Favorite))
        {
            lockedOverlay.SetActive(true);
            ShowInfo(false);
            item.btn.enabled = false;
            btnSounds.enabled = false;
        }
    }

    void SetNodeInfo()
    {
        int set = item.correspondingLevel.GetSetCount();
        int node = item.correspondingLevel.GetNodeCount();

        nodeInfo.text = "Sets: " + set + " Nodes: " + node;

        nodePreview.CreateNodePreview();
    }

    void ShowInfo(bool show)
    {
        label.gameObject.SetActive(show);
        nodeInfo.gameObject.SetActive(show);
        nodePreview.gameObject.SetActive(show);
    }

    public void InitState()
    {
        // initial unlock
        lockedOverlay.SetActive(false);
        ShowInfo(true);

        // set lock state
        if (item.correspondingLevel.IsLocked())
            Lock();
        else
            Unlock();
    }
}

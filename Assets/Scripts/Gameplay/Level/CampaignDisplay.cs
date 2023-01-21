using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignDisplay : MonoBehaviour
{/*
    [SerializeField] GameObject selectionPanel;
    [SerializeField] List<GameObject> displayedLevels;
    [SerializeField] GameObject levelDisplayPrefab;
    [SerializeField] GameObject campaignDisplayPrefab;


    [SerializeField] GameObject campaignsSection;

    List<LevelSet> sets;
    List<GameObject> campaignPreviews;

    LevelSet selectedCampaign;

    // Start is called before the first frame update
    void Start()
    {
        ThemeSelect.themeChangeDelegate += UpdateVisuals;

        displayedLevels = new List<GameObject>();
        campaignPreviews = new List<GameObject>();
        sets = GameManager.Instance.campaignManager.GetSets();

        //LoadLevels();
        LoadCampaigns();
    }

    void LoadLevels()
    {
        List<LevelSet> sets = GameManager.Instance.campaignManager.GetSets();

        List<Level> allLevels = new List<Level>();

        foreach (LevelSet set in sets)
        {
            allLevels.AddRange(set.levels);
        }

        foreach (Level lvl in allLevels)
        {
            GameObject tempLevel = Instantiate(levelDisplayPrefab, selectionPanel.transform);
            InitLevelDisplay(tempLevel, lvl);
            displayedLevels.Add(tempLevel);
        }
    }

    void InitLevelDisplay(GameObject display, Level level)
    {
        display.GetComponent<LevelPreview>().SetState(level);
    }

    void InitCampaignDisplay(GameObject display, LevelSet set)
    {
        display.GetComponent<CampaignPreview>().SetState(set);
    }

    void LoadCampaigns()
    {
        foreach (LevelSet set in sets)
        {
            GameObject tempCamp = Instantiate(campaignDisplayPrefab, campaignsSection.transform);
            InitCampaignDisplay(tempCamp, set);
            campaignPreviews.Add(tempCamp);
        }
    }

    public void LoadLevelsForCampaign(LevelSet set)
    {
        selectedCampaign = set;
        UpdateStateOfCampaigns();

        if (displayedLevels.Count > 0)
            ClearDisplayedLevels();

        foreach (Level lvl in selectedCampaign.levels)
        {
            GameObject tempLevel = Instantiate(levelDisplayPrefab, selectionPanel.transform);
            InitLevelDisplay(tempLevel, lvl);
            displayedLevels.Add(tempLevel);
        }
    }

    void ClearDisplayedLevels()
    {
        foreach (GameObject obj in displayedLevels)
        {
            Destroy(obj);
        }
        displayedLevels.Clear();
    }

    void UpdateVisuals()
    {
        foreach (GameObject obj in campaignPreviews)
        {
            Image tempImg = obj.GetComponent<Image>();
            tempImg.sprite = GameManager.Instance.currentTheme.nodeOptions[0];
        }

        foreach (GameObject obj in displayedLevels)
        {
            Image tempImg = obj.GetComponent<Image>();
            tempImg.sprite = GameManager.Instance.currentTheme.nodeOptions[0];
        }
    }

    public void UpdateStateOfCampaigns()
    {
        foreach (GameObject obj in campaignPreviews)
        {
            CampaignPreview prev = obj.GetComponent<CampaignPreview>();
            if ((prev.correspondingCampaign != selectedCampaign))
                prev.Deselect();
        }
    }
    */
}

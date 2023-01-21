using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI groupTitel;
    [SerializeField] TextMeshProUGUI levelCountProgress;
    [SerializeField] GameObject levelOptions;
    [SerializeField] GameObject groupOptions;

    [SerializeField] List<GameObject> displayedItems;
    [SerializeField] List<GameObject> displayedGroups;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject groupPrefab;

    [SerializeField] GameObject levelNotification;

    GroupItemPreview selectedGroup;
    public int lastSelectedIndex;
    public bool isFavoriteDisplay = false;

    [Header("Scrollable")]
    [SerializeField] bool scrollable = false;
    [SerializeField] ScrollControl scrollControl;

    [SerializeField] InfoHover infoHover;
    [SerializeField] bool infoHoverActive = false;

    private void Start()
    {
        displayedGroups = new List<GameObject>();
        displayedItems = new List<GameObject>();

        if (scrollable == true)
            scrollControl.ActivateScroll();

        if (!infoHoverActive)
            infoHover.Deactivate();
    }

    public void AddGroupItem(string titel, LevelSet set, DisplayWindow targetDisplay, Difficulty diff = Difficulty.None, bool isEditable = false)
    {
        GameObject groupObj = Instantiate(groupPrefab, groupOptions.transform);
        displayedGroups.Add(groupObj);

        if (diff != Difficulty.None)
            set.difficulty = diff;


        GroupItemPreview grpItem = groupObj.GetComponent<GroupItemPreview>();
        grpItem.isEditable = isEditable;
        grpItem.targetDisplay = targetDisplay;
        grpItem.set = set;
        grpItem.btnLabel.text = titel;
        grpItem.SetState();
    }

    void ClearLevelDisplay()
    {
        foreach (Transform child in levelOptions.transform)
        {
            Destroy(child.gameObject);
        }
        displayedItems.Clear();
        DisplayLevelNotification(false);
    }

    public void ClearGroupDisplay()
    {
        foreach (Transform child in groupOptions.transform)
            Destroy(child.gameObject);
        displayedGroups.Clear();
    }

    public void DisplayLevelNotification(bool displaying)
    {
        levelNotification.SetActive(displaying);
    }

    public bool HasMoreDisplayedItems()
    {
        if (levelOptions.transform.childCount > 1)
            return true;

        /*
        Debug.Log(displayedItems.Count); // for some reason this is 0
        if (displayedItems.Count > 1)
            return true; */
        return false;
    }

    public void AddLevelItems(LevelSet set, GroupItemPreview group, bool isEditable = false)
    {
        // note selected group item
        selectedGroup = group;

        // set titel for currently selected group
        groupTitel.text = group.btnLabel.text;

        ClearLevelDisplay();
        int unlockedCount = 0;
        foreach (Level lvl in set.levels)
        {
            // create a item gameobject in the item display
            GameObject itemObj = Instantiate(itemPrefab, levelOptions.transform);
            displayedItems.Add(itemObj);

            // initialize item with relevant visuals and functionality
            LevelItemPreview lvlItem = itemObj.GetComponent<LevelItemPreview>();
            lvlItem.isEditable = isEditable;
            lvlItem.correspondingLevel = lvl;
            lvlItem.correspondingGroup = group;
            lvlItem.diff = set.difficulty;
            lvlItem.btnLabel.text = lvl.descName;
            lvlItem.correspondingSet = group.set;
            lvlItem.indexInSet = LevelItemPreview.GetIndexInSet(lvl, set);
            lvlItem.SetState();

            if (isEditable)
                lvlItem.type = SessionType.Favorite;
            else
                lvlItem.type = SessionType.Campaign;

            lvlItem.InitPreview();

            if (!lvl.IsLocked())
                unlockedCount++;
        }

        // init progress level count
        if (isEditable) // favorites only show count
        {
            levelCountProgress.text = set.levels.Count.ToString() + " Levels";
        }
        else
        {
            levelCountProgress.text = unlockedCount.ToString() + "/" + set.levels.Count.ToString() + " Levels unlocked";
        }

        if (set.levels.Count == 0)
            DisplayLevelNotification(true);
    }

    private void OnEnable()
    {
        // show last selected group
        selectedGroup = displayedGroups[lastSelectedIndex].GetComponent<GroupItemPreview>();
            
        AddLevelItems(selectedGroup.set, selectedGroup, isFavoriteDisplay);
    }

    public void SetSelectedIndex(GameObject previewItem)
    {
        int count = 0;
        foreach (Transform ob in groupOptions.transform)
        {
            if (ob.gameObject == previewItem)
                lastSelectedIndex = count;
            count++;
        }
    }

    public void UpdateLevelProgress(LevelSet set)
    {
        int unlockedCount = 0;
        foreach (Level lvl in set.levels)
        {
            if (lvl.IsLocked())
                unlockedCount++;
        }

        if (!set.isCampaign) // favorites only show count
        {
            levelCountProgress.text = set.levels.Count.ToString() + " Levels";
        }
        else
        {
            levelCountProgress.text = unlockedCount.ToString() + "/" + set.levels.Count.ToString() + " Levels unlocked";
        }
    }
}

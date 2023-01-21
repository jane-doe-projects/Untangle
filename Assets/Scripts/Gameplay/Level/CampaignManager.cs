using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    [SerializeField] List<LevelSet> sets;

    [SerializeField] DisplayWindow campaignDisplayWindow;
    WindowControl windowControl;

    private void Start()
    {
        windowControl = GameManager.Instance.windowControl;

        InitSuccessors();
        InitIds();
    }

    public void DisplayCampaings()
    {
        InitGroups();
        SetLockState();

        windowControl.ShowCampaignSelect();
    }

    void InitGroups()
    {
        campaignDisplayWindow.ClearGroupDisplay();

        foreach (LevelSet set in sets)
        {
            campaignDisplayWindow.AddGroupItem(set.descName, set, campaignDisplayWindow);
        }
    }

    void InitSuccessors()
    {
        foreach (LevelSet set in sets)
        {
            for (int i = 0; i < set.levels.Count-1; i++)
            {
                set.levels[i].SetSuccessor(set.levels[i + 1]);
            }
        }
    }

    void InitIds()
    {
        foreach (LevelSet set in sets)
        {
            set.SetIds();
        }
    }

    void SetLockState()
    {
        foreach (LevelSet set in sets)
        {
            int lockIndex = GameManager.Instance.serManager.GetLockIndexForCampaign(set);

            // set locks for all locked levels for each campaign
            for (int i = lockIndex+1; i < set.levels.Count; i++)
            {
                Level tempLvl = set.levels[i];
                tempLvl.Lock();
            }

        }
    }

    public List<LevelSet> GetCampaigns()
    {
        return sets;
    }

    public int GetUnlockedCount()
    {
        int totalUnlocked = 0;

        foreach (LevelSet set in sets)
        {
            int lockIndex = GameManager.Instance.serManager.GetLockIndexForCampaign(set);
            totalUnlocked++; // foreach campaign since first level is always unlocked
            totalUnlocked += lockIndex; // plus amount of levels that are unlocked
        }

        /*foreach (LevelSet set in sets)
        {
            foreach (Level lvl in set.levels)
            {
                if (!lvl.IsLocked())
                    totalUnlocked++;
            }
        }

        // correction of unlock count - since first level of a campaign is always unlocked
        //totalUnlocked -= sets.Count; */

        return totalUnlocked;
    }

    public int GetTotalLevelsCount()
    {
        int total = 0;
        foreach (LevelSet set in sets)
            total += set.levels.Count;
        return total;
    }

}



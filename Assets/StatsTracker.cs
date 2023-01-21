using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class StatsTracker : MonoBehaviour
{
    static int total_moves;
    static int easy_solved;
    static int medium_solved;
    static int hard_solved;
    static int any_solved;
    static int total_saved;
    static int total_reset;

    static int maxMoveAchievementLimit = 5000;

    public void InitializeAllStats()
    {
        SteamUserStats.GetStat("TOTAL_MOVES", out total_moves);
        SteamUserStats.GetStat("EASY_SOLVED", out easy_solved);
        SteamUserStats.GetStat("MEDIUM_SOLVED", out medium_solved);
        SteamUserStats.GetStat("HARD_SOLVED", out hard_solved);
        SteamUserStats.GetStat("ANY_SOLVED", out any_solved);
        SteamUserStats.GetStat("TOTAL_SAVED", out total_saved);
        SteamUserStats.GetStat("TOTAL_RESET", out total_reset);

        // saving and updating values from playerprefs in case player is offline

        //PrintStatsValues();

        if (total_moves < maxMoveAchievementLimit)
            Node.onSwapDelegate += IncreaseMove;
    }

    void PrintStatsValues()
    {
        Debug.Log("moves: " + total_moves);
        Debug.Log("easy_s: " + easy_solved);
        Debug.Log("medium_s: " + medium_solved);
        Debug.Log("hard_s: " + hard_solved);
        Debug.Log("any_s: " + any_solved);
        Debug.Log("total_sav: " + total_saved);
        Debug.Log("total_res: " + total_reset);
    }

    public static void IncreaseMove()
    {
        if (SteamManager.Initialized)
        {
            if (total_moves < maxMoveAchievementLimit)
            {
                // increase and update move count
                total_moves++;
                SteamUserStats.SetStat("TOTAL_MOVES", total_moves);
                SteamUserStats.StoreStats();
            }
        }
        // save stuff to player prefs
    }

    public static void IncreaseForDifficulty(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Easy:
                IncreaseEasySolved();
                break;
            case Difficulty.Medium:
                IncreaseMediumSolved();
                break;
            case Difficulty.Hard:
                IncreaseHardSolved();
                break;
        }

        // check if all are at least 50 - only check when one of them hit 50 - else it will be triggered all the time - we dont want that - trigger things in threes achievement 
        if (easy_solved == 50 || medium_solved == 50 || hard_solved == 50)
        {
            if (DifficultiesFifty() && SteamManager.Initialized)
                SteamAchievements.AwardAchievement(Achievement.THINGS_IN_THREES);
        }
    }

    public static void IncreaseEasySolved()
    {
        if (SteamManager.Initialized)
        {
            if (easy_solved < 100)
            {
                easy_solved++;
                SteamUserStats.SetStat("EASY_SOLVED", easy_solved);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void IncreaseMediumSolved()
    {
        if (SteamManager.Initialized)
        {
            if (medium_solved < 100)
            {
                medium_solved++;
                SteamUserStats.SetStat("MEDIUM_SOLVED", medium_solved);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void IncreaseHardSolved()
    {
        if (SteamManager.Initialized)
        {
            if (hard_solved < 100)
            {
                hard_solved++;
                SteamUserStats.SetStat("hard_SOLVED", hard_solved);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void IncreaseSolved()
    {
        if (SteamManager.Initialized)
        {
            if (any_solved < 1000)
            {
                any_solved++;
                SteamUserStats.SetStat("ANY_SOLVED", any_solved);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void IncreaseSaved()
    {
        if (SteamManager.Initialized)
        {
            if (total_saved < 150)
            {
                total_saved++;
                SteamUserStats.SetStat("TOTAL_SAVED", total_saved);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void IncreaseReset()
    {
        if (SteamManager.Initialized)
        {
            if (total_reset < 50)
            {
                total_reset++;
                SteamUserStats.SetStat("TOTAL_RESET", total_reset);
                SteamUserStats.StoreStats();
            }
        }
    }

    static bool DifficultiesFifty()
    {
        if (easy_solved < 50 || medium_solved < 50 || hard_solved < 50)
            return false;
        return true;
    }

    public static void CampaignComplete(int campaignId)
    {
        string campaignString = "campaign" + campaignId.ToString();
        PlayerPrefs.SetInt(campaignString, 1);

        // check if all campaigns are set ids: 9221, 9222, 9223
        int cOne = PlayerPrefs.GetInt("campaign9221");
        int cTwo = PlayerPrefs.GetInt("campaign9222");
        int cThree = PlayerPrefs.GetInt("campaign9223");

        if (SteamManager.Initialized)
        {
            if (campaignId == 9221)
                SteamAchievements.AwardAchievement(Achievement.CAMPAIGN_VANILLA);
            else if (campaignId == 9222)
                SteamAchievements.AwardAchievement(Achievement.CAMPAIGN_MIRROR);
            else if (campaignId == 9223)
                SteamAchievements.AwardAchievement(Achievement.CAMPAIGN_GEOMETRIC);


            if (cOne == 1 && cTwo == 1 && cThree == 1)
                SteamAchievements.AwardAchievement(Achievement.PUZZLER);
        }
        
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class SteamAchievements : MonoBehaviour
{
    [SerializeField] StatsTracker statsTracker;

    [SerializeField] Achievement currentTest;
    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized)
            return;

        statsTracker.InitializeAllStats();
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AwardAchievement(currentTest);
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            ResetAchievement(currentTest);
        } else if (Input.GetKeyDown(KeyCode.L))
        {
            ResetAllAchievements();
        }
    }
    */

    public static void AwardAchievement(Achievement achiv)
    {
        bool success = SteamUserStats.SetAchievement(achiv.ToString());
        SteamUserStats.StoreStats();
        Debug.Log("Tried to award " + achiv.ToString() + " achievement. Success: " + success.ToString() );
    }

   
    void ResetAchievement(Achievement achiv)
    {

    }

    void ResetAllAchievements()
    {
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.StoreStats();
    }
}

public enum Achievement
{
    SOLVER_1,
    SOLVER_2,
    SOLVER_3,
    THINGS_IN_THREES,
    LIKE_PUZZLES,
    COLLECTOR_FAVORITES,
    PUZZLER,
    PUZZLE_NOVICE,
    PUZZLE_RECRUIT,
    PUZZLE_EXPERT,
    PUZZLE_MASTER,
    PUZZLE_GRANDMASTER,
    REDOER,
    PUZZLE_TACTICIAN,
    PUZZLE_JUNKIE_EASY,
    PUZZLE_JUNKIE_MEDIUM,
    PUZZLE_JUNKIE_HARD,
    CAMPAIGN_VANILLA,
    CAMPAIGN_MIRROR,
    CAMPAIGN_GEOMETRIC,
    PUZZLE_CHILLING,
    PUZZLE_THAT_EASY

}

public enum Stats
{
    TOTAL_MOVES,
    EASY_SOLVED,
    MEDIUM_SOLVED,
    HARD_SOLVED,
    ANY_SOLVED,
    TOTAL_SAVED,
    TOTAL_RESET,
    TOTAL_SOLVED
}

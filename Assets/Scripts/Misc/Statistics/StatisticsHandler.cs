using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsHandler : MonoBehaviour
{
    [SerializeField] GameObject statItemPrefab;
    [SerializeField] GameObject statGroupPrefab;

    [SerializeField] StatisticsBlock endlessStatsBlock;
    List<Statistic> endlessStats;
    [SerializeField] StatisticsBlock campaignStatsBlock;
    List<Statistic> campaignStats;
    [SerializeField] StatisticsBlock favoriteStatsBlock;
    List<Statistic> favoriteStats;

    [SerializeField] TextMeshProUGUI recordedSinceLabel;
    [SerializeField] MenuButton resetStatsButton;

    string timeTextFormat = "0.00";

    private void Start()
    {
        WindowControl.onStatisticsOpen += InitializeBlocks;
        InitializeStatistics();
    }

    void InitializeStatistics()
    {
        StatisticsCollection.LoadCampaignCollection();
        StatisticsCollection.LoadFavoriteCollection();
        StatisticsCollection.LoadEndlessCollection();
    }

    public void AddToStatistic(SessionType type, Difficulty difficulty, float time, int moves)
    {
        switch (type)
        {
            case SessionType.Random:
                StatisticsCollection.UpdateEndless(difficulty, time, moves);
                break;
            case SessionType.Campaign:
                StatisticsCollection.UpdateCampaign(time, moves);
                break;
            case SessionType.Favorite:
                StatisticsCollection.UpdateFavorite(difficulty, time, moves);
                break;
        }
    }

    public void InitializeBlocks()
    {
        UpdateRecordInfo();
        UpdateEndless();
        UpdateCampaign();
        UpdateFavorite();
    }

    void UpdateEndless()
    {
        endlessStats = StatisticsCollection.GetEndlessStats();

        foreach (Transform child in endlessStatsBlock.statsParent.transform)
        {
            foreach (Transform childsChild in child)
                Destroy(childsChild.gameObject);
            Destroy(child.gameObject);
        }
            

        foreach (Statistic stat in endlessStats)
        {
            StatisticGroup statGroup = Instantiate(statGroupPrefab, endlessStatsBlock.statsParent.transform).GetComponent<StatisticGroup>();
            statGroup.SetLabel(stat.descName);

            StatisticItem solved = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgTime = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgMoves = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();

            solved.SetLabels("Solved levels", stat.totalCount.ToString());
            avrgTime.SetLabels("Average time to solve", stat.averageTime.ToString(timeTextFormat));
            avrgMoves.SetLabels("Average moves to solve", stat.averageMoves.ToString());
        }
    }

    void UpdateCampaign()
    {
        campaignStats = StatisticsCollection.GetCampaignStats();

        foreach (Transform child in campaignStatsBlock.statsParent.transform)
        {
            foreach (Transform childsChild in child)
                Destroy(childsChild.gameObject);
            Destroy(child.gameObject);
        }

        StatisticGroup statGroup = Instantiate(statGroupPrefab, campaignStatsBlock.statsParent.transform).GetComponent<StatisticGroup>();
        statGroup.SetLabel("All");
        StatisticItem solved = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
        StatisticItem avrgTime = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
        StatisticItem avrgMoves = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();

        int unlockedCount = GameManager.Instance.campaignManager.GetUnlockedCount();
        int totalCount = GameManager.Instance.campaignManager.GetTotalLevelsCount();

        solved.SetLabels("Unlocked campaigns", unlockedCount + " / " + totalCount);
        avrgTime.SetLabels("Average time to solve", campaignStats[0].averageTime.ToString(timeTextFormat));
        avrgMoves.SetLabels("Average moves to solve", campaignStats[0].averageMoves.ToString());

    }

    void UpdateFavorite()
    {
        favoriteStats = StatisticsCollection.GetFavoriteStats();

        foreach (Transform child in favoriteStatsBlock.statsParent.transform)
        {
            foreach (Transform childsChild in child)
                Destroy(childsChild.gameObject);
            Destroy(child.gameObject);
        }


        foreach (Statistic stat in favoriteStats)
        {
            StatisticGroup statGroup = Instantiate(statGroupPrefab, favoriteStatsBlock.statsParent.transform).GetComponent<StatisticGroup>();
            statGroup.SetLabel(stat.descName);

            StatisticItem solved = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgTime = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgMoves = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();

            int favsCount = GameManager.Instance.favoritesManager.GetLevelCountForDifficulty(stat.difficulty);
            solved.SetLabels("Saved levels", favsCount.ToString());
            avrgTime.SetLabels("Average time to solve", stat.averageTime.ToString(timeTextFormat));
            avrgMoves.SetLabels("Average moves to solve", stat.averageMoves.ToString());
        }
    }

    void UpdateGroup(SessionType type)
    {
        List<Statistic> targetList = StatisticsCollection.GetStats(type);

        foreach (Transform child in endlessStatsBlock.statsParent.transform)
        {
            foreach (Transform childsChild in child)
                Destroy(childsChild.gameObject);
            Destroy(child.gameObject);
        }


        foreach (Statistic stat in endlessStats)
        {
            StatisticGroup statGroup = Instantiate(statGroupPrefab, endlessStatsBlock.statsParent.transform).GetComponent<StatisticGroup>();
            statGroup.SetLabel(stat.descName);

            StatisticItem solved = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgTime = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();
            StatisticItem avrgMoves = Instantiate(statItemPrefab, statGroup.itemParent.transform).GetComponent<StatisticItem>();

            solved.SetLabels("Solved levels", stat.totalCount.ToString());
            avrgTime.SetLabels("Average time to solve", stat.averageTime.ToString(timeTextFormat));
            avrgMoves.SetLabels("Average moves to solve", stat.averageMoves.ToString());
        }
    }

    public void ResetStats()
    {
        StatisticsCollection.ResetAllStats();
        UpdateEndless();
        UpdateCampaign();
        UpdateFavorite();

        // update subtitle recording information and display popup message
        UpdateRecordInfo();
        GameManager.Instance.popoutMessage.DisplayMessage("Statistics reset");

    }

    void UpdateRecordInfo()
    {
        string savedTime = StatisticsCollection.GetRecordingStart();
        recordedSinceLabel.text = "Recorded since " + savedTime;
    }
}


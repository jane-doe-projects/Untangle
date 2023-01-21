using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatisticsCollection
{
    // endless collection
    static Statistic easy;
    static Statistic medium;
    static Statistic hard;
    static List<Difficulty> endlessPrefixes = new List<Difficulty> { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard};

    static Statistic campaign;
    static Statistic favoriteEasy;
    static Statistic favoriteMedium;
    static Statistic favoriteHard;

    static string dateTimeFormat = "dd/MM/yyyy HH:mm";

    public static void UpdateEndless(Difficulty difficulty, float time, int moves) 
    {
        Statistic targetStat = null;
        switch (difficulty)
        {
            case Difficulty.Easy:
                targetStat = easy;
                break;
            case Difficulty.Medium:
                targetStat = medium;
                break;
            case Difficulty.Hard:
                targetStat = hard;
                break;
        }

        if (targetStat == null)
            return;

        string statPrefix = difficulty.ToString();

        targetStat.totalCount += 1;
        targetStat.totalMoves += moves;
        targetStat.totalTime += time;
        targetStat.averageMoves = targetStat.totalMoves / targetStat.totalCount;
        targetStat.averageTime = targetStat.totalTime / targetStat.totalCount;

        PlayerPrefs.SetInt(statPrefix + "TotalCount", targetStat.totalCount);
        PlayerPrefs.SetInt(statPrefix + "TotalMoves", targetStat.totalMoves);
        PlayerPrefs.SetFloat(statPrefix + "TotalTime", targetStat.totalTime);
        // dont need to save rest because it can be computed out of saved values during runtime
        PlayerPrefs.Save();
    }

    public static void UpdateCampaign(float time, int moves)
    {
        string statPrefix = campaign.type.ToString();
        campaign.totalCount += 1;
        campaign.totalMoves += moves;
        campaign.totalTime += time;

        campaign.averageMoves = campaign.totalMoves / campaign.totalCount;
        campaign.averageTime = campaign.totalTime / campaign.totalCount;

        PlayerPrefs.SetInt(statPrefix + "TotalCount", campaign.totalCount);
        PlayerPrefs.SetInt(statPrefix + "TotalMoves", campaign.totalMoves);
        PlayerPrefs.SetFloat(statPrefix + "TotalTime", campaign.totalTime);
        // dont need to save rest because it can be computed out of saved values during runtime
        PlayerPrefs.Save();
    }

    public static void UpdateFavorite(Difficulty difficulty, float time, int moves)
    {
        Statistic targetStat = null;
        switch (difficulty)
        {
            case Difficulty.Easy:
                targetStat = favoriteEasy;
                break;
            case Difficulty.Medium:
                targetStat = favoriteMedium;
                break;
            case Difficulty.Hard:
                targetStat = favoriteHard;
                break;
        }

        if (targetStat == null)
            return;

        string statPrefix = "Favorite" + difficulty.ToString();

        targetStat.totalCount += 1;
        targetStat.totalMoves += moves;
        targetStat.totalTime += time;
        targetStat.averageMoves = targetStat.totalMoves / targetStat.totalCount;
        targetStat.averageTime = targetStat.totalTime / targetStat.totalCount;

        PlayerPrefs.SetInt(statPrefix + "TotalCount", targetStat.totalCount);
        PlayerPrefs.SetInt(statPrefix + "TotalMoves", targetStat.totalMoves);
        PlayerPrefs.SetFloat(statPrefix + "TotalTime", targetStat.totalTime);
        // dont need to save rest because it can be computed out of saved values during runtime
        PlayerPrefs.Save();
    }

    public static void LoadEndlessCollection()
    {
        easy = LoadStatsForDifficulty(Difficulty.Easy);
        medium = LoadStatsForDifficulty(Difficulty.Medium);
        hard = LoadStatsForDifficulty(Difficulty.Hard);
    }

    public static void LoadFavoriteCollection()
    {
        favoriteEasy = LoadStatsForDifficulty(Difficulty.Easy, isFavorite: true);
        favoriteMedium = LoadStatsForDifficulty(Difficulty.Medium, isFavorite: true);
        favoriteHard = LoadStatsForDifficulty(Difficulty.Hard, isFavorite: true);
    }

    static Statistic LoadStatsForDifficulty(Difficulty difficulty, bool isFavorite = false)
    {
        Statistic stats = new Statistic();
        stats.descName = difficulty.ToString();
        stats.difficulty = difficulty;
        stats.type = SessionType.Random;

        string statPrefix = difficulty.ToString();
        if (isFavorite)
            statPrefix = "Favorite" + difficulty.ToString();

        stats.totalCount = PlayerPrefs.GetInt(statPrefix + "TotalCount", 0);

        if (stats.totalCount == 0)
            return stats;
        stats.totalTime = PlayerPrefs.GetFloat(statPrefix + "TotalTime", 0);
        stats.averageTime = stats.totalTime / stats.totalCount;

        stats.totalMoves = PlayerPrefs.GetInt(statPrefix + "TotalMoves", 0);
        stats.averageMoves = stats.totalMoves / stats.totalCount;

        return stats;
    }

    public static void LoadCampaignCollection()
    {
        campaign = new Statistic();
        campaign.descName = "Campaigns";
        campaign.difficulty = Difficulty.None;
        campaign.type = SessionType.Campaign;

        string prefix = campaign.type.ToString();

        campaign.totalCount = PlayerPrefs.GetInt(prefix + "TotalCount", 0);
        campaign.totalTime = PlayerPrefs.GetFloat(prefix + "TotalTime", 0);
        campaign.totalMoves = PlayerPrefs.GetInt(prefix + "TotalMoves", 0);
        campaign.averageTime = 0;
        campaign.averageMoves = 0;

        if (campaign.totalCount == 0)
            return;

        campaign.averageTime = campaign.totalTime / campaign.totalCount;
        campaign.averageMoves = campaign.totalMoves / campaign.totalCount;
    }

    public static List<Statistic> GetEndlessStats()
    {
        LoadEndlessCollection();
        List<Statistic> stats = new List<Statistic>();
        stats.Add(easy);
        stats.Add(medium);
        stats.Add(hard);
        return stats;
    }

    public static List<Statistic> GetCampaignStats()
    {
        LoadCampaignCollection();
        List<Statistic> stats = new List<Statistic>();
        stats.Add(campaign);
        return stats;
    }

    public static List<Statistic> GetFavoriteStats()
    {
        LoadFavoriteCollection();
        List<Statistic> stats = new List<Statistic>();
        stats.Add(favoriteEasy);
        stats.Add(favoriteMedium);
        stats.Add(favoriteHard);
        return stats;
    }

    public static void ResetAllStats()
    {
        Statistic targetStat = null;

        string statPrefix = "";
        foreach (Difficulty difficulty in endlessPrefixes)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    targetStat = easy;
                    break;
                case Difficulty.Medium:
                    targetStat = medium;
                    break;
                case Difficulty.Hard:
                    targetStat = hard;
                    break;
            }

            if (targetStat == null)
                return;

            statPrefix = difficulty.ToString();

            PlayerPrefs.SetInt(statPrefix + "TotalCount", 0);
            PlayerPrefs.SetInt(statPrefix + "TotalMoves", 0);
            PlayerPrefs.SetFloat(statPrefix + "TotalTime", 0);

            // reset for favorites
            statPrefix = "Favorite" + difficulty.ToString();
            PlayerPrefs.SetInt(statPrefix + "TotalCount", 0);
            PlayerPrefs.SetInt(statPrefix + "TotalMoves", 0);
            PlayerPrefs.SetFloat(statPrefix + "TotalTime", 0);
        }

        // reset for campaign
        statPrefix = "Campaign";
        PlayerPrefs.SetInt(statPrefix + "TotalCount", 0);
        PlayerPrefs.SetInt(statPrefix + "TotalMoves", 0);
        PlayerPrefs.SetFloat(statPrefix + "TotalTime", 0);

        // reset "start of recording date"
        PlayerPrefs.SetString("RecordingStart", DateTime.Now.ToString(dateTimeFormat));
        PlayerPrefs.Save();
    }

    public static string GetRecordingStart()
    {
        string start = PlayerPrefs.GetString("RecordingStart");
        return start;
    }

    public static void InitializeStatsRecordingDate()
    {
        string start = PlayerPrefs.GetString("RecordingStart", "0");
        if (start == "0")
        {
            PlayerPrefs.SetString("RecordingStart", DateTime.Now.ToString(dateTimeFormat));
            PlayerPrefs.Save();
        }
    }

    public static List<Statistic> GetStats(SessionType type)
    {
        switch(type)
        {
            case SessionType.Random:
                return GetEndlessStats();
            case SessionType.Campaign:
                return GetCampaignStats();
            /*case SessionType.Favorite:
                return GetFavoriteStats();*/

        }
        return null;
    }
}

public class Statistic
{
    public string descName;
    public SessionType type;
    public Difficulty difficulty;

    public int totalCount;
    public float totalTime;
    public float averageTime;
    public int totalMoves;
    public int averageMoves;
}

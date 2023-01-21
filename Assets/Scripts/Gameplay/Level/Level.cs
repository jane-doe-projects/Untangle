using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Campaign/New Level")]
public class Level : ScriptableObject
{
    bool locked = false;

    public LevelType type;
    public Level successor;

    public float bestTime;
    public int fewestClicks;

    public int id;
    public int cId;
    public long fId;

    public string descName;
    //public List<NodeSet> nodeSets;
    public List<SingleSetCoordinates> nodeSetsCoordinates;

    public void AddNodeSet(SingleSetCoordinates set)
    {
        nodeSetsCoordinates.Add(set);
    }

    public bool HasSuccessor()
    {
        if (successor != null)
            return true;
        return false;
    }

    public Level GetSuccessor()
    {
        if (HasSuccessor())
            return successor;
        return null;
    }

    public void SetSuccessor(Level lvl)
    {
        successor = lvl;
    }

    public CampaignSaveData GetBestScore()
    {
        CampaignSaveData score = GameManager.Instance.serManager.LoadCampaignScore(cId, id);
        return score;
    }

    public FavoriteSaveData GetBestScoreFavorite()
    {
        FavoriteSaveData score = GameManager.Instance.serManager.LoadFavoriteScore(fId);
        return score;
    }

    public void UpdateBestMoves(int moves)
    {
        fewestClicks = moves;
    }

    public void UpdateBestTime(float time)
    {
        bestTime = time;
    }

    public int GetNodeCount()
    {
        int count = 0;

        foreach (SingleSetCoordinates coordSet in nodeSetsCoordinates)
            count += coordSet.coordsForSet.Count;

        return count;
    }

    public int GetSetCount()
    {
        return nodeSetsCoordinates.Count;
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    public bool IsLocked()
    {
        return locked;
    }


}

[System.Serializable]
public class SingleSetCoordinates
{
    public List<Vector2> coordsForSet;
}

[System.Serializable]
public class BestScore
{
    public float fastestTime;
    public int fewestMoves;
}

[System.Serializable]
public enum LevelType
{
    Campaign,
    Favorite,
    Random
}


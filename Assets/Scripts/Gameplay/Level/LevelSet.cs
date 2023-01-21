using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Set", menuName = "Campaign/New Level Set")]
public class LevelSet : ScriptableObject
{
    public string descName;
    public string description;
    public List<Level> levels;

    public Difficulty difficulty;

    public bool isCampaign;
    [SerializeField] public int campaignId;

    public void SetIds()
    {
        int count = 0;
        foreach (Level lvl in levels)
        {
            lvl.id = count;
            count++;
            lvl.cId = campaignId;
        }
    }
}

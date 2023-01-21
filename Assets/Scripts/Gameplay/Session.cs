using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    public delegate void OnLevelStart();
    public static OnLevelStart onLevelStartDelegate;

    public delegate void OnLevelReset();
    public static OnLevelReset onLevelResetDelegate;

    public delegate void OnSolve();
    public static event OnSolve onSolveDelegate;

    bool solveCalled = false;

    [SerializeField] public Tracker tracker;
    [SerializeField] SessionInfo info;

    [SerializeField] StatisticsHandler statisticsHandler;

    [SerializeField] List<GameObject> nodeSets;
    List<NodeSet> nodeSetsComponents;
    [SerializeField] List<ColliderLine> colliderLines;
    [SerializeField] List<Collider2D> nodeColliders;
    public bool initialized = false;

    public bool solved = false;
    public bool isSaved = false;
    public Level currentLevel;

    Difficulty currentDifficulty;

    List<SingleSetCoordinates> startingPointsOfCurrentSession;

    [SerializeField] SolvedManager solvedManager;

    public static bool justLoaded = false;


    /******************************************/

    LevelSet listOfLevels;
    int currentLevelIndex;
    public SessionType currentType;

    /******************************************/


    private void Start()
    {
        tracker.HideValues();
        info.HideValues();
    }

    private void Update()
    {
        if (initialized)
        {
            if (( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ) && NodeSwitch.selected != null)
                HandelSwap();
        }
    }


    /*********************************************************************************/


    public void NextLevel(bool withSound = true, bool shouldPauseOnStart = false)
    {
        solveCalled = false;
        if (currentType != SessionType.Random)
        {
            
            // check if there is a successor level available
            if (currentLevelIndex < listOfLevels.levels.Count - 1)
            {
                currentLevelIndex++;
                StartNewLevel(currentType, listOfLevels, currentLevelIndex);
                if (withSound)
                    GameManager.Instance.soundControl.PlayNew();

                if (shouldPauseOnStart)
                    solvedManager.PauseGame();
            } else
            {
                if (currentType == SessionType.Campaign)
                {
                    GameManager.Instance.windowControl.ShowCampaignComplete();
                    // save completion of specific campaign to player prefs and check if all campaigns where solved
                    StatsTracker.CampaignComplete(listOfLevels.campaignId);
                }
                else
                    GameManager.Instance.windowControl.ShowFavoritesComplete();
                // reset session
                GameManager.Instance.currentSession.ResetSessionState();
            }
            return;
        }

        // else start a new level with set difficulty
        StartNewLevel(currentDifficulty);
        if (withSound)
            GameManager.Instance.soundControl.PlayNew();

        if (shouldPauseOnStart)
            solvedManager.PauseGame();

    }


    public void StartNewLevel(SessionType type, LevelSet set, int levelIndex)
    {
        if (initialized)
            ResetSessionState();

        // make materials invisible
        GameManager.Instance.gameElements.fadeController.HideMaterials();

        currentType = type;
        currentLevelIndex = levelIndex;

        if (type == SessionType.Favorite)
            currentDifficulty = set.difficulty;
        else
            currentDifficulty = Difficulty.None;

        onLevelStartDelegate();

        listOfLevels = set;
        currentLevel = set.levels[levelIndex];

        startingPointsOfCurrentSession = GenerateDefinedSession(currentLevel);

        GetComponentsSetStates();
    }

    public void StartNewLevel(Difficulty difficulty)
    {
        if (initialized)
            ResetSessionState();
        currentDifficulty = difficulty;

        // make materials invisible
        GameManager.Instance.gameElements.fadeController.HideMaterials();


        currentType = SessionType.Random;
        currentLevel = null;

        onLevelStartDelegate();

        // do difficulty initialization here
        Vector4 difficultySettings = ValuesForDifficulty(difficulty);
        startingPointsOfCurrentSession = GenerateRandomSession((int)difficultySettings.x, (int)difficultySettings.y, (int)difficultySettings.z, (int)difficultySettings.w);

        GetComponentsSetStates();

        //Debug.Log("Session with difficulty <" + difficulty + "> started.");
    }

    public void AdaptCurrentLevel(SessionState state, List<SingleSetCoordinates> levelCoords, bool isRandom = false)
    {
        // /////////////
        //onLevelResetDelegate();

        if (isRandom) // update starting points
            startingPointsOfCurrentSession = state.startingPoints;

        GameManager.Instance.playAreaManager.ResetGrid();
        // destroy all children in node parent
        foreach (Transform child in GameManager.Instance.gameElements.nodeParent.transform)
            Destroy(child.gameObject);

        NodeSwitch.selected = null;
        nodeSets.Clear();
        nodeSetsComponents.Clear();
        nodeColliders.Clear();
        colliderLines.Clear();

        Level tempLvl = ScriptableObject.CreateInstance<Level>();
        tempLvl.nodeSetsCoordinates = levelCoords;
        GenerateDefinedSession(tempLvl);

        GetComponentsSetStates();

        tracker.SetTime(state.trackerTime);
        tracker.SetMoves(state.trackerMoves);
    }

    public void ResetSessionState()
    {
        initialized = false;
        currentLevel = null;
        listOfLevels = null;
        currentLevelIndex = 0;

        solveCalled = false;
        GameManager.Instance.solvedManager.solved = false;
        solved = false;
        isSaved = false;

        LevelReset();
        tracker.HideValues();
        info.HideValues();
    }

    void GetComponentsSetStates()
    {
        nodeSetsComponents = GetNodeSetsComponents();
        colliderLines = GetColliderLines();
        nodeColliders = GetNodeColliders();

        // to fix error with uninitialized polycollider - give it time to be initialized before checking states of lines
        StartCoroutine("InitalLineCheck");

        tracker.InitAndStart();
        info.SetValues();
    }

    public void ResetCurrentLevel()
    {
        LevelReset();
        SetNodesWithStartingPoint();
    }

    void LevelReset()
    {
        onLevelResetDelegate();

        GameManager.Instance.playAreaManager.ResetGrid();
        // destroy all children in node parent
        foreach (Transform child in GameManager.Instance.gameElements.nodeParent.transform)
            Destroy(child.gameObject);

        NodeSwitch.selected = null;
        nodeSets.Clear();
        nodeSetsComponents.Clear();
        nodeColliders.Clear();
        colliderLines.Clear();
        tracker.ResetValues();
    }

    void SetNodesWithStartingPoint()
    {
        Level tempLvl = ScriptableObject.CreateInstance<Level>();
        tempLvl.nodeSetsCoordinates = startingPointsOfCurrentSession;
        GenerateDefinedSession(tempLvl);

        GetComponentsSetStates();
    }


    public List<SingleSetCoordinates> GenerateDefinedSession(Level lvl)
    {
        foreach (SingleSetCoordinates set in lvl.nodeSetsCoordinates)
        {
            GameObject newSet = Instantiate(GameManager.Instance.gameElements.nodeSetPrefab, GameManager.Instance.gameElements.nodeParent.transform);
            newSet.GetComponent<NodeSet>().GenerateSet(set);

            nodeSets.Add(newSet);
        }

        initialized = true;
        tracker.ShowValues();

        return lvl.nodeSetsCoordinates;
    }

    public LevelSet CurrentCampaign()
    {
        return listOfLevels;
    }

    public Level GetSuccessor()
    {
        if (listOfLevels.levels.Count > currentLevelIndex+1)
            return listOfLevels.levels[currentLevelIndex + 1];
        return null;
    }

    /*********************************************************************************/

    List<SingleSetCoordinates> GenerateRandomSession(int minSets = 1, int maxSets = 2, int minPerSet = 4, int maxPerSet = 8)
    {
        int setCount = Random.Range(minSets, maxSets + 1);
        List<SingleSetCoordinates> coords = new List<SingleSetCoordinates>();

        for (int i = 0; i < setCount; i++)
        {
            // random node count for set
            int count = Random.Range(minPerSet, maxPerSet + 1);

            // generate a set
            GameObject newSet = Instantiate(GameManager.Instance.gameElements.nodeSetPrefab, GameManager.Instance.gameElements.nodeParent.transform);
            List<GameObject> nodes = newSet.GetComponent<NodeSet>().GenerateSet(count);


            // save coordinates for revertion and favorites
            SingleSetCoordinates singleSet = new SingleSetCoordinates();
            singleSet.coordsForSet = new List<Vector2>();

            foreach (GameObject obj in nodes)
            {
                Vector2 tempCoords = new Vector2(obj.transform.position.x, obj.transform.position.y);
                singleSet.coordsForSet.Add(tempCoords);
            }

            coords.Add(singleSet);
            // end of saving coordinates

            // add set to list
            nodeSets.Add(newSet);
        }
        initialized = true;
        tracker.ShowValues();

        return coords;
    }


    public void CheckCrossingState()
    {
        bool crossed;
        bool tempSolved = true; // to prevent calling solved twice ?
        foreach (ColliderLine line in colliderLines)
        {
            crossed = line.UpdateCrossingState();
            //Debug.Log(crossed);
            if (crossed == true)
                tempSolved = false;
        }

        solved = tempSolved;
        // init new session
        if (solved && (tracker.GetMoves() > 0))
        {
            // check for steamachievements puzzle tactician and im just chilling
            CheckTrackerAchievements();

            Solved();
        }
        else if (solved)
            Skip();
        else
            GameManager.Instance.gameElements.fadeController.FadeInMaterials();

    }

    void CheckTrackerAchievements()
    {
        if (SteamManager.Initialized)
        {
            // check if it was a hard level / or favorite hard level and if moves were below 15 to award tactician achievement on steam
            if (currentDifficulty == Difficulty.Hard && tracker.GetMoves() <= 15)
                SteamAchievements.AwardAchievement(Achievement.PUZZLE_TACTICIAN);

            float requiredTime = tracker.GetTime();
            // check if it took over 20 minutes to solve any kind of level
            if (requiredTime >= 1200)
                SteamAchievements.AwardAchievement(Achievement.PUZZLE_CHILLING);

            // check if it took over 10 minutes to solve an easy level
            if (currentDifficulty == Difficulty.Easy && requiredTime >= 600)
                SteamAchievements.AwardAchievement(Achievement.PUZZLE_THAT_EASY);
        }
    }

    void Solved()
    {
        if (!solveCalled)
        {
            // if this was a just loaded levelthat was saved as completed - fade in materials anyway
            if (justLoaded)
            {
                GameManager.Instance.gameElements.fadeController.FadeInMaterials();
                justLoaded = false;
            }

            onSolveDelegate?.Invoke(); // not used yet
            solveCalled = true;
            tracker.StopAll();
            tracker.UpdateValues();
            statisticsHandler.AddToStatistic(currentType, currentDifficulty,  tracker.GetTime(), tracker.GetMoves());
            GameManager.Instance.solvedManager.Solved();
        }
    }

    void Skip()
    {
        tracker.StopAll();
        GameManager.Instance.solvedManager.Skip();
    }

    List<NodeSet> GetNodeSetsComponents()
    {
        List<NodeSet> retrievedComponents = new List<NodeSet>();

        foreach (GameObject obj in nodeSets)
        {
            retrievedComponents.Add(obj.GetComponent<NodeSet>());
        }

        return retrievedComponents;
    }

    List<ColliderLine> GetColliderLines()
    {
        List<ColliderLine> retrievedLines = new List<ColliderLine>();

        foreach (NodeSet set in nodeSetsComponents)
        {
            retrievedLines.AddRange(set.GetAllLinesFromSet());
        }

        return retrievedLines;
    }

    IEnumerator InitalLineCheck()
    {
        bool doWait = false;
        foreach (ColliderLine line in colliderLines)
        {
            if (line.GetComponent<PolygonCollider2D>() == null)
            {
                doWait = true;
                break;
            }

        }
        if (doWait)
            yield return new WaitForSeconds(0.001f); 
        CheckCrossingState();

        yield return null;
    }

    public void UpdateVisualsForAllItems()
    {
        foreach (ColliderLine line in colliderLines)
        {
            line.ReinitializeLineRendererColors();
            line.UpdateCrossingState();
        }

        foreach (NodeSet set in nodeSetsComponents)
        {
            List<Node> nodes = set.GetNodeComponents(set.nodes);

            foreach (Node node in nodes)
            {
                node.SetNodeVisual();
            }
        }
    }

    Vector4 ValuesForDifficulty(Difficulty diff)
    {
        Vector4 retVal = new Vector4();

        switch (diff)
        {
            case Difficulty.Easy:
                retVal.x = 1;
                retVal.y = 3;
                retVal.z = 4;
                retVal.w = 6;
                break;
            case Difficulty.Medium:
                retVal.x = 2;
                retVal.y = 4;
                retVal.z = 4;
                retVal.w = 8;
                break;
            case Difficulty.Hard:
                retVal.x = 3;
                retVal.y = 6;
                retVal.z = 4;
                retVal.w = 10;
                break;
            default: Debug.Log("Error. No difficulty matched.");
                break;
        }

        return retVal;
    }

    public void DeactivateInteraction()
    {
        List<Node> nodeComponents = new List<Node>();

        // do some nice visual on the nodes and deactivate clickablility 

        // deactivate collider on node body
        foreach (NodeSet set in nodeSetsComponents)
        {
            List<Node> tempNodes = set.GetNodeComponents(set.nodes);
            nodeComponents.AddRange(tempNodes);
        }

        foreach (Node node in nodeComponents)
        {
            node.DeactivateToSolved();
        }
    }

    public void DeactiveInteractionForNodes()
    {
        foreach (Collider2D col in nodeColliders)
        {
            col.enabled = false;
        }
    }

    public void ActiveInteractionForNodes()
    {
        if (!solved)
        {
            foreach (Collider2D col in nodeColliders)
            {
                col.enabled = true;
            }
        }
    }

    List<Collider2D> GetNodeColliders()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        List<Node> nodeComponents = new List<Node>();

        foreach (NodeSet set in nodeSetsComponents)
        {
            List<Node> tempNodes = set.GetNodeComponents(set.nodes);
            nodeComponents.AddRange(tempNodes);
        }

        foreach (Node node in nodeComponents)
        {
            colliders.Add(node.nodeCollider);
        }

        return colliders;
    }


    public void IncreaseMoveCount()
    {
        tracker.IncreaseMoveCount();
    }

    public void RevertToStart()
    {
        ResetCurrentLevel();
    }

    public SessionDetails GetSessionDetails()
    {
        SessionDetails details = new SessionDetails();

        details.nodeCount = nodeColliders.Count;
        details.setCount = nodeSets.Count;

        string modeName = "";

        if (currentType == SessionType.Campaign)
            modeName = "Campaign";
        else if (currentType == SessionType.Favorite)
            modeName = "Favorite";
        else
            modeName = currentDifficulty.ToString();

        details.mode = modeName;

        if (currentType != SessionType.Random)
        {
            details.currentCount = currentLevelIndex + 1;
            details.totalCount = listOfLevels.levels.Count;
            details.levelName = currentLevel.descName;
        } else
        {
            details.currentCount = -1;
            details.totalCount = -1;
            details.levelName = details.mode;
        }

        return details;
    }

    public List<SingleSetCoordinates> GetStartingPoints()
    {
        return startingPointsOfCurrentSession;
    }

    public Difficulty GetDifficulty()
    {
        return currentDifficulty;
    }

    public Score GetScore()
    {
        Score score = new Score();
        score.moves = tracker.GetMoves();
        score.seconds = tracker.GetTime();
        return score;
    }

    public List<SingleSetCoordinates> GetCurrentCoordinates()
    {
        List<SingleSetCoordinates> currentCoordinates = new List<SingleSetCoordinates>();

        foreach (NodeSet set in nodeSetsComponents)
        {
            SingleSetCoordinates singleSet = new SingleSetCoordinates();
            singleSet.coordsForSet = new List<Vector2>();
            foreach (GameObject node in set.nodes)
            {
                singleSet.coordsForSet.Add(new Vector2(node.transform.position.x, node.transform.position.y));
            }
            currentCoordinates.Add(singleSet);
        }

        return currentCoordinates;
    }

    void HandelSwap()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.tag != "Node")
            {
                // Debug.Log("i didnt hit a node");
            }
        }
        else
        {
            NodeSwitch.selected.GetComponent<Node>().DeselectNode();
        }
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public int GetCurrentLevelSetIndex()
    {
        int index = 0;
        if (currentType == SessionType.Campaign)
        {
            List<LevelSet> sets = GameManager.Instance.campaignManager.GetCampaigns();
            foreach (LevelSet set in sets)
            {
                // if levels in lists are the same then you found the corresponding levelset list
                if (listOfLevels.levels[0] == set.levels[0])
                    return index;
                index++;
            }
        } else if (currentType == SessionType.Favorite)
        {
            if (currentDifficulty == Difficulty.Easy)
                index = 0;
            else if (currentDifficulty == Difficulty.Medium)
                index = 1;
            else if (currentDifficulty == Difficulty.Hard)
                index = 2;
            else
                index = -1;
            return index;
        }

        return -1;
    }

    public bool OnCampaign()
    {
        return currentType == SessionType.Campaign;
    }

    public bool Favorite()
    {
        return currentType == SessionType.Favorite;
    }

}

public class NodeSwitch
{
    public static GameObject selected;
}

public class Score
{
    public float seconds;
    public int moves;
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    None
}

public enum SessionType
{
    None,
    Random,
    Campaign,
    Favorite
}

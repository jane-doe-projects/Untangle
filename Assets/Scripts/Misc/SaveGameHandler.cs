using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveGameHandler : MonoBehaviour
{
    [SerializeField] Toggle saveOnExitToggle;
    [SerializeField] MenuButton continueWithSaved;

    public static bool wasSaved;
    string savePrefString = "SavedLevelFavorited";

    // Start is called before the first frame update
    void Start()
    {
        SetState();
    }

    
    void SaveState()
    {
        GameManager.Instance.prefs.SaveSaveOnExitState(saveOnExitToggle.isOn);
    }

    public void SetState()
    {
        // set state for saved level favorization
        int favorisation = PlayerPrefs.GetInt(savePrefString, 0);

        wasSaved = Convert.ToBoolean(favorisation);
        ////

        saveOnExitToggle.isOn = GameManager.Instance.prefs.LoadSaveOnExitState();


        if (saveOnExitToggle.isOn && (GameManager.Instance.serManager.LoadSavedSessionState() != null) ) // only activate button if the toggle is on and there is a saved level to load from
            continueWithSaved.gameObject.SetActive(true);
        else
            continueWithSaved.gameObject.SetActive(false);

    }

    public void SaveCurrentLevelState()
    {
        if (!GameManager.Instance.currentSession.initialized)
            return;

        // get current level and its current state
        SessionState state = GetSessionState();

        // save both to a dedicated savegamefile
        GameManager.Instance.serManager.SaveCurrentSessionState(state);


        // note if saved level was already favorited so the button can be deactivated on load
        if (GameManager.Instance.currentSession.currentType == SessionType.Random)
        {
            if (GameManager.Instance.currentSession.isSaved)
                PlayerPrefs.SetInt(savePrefString, 1);
            else
                PlayerPrefs.SetInt(savePrefString, 0);
        } else
            PlayerPrefs.SetInt(savePrefString, 0);


        PlayerPrefs.Save();

    }

    SessionState GetSessionState()
    {
        SessionState state = new SessionState();
        Session session = GameManager.Instance.currentSession;

        state.trackerTime = session.tracker.GetTime();
        state.trackerMoves = session.tracker.GetMoves();
        state.solved = session.solved;
        state.isSaved = session.isSaved;

        state.currentDifficulty = session.GetDifficulty();
        state.stateCoordinates = session.GetCurrentCoordinates();
        state.startingPoints = session.GetStartingPoints();

        state.currentLevelIndex = session.GetCurrentLevelIndex();
        state.currentType = session.currentType;
        state.levelSetIndex = session.GetCurrentLevelSetIndex();

        return state;
    }

    public void SetSessionState()
    {
        continueWithSaved.gameObject.SetActive(false);

        // load the saved session etc
        SessionState loadedState = GameManager.Instance.serManager.LoadSavedSessionState();

        if (loadedState == null)
        {
            Debug.Log("Error, no session to load from.");
            return;
        }

        LevelSet set = null;
        if (loadedState.currentType == SessionType.Campaign)
        {
            set = GameManager.Instance.campaignManager.GetCampaigns()[loadedState.levelSetIndex];
            Debug.Log("Loading a campaign level.");
        } else if (loadedState.currentType == SessionType.Favorite)
        {
            set = GameManager.Instance.favoritesManager.GetCollectionForDifficulty(loadedState.currentDifficulty);
            Debug.Log("Loading a favorites level.");
        } else
        {
            Debug.Log("Loading random set.");
        }

        // initialize saved positions and tracker information

        if (set != null)
        {
            GameManager.Instance.currentSession.StartNewLevel(loadedState.currentType, set, loadedState.currentLevelIndex);
            GameManager.Instance.currentSession.AdaptCurrentLevel(loadedState, loadedState.stateCoordinates);
        }
        else
        {
            GameManager.Instance.currentSession.StartNewLevel(loadedState.currentDifficulty);
            GameManager.Instance.currentSession.AdaptCurrentLevel(loadedState, loadedState.stateCoordinates, true);
        }

        // deactivate favorite button if needed
        if (GameManager.Instance.currentSession.currentType == SessionType.Random && wasSaved)
            GameManager.Instance.solvedManager.DeactivateFavButton();

        Session.justLoaded = true;
    }

    private void OnApplicationQuit()
    {

        SaveState();

        if (saveOnExitToggle.isOn)
        {
            SaveCurrentLevelState();
        }
    }

}

public class SessionState {

    public float trackerTime;
    public int trackerMoves;

    //public SessionInfo info;
    //public List<GameObject> nodeSets;
    public bool solved = false;
    public bool isSaved = false;
    public List<SingleSetCoordinates> startingPoints;
    public Difficulty currentDifficulty;
    public List<SingleSetCoordinates> stateCoordinates;

    /******************************************/

    //LevelSet listOfLevels;
    public int levelSetIndex;
    public int currentLevelIndex;
    public SessionType currentType;

    /******************************************/
}

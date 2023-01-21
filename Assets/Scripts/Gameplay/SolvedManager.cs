using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SolvedManager : MonoBehaviour
{
    [SerializeField] bool continueAfterPause;
    [SerializeField] float pauseForSeconds = 2f;
    [SerializeField] GameObject continueButton;

    [SerializeField] GameObject revertButton;
    [SerializeField] GameObject favoriteButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject pauseOverlay;

    public static bool wasAutoLoaded = false;

    public bool solved = false;

    // delayed score saving variables 
    public bool saveScoreOnCompletion = false;
    long favId = 0;

    private void Start()
    {
        continueButton.SetActive(false);
        Session.onLevelResetDelegate += HideContinueButton;
        Session.onLevelStartDelegate += ActivateUtility;
        Session.onLevelResetDelegate += Unpause;
        Session.onLevelStartDelegate += Unpause;
    }

    public void Skip()
    {
        Debug.Log("Was solved instantly");
        NextSession(withSound: false);
    }

    public void Solved()
    {
        GameManager.Instance.soundControl.PlaySolved();
        solved = true;

        // unlock following level if its a campaign - and save it to the disk
        if (GameManager.Instance.currentSession.currentType == SessionType.Campaign)
        {
            Level successor = GameManager.Instance.currentSession.GetSuccessor();
            if (successor != null)
            {
                successor.Unlock();
                LevelSet campaign = GameManager.Instance.currentSession.CurrentCampaign();
                int levelIndex = LevelItemPreview.GetIndexInSet(successor, campaign);

                // check if lockstate should be overridden
                int lockIndex = GameManager.Instance.serManager.GetLockIndexForCampaign(campaign);
                if (lockIndex < levelIndex)
                    GameManager.Instance.serManager.SetLockIndexForCampaign(campaign, levelIndex);
            }
        }
        ///////
        ///
        Session currentSession = GameManager.Instance.currentSession;

        if (currentSession.currentType == SessionType.Random)
        {
            if (saveScoreOnCompletion)
                SaveScoreOnCompletion(); // for favorites
        }
        else
        {
            saveScoreOnCompletion = false;
            if (currentSession.currentType == SessionType.Campaign)
                GameManager.Instance.serManager.SaveCampaignScore(currentSession.tracker.GetMoves(), currentSession.tracker.GetTime(), currentSession.currentLevel.cId, currentSession.currentLevel.id);
            else if (currentSession.currentType == SessionType.Favorite)
                GameManager.Instance.serManager.SaveFavoriteScore(currentSession.tracker.GetMoves(), currentSession.tracker.GetTime(), currentSession.currentLevel.fId);
        }

        StartCoroutine("SolvedRoutine");

        // track steam stats
        StatsTracker.IncreaseSolved();
        if (currentSession.currentType == SessionType.Random)
            StatsTracker.IncreaseForDifficulty(currentSession.GetDifficulty());

        // make orbs unclickable
        GameManager.Instance.currentSession.DeactivateInteraction();
        revertButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    IEnumerator SolvedRoutine()
    {
        if (continueAfterPause)
        {
            wasAutoLoaded = true;
            yield return new WaitForSeconds(pauseForSeconds);

            Continue();
        } else
        {
            continueButton.SetActive(true);
        }
    }

    public void Continue()
    {
        NextSession();
        continueButton.SetActive(false);
        solved = false;
    }

    public void NextSession(bool withSound = true)
    {
        bool shouldPause = wasAutoLoaded && IsPaused();

        solved = false;
        /* if (withSound)
            GameManager.Instance.soundControl.PlayNew(); */
        GameManager.Instance.currentSession.NextLevel(withSound, shouldPause);
    }

    public void RevertToStart()
    {
        GameManager.Instance.currentSession.RevertToStart();
        GameManager.Instance.popoutMessage.DisplayMessage("Reverted to start");

        // steam stats tracker
        StatsTracker.IncreaseReset();
    }

    void SaveScoreOnCompletion()
    {
        Debug.Log("Doing delayed saving of score for favorite");
        Session currentSession = GameManager.Instance.currentSession;
        saveScoreOnCompletion = false;

        GameManager.Instance.serManager.SaveFavoriteScore(currentSession.tracker.GetMoves(), currentSession.tracker.GetTime(), favId);
        favId = 0;
    }

    public void SaveFavorite()
    {
        // open msk for name input? TODO
        Session currentSession = GameManager.Instance.currentSession;

        List<SingleSetCoordinates> coords =  currentSession.GetStartingPoints();
        Difficulty diff = currentSession.GetDifficulty();

        //string format = "ddMMyyyyHHmmss";
        string format = "yyyyMMddHHmmss";
        long id = Int64.Parse(System.DateTime.Now.ToString(format));
        string tempDesc = id.ToString();

        Score score = currentSession.GetScore();

        GameManager.Instance.serManager.SaveRandomToFavorites(coords, diff, tempDesc, id, score);

        // save its current (best) score too
        if (solved)
            GameManager.Instance.serManager.SaveFavoriteScore(currentSession.tracker.GetMoves(), currentSession.tracker.GetTime(), id);
        else
        {
            saveScoreOnCompletion = true;
            favId = id;
        }

        // saved! pop up - deactive the button after
        favoriteButton.SetActive(false);
        GameManager.Instance.currentSession.isSaved = true;

        GameManager.Instance.popoutMessage.DisplayMessage("Level saved to Favorties");


        // track steam stats
        StatsTracker.IncreaseSaved();
    }

    void ActivateUtility()
    {
        revertButton.SetActive(true);
        pauseButton.SetActive(true);
        if (!GameManager.Instance.currentSession.OnCampaign() && !GameManager.Instance.currentSession.Favorite() && !GameManager.Instance.currentSession.isSaved)
            favoriteButton.SetActive(true);
        else
            favoriteButton.SetActive(false);    
    }

    void HideContinueButton()
    {
        continueButton.SetActive(false);
    }

    public void PauseGame()
    {
        // toggle pause

        if (!pauseOverlay.activeSelf)
        {
            // stop timer
            GameManager.Instance.currentSession.tracker.StopTimer();
            GameManager.Instance.currentSession.DeactiveInteractionForNodes();

            // activate overlay
            pauseOverlay.SetActive(true);

            pauseButton.GetComponent<MenuButton>().SetCustomSprite(CustomSprite.Play);
        } else
        {
            GameManager.Instance.currentSession.tracker.StartTimer();
            GameManager.Instance.currentSession.ActiveInteractionForNodes();
            pauseOverlay.SetActive(false);
            pauseButton.GetComponent<MenuButton>().SetCustomSprite(CustomSprite.Pause);
        }
       
    }

    public void Unpause()
    {
        pauseOverlay.SetActive(false);
        GameManager.Instance.currentSession.tracker.StartTimer();
        GameManager.Instance.currentSession.ActiveInteractionForNodes();
        pauseButton.GetComponent<MenuButton>().SetCustomSprite(CustomSprite.Pause);
    }

    public bool IsPaused()
    {
        return pauseOverlay.activeSelf;
    }

    public void AutomaticNext(bool autoNext)
    {
        continueAfterPause = autoNext;
    }

    public void DeactivateFavButton()
    {
        // make sure button is disabled and favorite saving state is set correctly
        favoriteButton.SetActive(false);
        GameManager.Instance.currentSession.isSaved = true;
    }
}

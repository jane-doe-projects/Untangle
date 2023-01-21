using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tracker : MonoBehaviour
{
    int moveCount = 0;
    float timer;
    bool running = false;


    [SerializeField] ScoreItem time;
    [SerializeField] ScoreItem moves;

    TextMeshProUGUI timerText;
    TextMeshProUGUI moveCountText;

    int minutes;
    int seconds;


    bool scoreSet = false; // initial score exists
    float bestTime = Mathf.Infinity;
    int bestMove = 999999999;

    string timeDisplayFormat = "0";

    private void Start()
    {
        timerText = time.current;
        moveCountText = moves.current;
    }

    private void Update()
    {
        if (running)
        {
            UpdateTimer();
        }
    }

    public void HideValues()
    {
        time.gameObject.SetActive(false);
        moves.gameObject.SetActive(false);
    }

    public void ShowValues()
    {
        time.gameObject.SetActive(true);
        moves.gameObject.SetActive(true);
        ResetValues();
        SetBestValues();
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        timerText.text = timer.ToString(timeDisplayFormat);
    }


    public void ResetValues()
    {
        moveCount = 0;
        timer = 0;

        bestMove = 999999999;
        bestTime = Mathf.Infinity;

        timerText.text = timer.ToString(timeDisplayFormat);
        moveCountText.text = moveCount.ToString();

    }

    public void IncreaseMoveCount()
    {
        if (running)
        {
            moveCount++;
            moveCountText.text = moveCount.ToString();
        }
    }

    public void StartTimer()
    {
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }

    public void InitAndStart()
    {
        ResetValues();
        SetBestValues();
        StartTimer();
    }

    void SetBestValues()
    {
        // fetch best values for current level
        Session currentSession = GameManager.Instance.currentSession;

        Level level = GameManager.Instance.currentSession.currentLevel;
        if (level != null)
        {
            if (currentSession.currentType == SessionType.Campaign)
            {
                CampaignSaveData score = level.GetBestScore();
                SetScoreValues(score);
            } else if (currentSession.currentType == SessionType.Favorite)
            {
                FavoriteSaveData score = level.GetBestScoreFavorite();
                SetScoreValues(score);
            }

            if (scoreSet)
                ShowBestScores();
            else
                HideBestScores();

        } else
            HideBestScores();

    }

    void HideBestScores()
    {
        time.best.enabled = false;
        moves.best.enabled = false;
    }

    void ShowBestScores()
    {
        time.best.enabled = true;
        time.best.text = bestTime.ToString(timeDisplayFormat);
        moves.best.enabled = true;
        moves.best.text = bestMove.ToString();
    }

    void SetScoreValues(CampaignSaveData score)
    {
        if (score != null)
        {
            scoreSet = true;
            bestTime = score.bestTime;
            bestMove = score.bestMove;
        }
        else
        {
            // no saved scores yet - set these for now
            scoreSet = false;
            bestTime = Mathf.Infinity;
            bestMove = 999999999;
        }
    }

    void SetScoreValues(FavoriteSaveData score)
    {
        if (score != null)
        {
            scoreSet = true;
            bestTime = score.bestTime;
            bestMove = score.bestMove;
        }
        else
        {
            // no saved scores yet - set these for now
            scoreSet = false;
            bestTime = Mathf.Infinity;
            bestMove = 999999999;
        }
    }

    public void UpdateValues()
    {
        Level level = GameManager.Instance.currentSession.currentLevel;
        if (level != null)
        {
            // check if values are better than best - if so - save them
            if (timer < bestTime)
            {
                level.UpdateBestTime(timer);
                if (scoreSet)
                    time.Notify();
            }
            if (moveCount < bestMove)
            {
                level.UpdateBestMoves(moveCount);
                if (scoreSet)
                    moves.Notify();
            }
                
        }
    }

    public int GetMoves()
    {
        return moveCount;
    }

    public float GetTime()
    {
        return timer;
    }

    public void SetTime(float val)
    {
        timer = val;
    }

    public void SetMoves(int val)
    {
        moveCount = val;
        moveCountText.text = moveCount.ToString();
    }

    public void StopAll()
    {
        StopTimer();
        // stops the move count increase also - due to boolean check "running = false"
    }

}

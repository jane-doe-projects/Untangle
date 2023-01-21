using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentButton : MonoBehaviour
{
    [SerializeField] ButtonActionType type;
    [SerializeField] Difficulty difficulty;

    [SerializeField] string label;
    [SerializeField] TextMeshProUGUI uiLabel;

    Button btn;
    Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.material = GameManager.Instance.gameElements.buttonBlurMaterial;

        btn = GetComponent<Button>();

        SetButtonAction();

    }

    void SetButtonAction()
    {
        switch (type)
        {
            case ButtonActionType.Difficulty:
                btn.onClick.AddListener(InitSessionWithDifficulty);
                SetDifficultyLabel();
                break;
            case ButtonActionType.Campaign:
                btn.onClick.AddListener(DisplayCampaign);
                uiLabel.text = "Campaign";
                break;
            case ButtonActionType.Favorites:
                btn.onClick.AddListener(DisplayFavorites);
                uiLabel.text = "Favorites";
                break;
            case ButtonActionType.ExitYes:
                btn.onClick.AddListener(QuitGame);
                uiLabel.text = "Yes";
                break;
            case ButtonActionType.ExitNo:
                btn.onClick.AddListener(CancelLeaveGame);
                uiLabel.text = "No";
                break;
            case ButtonActionType.ScreenYes:
                btn.onClick.AddListener(SaveScreenSettings);
                uiLabel.text = "Keep changes";
                break;
            case ButtonActionType.ScreenNo:
                btn.onClick.AddListener(RevertScreenSettings);
                uiLabel.text = "Revert changes";
                break;
            case ButtonActionType.Deactivated:
                DisabledButtonVisual();
                break;
            default:
                uiLabel.text = label;
                btn.interactable = false;
                break;
        } 
    }

    void DisabledButtonVisual()
    {
        uiLabel.text = label;
        Color labelColor = Color.white;
        labelColor.a = 0.5f;
        uiLabel.color = labelColor;
        ButtonSounds sounds = GetComponent<ButtonSounds>();
        sounds.enabled = false;
        btn.interactable = false;
    }

    void SaveScreenSettings()
    {
        GameManager.Instance.resHandler.SetCurrentAsLast();
    }

    void RevertScreenSettings()
    {
        GameManager.Instance.resHandler.ReturnToLast();
    }

    void InitSessionWithDifficulty()
    {
        GameManager.Instance.soundControl.PlayNew();
        GameManager.Instance.currentSession.StartNewLevel(difficulty);
    }

    void DisplayCampaign()
    {
        GameManager.Instance.campaignManager.DisplayCampaings();
    }

    void DisplayFavorites()
    {
        GameManager.Instance.favoritesManager.DisplayFavorites();
    }

    void SetDifficultyLabel()
    {
        uiLabel.text = difficulty.ToString();
    }

    void QuitGame()
    {
        GameManager.Instance.ExitGame();
    }

    void CancelLeaveGame()
    {
        GameManager.Instance.windowControl.CloseAll();
        if (!GameManager.Instance.currentSession.initialized)
            GameManager.Instance.windowControl.ShowStart();
    }

}

public enum ButtonActionType
{
    Deactivated,
    Difficulty,
    Favorites,
    Campaign,
    Settings,
    Main,
    Credits,
    ExitYes,
    ExitNo,
    Zen,
    Continue,
    Save,
    Revert,
    ScreenYes,
    ScreenNo
}

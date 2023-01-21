using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void OnMaterialsInitialized();
    public static event OnMaterialsInitialized onMaterialsInitializedDelegate;

    public PlayerSettings prefs;
    public ResolutionHandler resHandler;
    public SoundControl soundControl;
    public ColorControl colorControl;

    public SolvedManager solvedManager;
    public EffectsManager effectsManager;
    public WindowControl windowControl;

    public GameElementSettings gameElements;
    public LanguageControl langControl;

    public bool paused;
    public bool started;
    public Session currentSession;
    public SessionInfo sessionInfo;
    public Difficulty currentDifficulty;

    public PlayAreaManager playAreaManager;

    public CampaignManager campaignManager;
    public FavoritesManager favoritesManager;

    public PopoutMessage popoutMessage;

    public float swapSpeed;

    [Header("Themes")]
    public Theme currentTheme;
    public Theme defaultTheme;

    [Header("Skin Handling")]
    public ColorPickerHandler colorPickerHandler;
    public SkinElementSelection skinElementSelection;

    [Header("Level Maker")]
    public LevelMaker levelMaker;

    [Header("Serialization")]
    public SerializationManager serManager;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            return;
        }

        Destroy(this);
    }

    private void Start()
    {
        LoadPlayerPreferences();
        LoadTheme();
    }

    public void LoadTheme()
    {
        DelegateHandler.onSkinLoadDelegate?.Invoke();

        // change background
        gameElements.InitBackgroundMaterial();
        gameElements.SetBackground(gameElements.backgroundMaterialRuntime);
        DelegateHandler.onBackgroundChangeDelegate();

        //change items in current session including nodes and lines
        if (currentSession.initialized)
            currentSession.UpdateVisualsForAllItems();

        // change sound effects and background music
        soundControl.SetThemeAudio();
        soundControl.PlayBackground();

        // change tone of menu (future)

        // change material colors
        gameElements.InitLineMaterials();
        gameElements.uncrossedLineMaterialRuntime.SetColor("_StartColor", currentTheme.lineColorStart);
        gameElements.uncrossedLineMaterialRuntime.SetColor("_EndColor", currentTheme.lineColorEnd);

        gameElements.crossedLineMaterialRuntime.SetColor("_StartColor", currentTheme.lineColorStartCrossed);
        gameElements.crossedLineMaterialRuntime.SetColor("_EndColor", currentTheme.lineColorEndCrossed);

        gameElements.InitNodeMaterial();
        foreach (Material mat in gameElements.nodeRuntimeMaterials)
        {
            mat.SetColor("_MainColor", currentTheme.nodeColor1);
            mat.SetColor("_AltColor", currentTheme.nodeColor2);
        }

        // reload lines
        DelegateHandler.onSkinLoadDelegate?.Invoke();
        onMaterialsInitializedDelegate?.Invoke();

        gameElements.fadeController.FadeInMaterials();
    }

    void LoadPlayerPreferences()
    {
        soundControl.LoadSoundPrefs();
        colorControl.LoadColorPrefs();

        prefs.LoadAll();
    }

    public void ExitGame()
    {
        // exit application
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

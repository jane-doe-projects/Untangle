using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoritesManager : MonoBehaviour
{
    FavoritesCollection collection;

    [SerializeField] DisplayWindow favoritesDisplayWindow;
    WindowControl windowControl;

    private void Start()
    {
        windowControl = GameManager.Instance.windowControl;
        SerializationManager.onSaveFavoriteDelegate += RefreshFavorites;

        ReadInWordLists();
    }

    public void DisplayFavorites()
    {

        RefreshFavorites();
        // instantiate group selection buttons
        InitGroups();

        // show display
        windowControl.ShowFavorites();
    }

    void InitGroups()
    {
        favoritesDisplayWindow.ClearGroupDisplay();
        favoritesDisplayWindow.AddGroupItem("easy", collection.easy, favoritesDisplayWindow, Difficulty.Easy, isEditable: true);
        favoritesDisplayWindow.AddGroupItem("medium", collection.medium, favoritesDisplayWindow, Difficulty.Medium, isEditable: true);
        favoritesDisplayWindow.AddGroupItem("hard", collection.hard, favoritesDisplayWindow, Difficulty.Hard, isEditable: true);
    }

    void InitSuccessors(LevelSet set)
    {
        for (int i = 0; i < set.levels.Count - 1; i++)
        {
            set.levels[i].SetSuccessor(set.levels[i + 1]);
        }
    }

    void RefreshFavorites()
    {
        collection = GameManager.Instance.serManager.LoadFavorites();

        InitSuccessors(collection.easy);
        InitSuccessors(collection.medium);
        InitSuccessors(collection.hard);
    }

    void ReadInWordLists()
    {
        List<WordList> lists = GameManager.Instance.gameElements.wordLists;
        foreach(WordList list in lists)
            list.ReadInWords();
    }

    public LevelSet GetCollectionForDifficulty(Difficulty diff)
    {
        RefreshFavorites();

        if (diff == Difficulty.Easy)
            return collection.easy;
        else if (diff == Difficulty.Medium)
            return collection.medium;
        else if (diff == Difficulty.Hard)
            return collection.hard;
        return null;
    }

    public int GetLevelCountForDifficulty(Difficulty difficulty)
    {
        if (collection == null)
            RefreshFavorites();
        switch (difficulty)
        {
            case Difficulty.Easy:
                return collection.easy.levels.Count;
            case Difficulty.Medium:
                return collection.medium.levels.Count;
            case Difficulty.Hard:
                return collection.hard.levels.Count;
        }

        return 0;
    }
}

public class FavoritesDisplay
{
    public Level level;
    public LevelItemPreview preview;
}

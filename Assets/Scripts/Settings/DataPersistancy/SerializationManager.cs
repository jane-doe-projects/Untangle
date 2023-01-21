using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager : MonoBehaviour
{
    public delegate void OnSaveFavorite();
    public static OnSaveFavorite onSaveFavoriteDelegate;

    string savePathDirectory = "/SaveData/";
    string favoritesDirectory = "Favorites";
    string campaignDirectory = "Campaigns";

    string easyDiff = "/EASY/";
    string mediumDiff = "/MEDIUM/";
    string hardDiff = "/HARD/";

    string fullFavoritesDirectoryPath = "";
    string fullCampaignDirectoryPath = "";

    string saveGamePath = "";

    private void Awake()
    {
        saveGamePath = Application.persistentDataPath + "/savegame";
    }

    private void Start()
    {
        fullFavoritesDirectoryPath = Application.persistentDataPath + savePathDirectory + favoritesDirectory;
        fullCampaignDirectoryPath = Application.persistentDataPath + savePathDirectory + campaignDirectory;
        CheckPaths();
    }

    public void SaveCampaignScore(int moves, float time, int cId, int lvlId)
    {
        string saveFileName = cId.ToString() + "_" + lvlId.ToString();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + savePathDirectory + saveFileName);

        CampaignSaveData data = new CampaignSaveData();
        data.bestMove = moves;
        data.bestTime = time;
        data.campaignID = cId;
        data.levelID = lvlId;

        bf.Serialize(fs, data);
        fs.Close();
        //Debug.Log("Data saved.");
    }

    public CampaignSaveData LoadCampaignScore(int cId, int lvlId)
    {
        CampaignSaveData data;
        string saveFileName = cId.ToString() + "_" + lvlId.ToString();
        string filePath = Application.persistentDataPath + savePathDirectory + saveFileName;
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(filePath, FileMode.Open);
            data = (CampaignSaveData)bf.Deserialize(fs);
            fs.Close();
        } else
        {
            data = null;
            //Debug.Log("No data to load.");
        }
        return data;
    }

    public void SaveFavoriteScore(int moves, float time, long fId)
    {
        // TODO check if favorite still exists - if it was deleted while playing it - dont save score so we dont pile up score files that have no associated level
        string saveFileName = fId.ToString() + "_s";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(fullFavoritesDirectoryPath + "/" + saveFileName);

        FavoriteSaveData data = new FavoriteSaveData();
        data.bestMove = moves;
        data.bestTime = time;
        data.favoriteID = fId;

        bf.Serialize(fs, data);
        fs.Close();
        //Debug.Log("Data (fav score) saved.");
    }

    public FavoriteSaveData LoadFavoriteScore(long fId)
    {
        FavoriteSaveData data;
        string saveFileName = fId.ToString() + "_s";
        string filePath = fullFavoritesDirectoryPath + "/" + saveFileName;
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(filePath, FileMode.Open);
            data = (FavoriteSaveData)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            data = null;
            //Debug.Log("No data to load.");
        }

        return data;
    }

    public void SaveRandomToFavorites(List<SingleSetCoordinates> coords, Difficulty diff, string desc, long favId, Score score)
    {
        // name of file
        string fileName = favId.ToString();

        // save location for different difficulties
        string difficultyPath = "";
        switch (diff)
        {
            case Difficulty.Easy:
                difficultyPath = easyDiff;
                break;
            case Difficulty.Medium:
                difficultyPath = mediumDiff;
                break;
            case Difficulty.Hard:
                difficultyPath = hardDiff;
                break;
            default:
                difficultyPath = "";
                break;
        }

        // final path
        string path = Application.persistentDataPath + savePathDirectory + favoritesDirectory + difficultyPath;

        /*
        Debug.Log(difficultyPath);
        Debug.Log(diff.ToString());
        Debug.Log(path);
        Debug.Log(fileName); */

        // set up the stuff and save it 
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(path + fileName);


        // fill 
        FavoriteLevelSaveData data = new FavoriteLevelSaveData();

        // todo should also save best score right away
        data.bestMove = score.moves;
        data.bestTime = score.seconds;

        //data.description = desc;
        data.description = WordList.GetRandomWord();
        data.difficulty = diff;
        data.favoriteID = favId;

        List<SerializableSingleSetCoordinates> convertedCoords = ConvertSetsToSerializeable(coords);
        data.nodeSets = convertedCoords;

        bf.Serialize(fs, data);
        fs.Close();
        //Debug.Log("Data (Fav) saved.");

        // add on save delegate - so the levels all get reloaded into the game for visualisation
        onSaveFavoriteDelegate();
    }

    public FavoritesCollection LoadFavorites()
    {
        FavoritesCollection collection = new FavoritesCollection();
        collection.easy = LoadAllFavsForDifficulty(easyDiff);
        collection.medium = LoadAllFavsForDifficulty(mediumDiff);
        collection.hard = LoadAllFavsForDifficulty(hardDiff);

        return collection;
    }

    Level LoadSingleFavorite(string path)
    {
        FavoriteLevelSaveData fav;

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path, FileMode.Open);
            fav = (FavoriteLevelSaveData)bf.Deserialize(fs);
            fs.Close();
        } else
        {
            Debug.Log("Error loading file: " + path);
            fav = null;
        }

        // convert level to savedfavorite
        Level convertedFav = ConvertToLevel(fav);

        return convertedFav;
    }

    public void RemoveSingleFavorite(Level lvl, Difficulty diff)
    {
        // remove favorite file and its score file


        // name of file
        string fileName = lvl.fId.ToString();

        // save location for different difficulties
        string difficultyPath = "";
        switch (diff)
        {
            case Difficulty.Easy:
                difficultyPath = easyDiff;
                break;
            case Difficulty.Medium:
                difficultyPath = mediumDiff;
                break;
            case Difficulty.Hard:
                difficultyPath = hardDiff;
                break;
            default:
                difficultyPath = "";
                break;
        }

        // final path
        string path = Application.persistentDataPath + savePathDirectory + favoritesDirectory + difficultyPath + fileName;
        string scoreFileName = lvl.fId.ToString() + "_s";
        string scorePath = fullFavoritesDirectoryPath + "/" + scoreFileName;

        //Debug.Log("Would delete: " + path);
        //Debug.Log("and " + scorePath);

        if (File.Exists(path))
            File.Delete(path);

        if (File.Exists(scorePath))
            File.Delete(scorePath);

        //Debug.Log("Data (Fav) deleted.");

        // add on save delegate - so the levels all get reloaded into the game for visualisation

    }

    LevelSet LoadAllFavsForDifficulty(string diffPath)
    {
        string fullDirPath = fullFavoritesDirectoryPath + diffPath;
        DirectoryInfo dirInfo = new DirectoryInfo(fullDirPath);

        FileInfo[] fileInfo = dirInfo.GetFiles();

        LevelSet collection = ScriptableObject.CreateInstance<LevelSet>();
        collection.levels = new List<Level>();

        // foreach file in path load file and add to list as favorite
        //Debug.Log("Listing if any available:");
        foreach (FileInfo file in fileInfo)
        {
            //Debug.Log(file.ToString()); // full path
            Level fav = LoadSingleFavorite(file.ToString());
            collection.levels.Add(fav);
        }

        return collection;
    }

    List<SerializableSingleSetCoordinates> ConvertSetsToSerializeable(List<SingleSetCoordinates> sets)
    {
        List<SerializableSingleSetCoordinates> converted = new List<SerializableSingleSetCoordinates>();

        foreach (SingleSetCoordinates set in sets)
        {
            SerializableSingleSetCoordinates serSet = ConvertCoordinatesToSerializable(set);
            converted.Add(serSet);
        }

        return converted;
    }


    List<SingleSetCoordinates> ConvertSetsToPlayable(List<SerializableSingleSetCoordinates> sets)
    {
        List<SingleSetCoordinates> converted = new List<SingleSetCoordinates>();

        foreach (SerializableSingleSetCoordinates coord in sets)
        {
            SingleSetCoordinates singleCoords = ConvertCoordinatesToPlayable(coord);
            converted.Add(singleCoords);
        }

        return converted;
    }

    SerializableSingleSetCoordinates ConvertCoordinatesToSerializable(SingleSetCoordinates set)
    {
        SerializableSingleSetCoordinates converted = new SerializableSingleSetCoordinates();
        converted.coordsForSet = new List<SerializableVector2>();

        foreach (Vector2 vec in set.coordsForSet)
        {
            SerializableVector2 serVec = new SerializableVector2();
            serVec.x = vec.x;
            serVec.y = vec.y;

            converted.coordsForSet.Add(serVec);
        }

        return converted;
    }

    SingleSetCoordinates ConvertCoordinatesToPlayable(SerializableSingleSetCoordinates set)
    {
        SingleSetCoordinates converted = new SingleSetCoordinates();
        converted.coordsForSet = new List<Vector2>();

        foreach (SerializableVector2 serVec in set.coordsForSet)
        {
            Vector2 vec = new Vector2();
            vec.x = serVec.x;
            vec.y = serVec.y;

            converted.coordsForSet.Add(vec);
        }

        return converted;
    }

    Level ConvertToLevel(FavoriteLevelSaveData data)
    {
        Level fav = ScriptableObject.CreateInstance<Level>();

        fav.type = LevelType.Favorite;
        fav.descName = data.description;
        fav.fId = data.favoriteID;
        fav.nodeSetsCoordinates = ConvertSetsToPlayable(data.nodeSets);

        fav.bestTime = data.bestTime;
        fav.fewestClicks = data.bestMove;

        return fav;
    }

    void CheckPaths()
    {
        // parent directory
        if (!Directory.Exists(Application.persistentDataPath + savePathDirectory))
        {
            // create the directory
            Directory.CreateDirectory(Application.persistentDataPath + savePathDirectory);
        }

        // campaign directory
        if (!Directory.Exists(fullCampaignDirectoryPath))
        {
            Directory.CreateDirectory(fullCampaignDirectoryPath);
        }

        // favorites directory
        if (!Directory.Exists(fullFavoritesDirectoryPath))
        {
            Directory.CreateDirectory(fullFavoritesDirectoryPath);
        }

        // favorites subdirectories

        if (!Directory.Exists(fullFavoritesDirectoryPath + easyDiff))
        {
            Directory.CreateDirectory(fullFavoritesDirectoryPath + easyDiff);
        }

        if (!Directory.Exists(fullFavoritesDirectoryPath + mediumDiff))
        {
            Directory.CreateDirectory(fullFavoritesDirectoryPath + mediumDiff);
        }

        if (!Directory.Exists(fullFavoritesDirectoryPath + hardDiff))
        {
            Directory.CreateDirectory(fullFavoritesDirectoryPath + hardDiff);
        }
    }


    public int GetLockIndexForCampaign(LevelSet campaign)
    {
        string path = fullCampaignDirectoryPath + "/" + campaign.campaignId;
        if (File.Exists(path))
        {
            // get info / read file
            CampaignLockSaveData lockData;

            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(path, FileMode.Open);
                lockData = (CampaignLockSaveData)bf.Deserialize(fs);
                fs.Close();
            }
            else
            {
                Debug.Log("Error loading file: " + path);
                lockData = null;
            }

            if (lockData != null)
                return lockData.lockIndex;

        }

        // none are unlocked / non found
        return 0;
    }

    public void SetLockIndexForCampaign(LevelSet campaign, int lockIndex)
    {
        string saveFileName = campaign.campaignId.ToString();

        // write file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(fullCampaignDirectoryPath + "/" + saveFileName);

        CampaignLockSaveData data = new CampaignLockSaveData();
        data.campaignId = campaign.campaignId;
        data.lockIndex = lockIndex;

        bf.Serialize(fs, data);
        fs.Close();
    }

    public void SaveCurrentSessionState(SessionState state)
    {
        SessionSaveData data = new SessionSaveData();

        data.trackerTime = state.trackerTime;
        data.trackerMoves = state.trackerMoves;
        data.isSaved = state.isSaved;
        data.solved = state.solved;
        data.currentLevelIndex = state.currentLevelIndex;
        data.levelSetIndex = state.levelSetIndex;
        data.currentDifficulty = state.currentDifficulty;
        data.currentType = state.currentType;

        data.levelStartingPoints = ConvertSetsToSerializeable(state.startingPoints);
        data.savedCoorinateState = ConvertSetsToSerializeable(state.stateCoordinates);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(saveGamePath);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public SessionState LoadSavedSessionState()
    {
        SessionSaveData state = new SessionSaveData();
        SessionState savedSession = new SessionState();

        if (File.Exists(saveGamePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(saveGamePath, FileMode.Open);
            state = (SessionSaveData)bf.Deserialize(fs);
            fs.Close();
        }
        else
        {
            Debug.Log("Error loading file: " + saveGamePath);
            state = null;
        }

        if (state != null)
        {
            savedSession.trackerTime = state.trackerTime;
            savedSession.trackerMoves = state.trackerMoves;
            savedSession.isSaved = state.isSaved;
            savedSession.solved = state.solved;
            savedSession.currentLevelIndex = state.currentLevelIndex;
            savedSession.levelSetIndex = state.levelSetIndex;
            savedSession.currentDifficulty = state.currentDifficulty;
            savedSession.currentType = state.currentType;

            savedSession.startingPoints = ConvertSetsToPlayable(state.levelStartingPoints);
            savedSession.stateCoordinates = ConvertSetsToPlayable(state.savedCoorinateState);
        } else
        {
            return null;
        }

        return savedSession;
    }

}

[Serializable]
public class CampaignSaveData
{
    public int bestMove;
    public float bestTime;
    public int campaignID;
    public int levelID;
}

[Serializable]
public class FavoriteSaveData
{
    public int bestMove;
    public float bestTime;
    public long favoriteID;
}

[Serializable]
public class FavoriteLevelSaveData
{
    public int bestMove;
    public float bestTime;
    public long favoriteID;
    public Difficulty difficulty;
    public List<SerializableSingleSetCoordinates> nodeSets;
    public string description;
}

[Serializable]
public class SerializableSingleSetCoordinates
{
    public List<SerializableVector2> coordsForSet;
}

[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;
}

[Serializable]
public class CampaignLockSaveData
{
    public int campaignId;
    public int lockIndex;
}

[Serializable]
public class SessionSaveData
{
    public float trackerTime;
    public int trackerMoves;
    public bool solved;
    public bool isSaved;
    public Difficulty currentDifficulty;

    public int levelSetIndex;
    public int currentLevelIndex;
    public SessionType currentType;

    public List<SerializableSingleSetCoordinates> savedCoorinateState;
    public List<SerializableSingleSetCoordinates> levelStartingPoints;
}

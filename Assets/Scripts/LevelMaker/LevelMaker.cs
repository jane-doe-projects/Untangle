using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class LevelMaker : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    public Level currentLevel;
    public List<Vector2> currentNodeset;
    string savePath = "Assets/Presets/LevelMaker/";
    string assetEnding = ".asset";
    public GameObject coordParent;

    public GameObject lineParent;
    public LinePreview currentLinePreview;
    [SerializeField] GameObject linePreviewPrefab;

    [SerializeField] bool hideTemplateSlots;
    [SerializeField] GameObject templateParent;
    [SerializeField] bool hideHelpGrid;
    [SerializeField] GameObject playAreaHelpGrid;

    List<Color> setColors;
    int colorIndex = 0;

    private void Start()
    {
        if (hideTemplateSlots)
            templateParent.SetActive(false);
        if (hideHelpGrid)
            playAreaHelpGrid.SetActive(false);

        InitColorList();

        RecordingCoordinate.onAddCoordinateDelegate += AddCoordinate;
        RecordingCoordinate.onAddCoordinateDelegate += RenderLine;
        NewLevel();
    }

    public void NewLevel()
    {
        bool success = true;
        // save current level as asset with name from textarea
        if (currentLevel != null)
            success = SaveLevel();

        if (success)
        {
            // clear playarea nodes
            ResetRecordingCoordinates();

            // clear lines
            foreach (Transform child in lineParent.transform)
                Destroy(child.gameObject);

            // create new empty asset
            currentLevel = ScriptableObject.CreateInstance<Level>();
            // initialize empty nodeset list
            currentLevel.nodeSetsCoordinates = new List<SingleSetCoordinates>();
            //currentNodeset = new List<Vector2>();
        }
    }

    public void AddCoordinate(Vector2 coord)
    {
        currentNodeset.Add(coord);
    }

    public void NewNodeSet()
    {
        colorIndex++;
        if (currentNodeset != null && currentNodeset.Count > 0)
        {
            SingleSetCoordinates singleSet = new SingleSetCoordinates();
            singleSet.coordsForSet = currentNodeset;

            currentLevel.AddNodeSet(singleSet);
        }

        currentNodeset = new List<Vector2>();

        currentLinePreview = Instantiate(linePreviewPrefab, lineParent.transform).GetComponent<LinePreview>();
    }

    public void ButtonSaveLevel()
    {
        NewLevel();
    }

    public bool SaveLevel()
    {
        NewNodeSet(); // saves the last node set to the level

        if (nameInput.text == "")
        {
            nameInput.GetComponent<Image>().color = Color.red;
            Debug.LogWarning("No name set for asset. Asset wont be saved.");
            return false;
        }

        currentLevel.descName = nameInput.text;
        string assetName = nameInput.text.Replace(" ", "");
        string path = savePath + assetName + assetEnding;

        if (File.Exists(path))
        {
            Debug.LogWarning("Asset with that name already exists. Choose a different name.");
            nameInput.GetComponent<Image>().color = Color.red;
            return false;
        }

        nameInput.text = "";
        nameInput.GetComponent<Image>().color = Color.white;

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(currentLevel, path);
#endif
        Debug.Log("Level " + assetName + " was saved.");
        colorIndex = 0;

        return true;
    }

    void ResetRecordingCoordinates()
    {
        foreach (Transform child in coordParent.transform)
        {
            RecordingCoordinate rec = child.GetComponent<RecordingCoordinate>();
            rec.Reset();
        }
    }

    void InitColorList()
    {
        setColors = new List<Color>();
        setColors.Add(Color.red);
        setColors.Add(Color.blue);
        setColors.Add(Color.green);
        setColors.Add(Color.yellow);
        setColors.Add(Color.cyan);
        setColors.Add(Color.magenta);
        setColors.Add(Color.white);
        setColors.Add(Color.gray);
    }

    public Color GetSetColor()
    {
        // start at 0 if more sets are being created in a level than there are colors to choose from
        int safeIndex = colorIndex % setColors.Count;
        return setColors[safeIndex];
    }

    void RenderLine(Vector2 coord)
    {
        if (currentLinePreview == null)
            currentLinePreview = Instantiate(linePreviewPrefab, lineParent.transform).GetComponent<LinePreview>();

        currentLinePreview.AddPosition(coord);
    }

    public void ResetLevel()
    {
        colorIndex = 0;
        // clear playarea nodes
        ResetRecordingCoordinates();

        // clear lines
        foreach (Transform child in lineParent.transform)
            Destroy(child.gameObject);

        // create new empty asset
        currentLevel = ScriptableObject.CreateInstance<Level>();
        // initialize empty nodeset list
        currentLevel.nodeSetsCoordinates = new List<SingleSetCoordinates>();

        currentNodeset = new List<Vector2>();
        currentLinePreview = Instantiate(linePreviewPrefab, lineParent.transform).GetComponent<LinePreview>();
    }
}

using UnityEngine;
using System.IO;
using TMPro;

public class CreditsPlacement : MonoBehaviour
{
    [SerializeField] TextAsset creditsFile;
    [SerializeField] TextMeshProUGUI test;

    string creditsDir = "/Credits";
    string fileName = "/credits_sources.txt";

    // Start is called before the first frame update
    void Start()
    {
        PlaceFile();
    }

    void PlaceFile()
    {
        string path = Application.dataPath;
        string fullPath = path + creditsDir + fileName;

        if (!Directory.Exists(path + creditsDir))
            Directory.CreateDirectory(path + creditsDir);

        if (!File.Exists(fullPath))
        {
            StreamWriter sw = new StreamWriter(fullPath, true);
            sw.WriteLine(creditsFile.text);
            sw.Close();
        }
    }
}

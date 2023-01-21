using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "New Word List", menuName = "Favorites/Word List")]
public class WordList : ScriptableObject
{
    [SerializeField] TextAsset wordListFile;
    public static List<string> words;

    public void ReadInWords()
    {
        // read in file
        string textFile = wordListFile.text;
        words = GetLines(textFile);
    }

    List<string> GetLines(string textString)
    {
        List<string> words;

        string[] split = Regex.Split(textString, "\n");
        words = new List<string>(split);

        return words;
    }

    public static string GetRandomWord()
    {
        int randomIndex = Random.Range(0, WordList.words.Count-1);

        return WordList.words[randomIndex];
    }

}

using UnityEngine;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject levelItemPrefab; // Drag the prefab here
    public Transform levelsParent;     // Drag the Levels GameObject here
    public List<SongData> allSongs;    // Add your songs via inspector or dynamically

    void Start()
    {
        List<SongData> allSongs = LoadSongsFromCSV("LevelMenu");

        foreach (var song in allSongs)
        {
            GameObject levelItem = Instantiate(levelItemPrefab, levelsParent);
            LevelItemUI ui = levelItem.GetComponent<LevelItemUI>();
            ui.Setup(song);
        }
    }

    public static List<SongData> LoadSongsFromCSV(string filename)
    {
        List<SongData> songs = new List<SongData>();

        TextAsset csvFile = Resources.Load<TextAsset>(filename); // e.g., "Songs"
        if (csvFile == null)
        {
            Debug.LogError($"CSV file '{filename}' not found in Resources.");
            return songs;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 4) continue;

            SongData song = new SongData
            {
                songTitle     = values[0],
                bpm           = values[1],
                difficulty    = int.Parse(values[2]),
                groupCSVName  = values[3]
            };

            songs.Add(song);
        }

        return songs;
    }
}


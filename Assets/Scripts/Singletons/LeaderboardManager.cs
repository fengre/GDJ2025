using UnityEngine;
using System.Collections.Generic;

public static class LeaderboardManager
{
    private const int maxEntries = 10;
    private const string keyPrefix = "HighScore_";

    public static void SaveScore(int score, string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        List<int> scores = GetScores(songTitle);

        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // Descending
        
        if (scores.Count > maxEntries)
            scores.RemoveAt(scores.Count - 1);

        // Save scores for this specific song
        for (int i = 0; i < scores.Count; i++)
            PlayerPrefs.SetInt($"{songKey}{i}", scores[i]);

        PlayerPrefs.Save();
    }

    public static List<int> GetScores(string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        List<int> scores = new List<int>();

        for (int i = 0; i < maxEntries; i++)
        {
            if (PlayerPrefs.HasKey($"{songKey}{i}"))
                scores.Add(PlayerPrefs.GetInt($"{songKey}{i}"));
        }

        return scores;
    }

    // Optional: Method to clear scores for testing
    public static void ClearScores(string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        for (int i = 0; i < maxEntries; i++)
        {
            PlayerPrefs.DeleteKey($"{songKey}{i}");
        }
        PlayerPrefs.Save();
    }
}
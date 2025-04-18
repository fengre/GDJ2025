using UnityEngine;
using System.Collections.Generic;

public static class LeaderboardManager
{
    private const int maxEntries = 5;
    private const string keyPrefix = "HighScore_";

    public static void SaveScore(int score)
    {
        List<int> scores = GetScores();
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // Descending
        if (scores.Count > maxEntries)
            scores.RemoveAt(scores.Count - 1);

        for (int i = 0; i < scores.Count; i++)
            PlayerPrefs.SetInt($"{keyPrefix}{i}", scores[i]);

        PlayerPrefs.Save();
    }

    public static List<int> GetScores()
    {
        List<int> scores = new List<int>();

        for (int i = 0; i < maxEntries; i++)
        {
            if (PlayerPrefs.HasKey($"{keyPrefix}{i}"))
                scores.Add(PlayerPrefs.GetInt($"{keyPrefix}{i}"));
        }

        return scores;
    }
}

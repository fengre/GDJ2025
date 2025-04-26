using UnityEngine;
using System.Collections.Generic;

public static class LeaderboardManager
{   
    private const int maxEntries = 10;
    private const string keyPrefix = "HighScore_";
    private const string streakPrefix = "Streak_";

    public static void SaveScore(int score, int streak, string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        string streakKey = $"{streakPrefix}{songTitle}_";
        List<ScoreEntry> scores = GetScores(songTitle);

        scores.Add(new ScoreEntry(score, streak));
        scores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort by score descending
        
        if (scores.Count > maxEntries)
            scores.RemoveAt(scores.Count - 1);

        // Save scores and streaks
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"{songKey}{i}", scores[i].score);
            PlayerPrefs.SetInt($"{streakKey}{i}", scores[i].streak);
        }

        PlayerPrefs.Save();
    }

    public static List<ScoreEntry> GetScores(string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        string streakKey = $"{streakPrefix}{songTitle}_";
        List<ScoreEntry> scores = new List<ScoreEntry>();

        for (int i = 0; i < maxEntries; i++)
        {
            if (PlayerPrefs.HasKey($"{songKey}{i}"))
            {
                int score = PlayerPrefs.GetInt($"{songKey}{i}");
                int streak = PlayerPrefs.GetInt($"{streakKey}{i}");
                scores.Add(new ScoreEntry(score, streak));
            }
        }

        scores.Sort((a, b) => b.score.CompareTo(a.score));
        return scores;
    }

    public static List<ScoreEntry> GetStreaks(string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        string streakKey = $"{streakPrefix}{songTitle}_";
        List<ScoreEntry> streaks = new List<ScoreEntry>();

        for (int i = 0; i < maxEntries; i++)
        {
            if (PlayerPrefs.HasKey($"{songKey}{i}"))
            {
                int score = PlayerPrefs.GetInt($"{songKey}{i}");
                int streak = PlayerPrefs.GetInt($"{streakKey}{i}");
                streaks.Add(new ScoreEntry(score, streak));
            }
        }

        streaks.Sort((a, b) => b.streak.CompareTo(a.streak));
        return streaks;
    }

    public static void ClearScores(string songTitle)
    {
        string songKey = $"{keyPrefix}{songTitle}_";
        string streakKey = $"{streakPrefix}{songTitle}_";
        for (int i = 0; i < maxEntries; i++)
        {
            PlayerPrefs.DeleteKey($"{songKey}{i}");
            PlayerPrefs.DeleteKey($"{streakKey}{i}");
        }
        PlayerPrefs.Save();
    }
}
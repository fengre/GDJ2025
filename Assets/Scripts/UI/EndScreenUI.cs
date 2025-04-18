using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ratingText;
    public TextMeshProUGUI leaderboardText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        scoreText.text = $"Final Score: {finalScore}";
        ratingText.text = $"Rating: {GetRating(finalScore)}";
        ShowLeaderboard();
    }

    string GetRating(int score)
    {
        if (score >= 1000) return "S";
        if (score >= 800) return "A";
        if (score >= 600) return "B";
        if (score >= 400) return "C";
        return "D";
    }

    void ShowLeaderboard()
    {
        List<int> topScores = LeaderboardManager.GetScores();
        leaderboardText.text = "Top Scores:\n";

        for (int i = 0; i < topScores.Count; i++)
        {
            leaderboardText.text += $"{i + 1}. {topScores[i]}\n";
        }
    }

}

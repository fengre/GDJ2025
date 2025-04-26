using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI ratingText;
    // public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI scoreLeaderboardText;
    public TextMeshProUGUI streakLeaderboardText;

    [Header("Hit Stats Pie Chart")]
    public Image perfectSlice;
    public Image greatSlice;
    public Image goodSlice;


    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int maxStreak = PlayerPrefs.GetInt("MaxStreak", 0);
        scoreText.text = $"{finalScore}";
        streakText.text = $"{maxStreak}";
        ratingText.text = $"{GetRating(finalScore)}";
        ShowLeaderboard();
        DisplayStats();
    }

    private string GetRating(int score)
    {
        if (score >= 3000) return "S";
        if (score >= 2000) return "A";
        if (score >= 1000) return "B";
        if (score >= 800) return "C";
        return "D";
    }

    private void ShowLeaderboard()
    {
        string songTitle = LevelManager.Instance.currentSong.songTitle;
        List<ScoreEntry> topScores = LeaderboardManager.GetScores(songTitle);
        List<ScoreEntry> topStreaks = LeaderboardManager.GetStreaks(songTitle);
        // songTitleText.text = songTitle;
        scoreLeaderboardText.text = "";
        streakLeaderboardText.text = "";

        if (topScores.Count == 0)
        {
            scoreLeaderboardText.text += "No scores yet!";
            return;
        }


        for (int i = 0; i < topScores.Count; i++)
        {
            if (i >= 3) break; // Limit to top 10 scores
            scoreLeaderboardText.text += $"{i + 1}. {topScores[i].score:N0}\n";
        }

        for (int i = 0; i < topStreaks.Count; i++)
        {
            if (i >= 3) break; // Limit to top 10 scores
            streakLeaderboardText.text += $"{i + 1}. {topStreaks[i].streak:N0}\n";
        }
    }


    public void DisplayStats()
    {
        int total = ScoreManager.Instance.TotalNotes;
        float perfectRatio = ScoreManager.Instance.PerfectHits / (float)total;
        float greatRatio = ScoreManager.Instance.GreatHits / (float)total;
        float goodRatio = ScoreManager.Instance.GoodHits / (float)total;

        perfectSlice.fillAmount = perfectRatio;
        greatSlice.fillAmount = perfectRatio + greatRatio;
        goodSlice.fillAmount = perfectRatio + greatRatio + goodRatio;

        Debug.Log(perfectRatio + "" + greatRatio + "" + goodRatio);

        perfectSlice.color = new Color32(0x4C, 0xAF, 0x50, 255);
        greatSlice.color = new Color32(0xFF, 0xC1, 0x07, 255);
        goodSlice.color = new Color32(0xFF, 0x98, 0x00, 255);
    }



}

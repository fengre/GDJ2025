using UnityEngine;
[System.Serializable]
public class ScoreEntry
{
    public int score;
    public int streak;

    public ScoreEntry(int score, int streak)
    {
        this.score = score;
        this.streak = streak;
    }
}
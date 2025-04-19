using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("Group Reference")]
    public GroupManager groupManager;

    [Header("Score Settings")]
    public float idealMin = 45f;
    public float idealMax = 55f;
    public float okayMin = 25f;
    public float okayMax = 75f;

    public int perfectPointsPerSecond = 10;
    public int okayPointsPerSecond = 5;

    private float scoreTimer = 0f;
    private int totalScore = 0;

    private void Update()
    {
        scoreTimer += Time.deltaTime;

        if (scoreTimer >= 1f)
        {
            scoreTimer -= 1f;

            int earned = CalculateScoreFromGroups();
            totalScore += earned;
        }
    }

    private int CalculateScoreFromGroups()
    {
        int earned = 0;

        foreach (var group in groupManager.groups)
        {
            if (group.groupValue >= idealMin && group.groupValue <= idealMax)
            {
                earned += perfectPointsPerSecond;
            }
            else if (group.groupValue >= okayMin && group.groupValue <= okayMax)
            {
                earned += okayPointsPerSecond;
            }
        }

        return earned;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}

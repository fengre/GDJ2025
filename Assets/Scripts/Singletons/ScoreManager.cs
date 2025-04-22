using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    public float idealMin = 45f;
    public float idealMax = 55f;
    public float okayMin = 25f;
    public float okayMax = 75f;

    public int perfectPointsPerSecond = 10;
    public int okayPointsPerSecond = 5;

    private float scoreTimer = 0f;
    private int totalScore = 0;

    [SerializeField] private int totalNotesHit;
    [SerializeField] private int perfectHits;
    [SerializeField] private int greatHits;
    [SerializeField] private int goodHits;
    [SerializeField] private int totalNotes;

    public int TotalNotesHit => totalNotesHit;
    public int PerfectHits => perfectHits;
    public int GreatHits => greatHits;
    public int GoodHits => goodHits;
    public int TotalNotes => totalNotes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    public void ResetValues()
    {
        scoreTimer = 0f;
        totalScore = 0;
        totalNotesHit = 0;
        perfectHits = 0;
        greatHits = 0;
        goodHits = 0;
        totalNotes = 0;
    }

    public void RegisterNote() => totalNotes++;

    public void RegisterHit(HitRating rating)
    {
        totalNotesHit++;

        switch (rating)
        {
            case HitRating.Perfect:
                perfectHits++;
                break;
            case HitRating.Great:
                greatHits++;
                break;
            case HitRating.Good:
                goodHits++;
                break;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")  // Replace with your actual scene name
        {
            scoreTimer += Time.deltaTime;

            if (scoreTimer >= 1f)
            {
                scoreTimer -= 1f;

                int earned = CalculateScoreFromGroups();
                totalScore += earned;
            }
        }
    }

    private int CalculateScoreFromGroups()
    {
        int earned = 0;

        foreach (var group in GroupManager.Instance.groups)
        {
            if (!group.hasStarted) continue;
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

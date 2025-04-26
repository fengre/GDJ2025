using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StreakAlertUI : AlertUI
{
    public static StreakAlertUI Instance { get; private set; }

    [Header("Streak Settings")]
    public Color normalColor = Color.white;
    public Color[] milestoneColors = {
        new Color(1f, 1f, 0f),      // Yellow (50)
        new Color(1f, 0.5f, 0f),    // Orange (100)
        new Color(1f, 0f, 0f)       // Red (200+)
    };
    public int[] milestoneThresholds = { 50, 100, 200 };

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateStreak(int streak)
    {
        // if (streak == 0)
        // {
        //     canvasGroup.alpha = 0f;
        //     return;
        // }

        // Determine color based on milestone thresholds
        Color targetColor = normalColor;
        for (int i = 0; i < milestoneThresholds.Length; i++)
        {
            if (streak >= milestoneThresholds[i] && i < milestoneColors.Length)
            {
                targetColor = milestoneColors[i];
            }
        }

        Show($"x{streak}", targetColor);
    }
}
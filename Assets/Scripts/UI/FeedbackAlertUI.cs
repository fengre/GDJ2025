using UnityEngine;

public class FeedbackAlertUI : AlertUI
{
    public static FeedbackAlertUI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Show the hit feedback in the center of the screen.
    /// </summary>
    public void ShowFeedback(HitRating rating)
    {
        // pick the message and color
        string msg;
        Color col;
        switch (rating)
        {
            case HitRating.Perfect:
                msg = "Perfect!";
                col = Color.green;
                break;
            case HitRating.Great:
                msg = "Great!";
                col = Color.cyan;
                break;
            case HitRating.Good:
                msg = "Good";
                col = Color.yellow;
                break;
            default:
                msg = "Miss";
                col = Color.red;
                break;
        }

        Show(msg, col);
    }
}

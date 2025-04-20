using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("Prefab & Parent")]
    public FeedbackUI feedbackPrefab;   // your HitFeedbackText prefab
    public RectTransform uiParent;      // the Canvas’s RectTransform

    private FeedbackUI feedbackUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Instantiate once, as a child of the UI canvas
        feedbackUI = Instantiate(feedbackPrefab, uiParent);
        RectTransform rt = feedbackUI.GetComponent<RectTransform>();
        // Center it in the canvas
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0f, -250f);
        // Start hidden
        feedbackUI.canvasGroup.alpha = 0f;
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

        feedbackUI.Show(msg, col);
    }
}

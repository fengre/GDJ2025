using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("Prefab & Parent")]
    public FeedbackUI feedbackPrefab;      
    public RectTransform uiParent;         // your Canvas’s RectTransform
    public Camera  uiCamera;               // the Camera rendering your UI Canvas

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// rating: Perfect/Great/Good/Miss
    /// worldPos: the world‐space point to anchor the feedback at
    /// </summary>
    public void ShowFeedback(HitRating rating, Vector3 worldPos)
    {
        // 1) Instantiate under the UI canvas
        FeedbackUI fb = Instantiate(feedbackPrefab, uiParent);

        // 2) Decide which camera to use for conversion
        Canvas canvas = uiParent.GetComponentInParent<Canvas>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : uiCamera;

        // 3) Convert world → screen → canvas local
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
        Debug.Log($"WorldPos: {worldPos} → ScreenPoint: {screenPoint}");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiParent, screenPoint, cam, out Vector2 localPoint);

        Debug.Log($"LocalPoint in UI: {localPoint}");

        // 4) Apply the position
        RectTransform rt = fb.GetComponent<RectTransform>();
        rt.anchoredPosition = localPoint;

        // 5) Show & destroy
        fb.Show(rating.ToString(), ColorForRating(rating));
    }

    private Color ColorForRating(HitRating r)
    {
        switch (r)
        {
            case HitRating.Perfect: return Color.green;
            case HitRating.Great:   return Color.cyan;
            case HitRating.Good:    return Color.yellow;
            default:                return Color.red;
        }
    }
}

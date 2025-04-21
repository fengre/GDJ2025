using UnityEngine;

public class JudgmentLineLane : MonoBehaviour
{
    public int laneIndex; // set this in the Inspector to 0–3

    private JudgmentLineGlow _glow;

    void Awake()
    {
        _glow = GetComponent<JudgmentLineGlow>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var note = other.GetComponent<NoteMover>();
        if (note != null && note.lane == laneIndex && !note.isHit)
        {
            note.isHittable = true;
            // trigger the glow
            // _glow?.TriggerGlow();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var note = other.GetComponent<NoteMover>();
        if (note != null && note.lane == laneIndex && !note.isHit)
        {
            // miss logic...
            FeedbackAlertUI.Instance.ShowFeedback(HitRating.Miss);
            note.isHittable = false;
        }
    }
}

// File: InputHandler.cs
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Key → Lane Mapping")]
    public KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };
    public Transform judgmentLine;

    // Track the hold note we started per lane
    private NoteMover[] activeHoldNotes;

    void Awake()
    {
        activeHoldNotes = new NoteMover[laneKeys.Length];
    }

    void Update()
    {
        double songTime = GameManager.Instance.GetSongTime();
        float judgmentY = judgmentLine.position.y;

        HandleLaneInput(songTime, judgmentY);
        HandleGroupSwitching();
    }

    private void HandleLaneInput(double songTime, float judgmentY)
    {
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                HandleKeyDown(lane, songTime, judgmentY);
            }

            if (Input.GetKeyUp(laneKeys[lane]))
            {
                HandleKeyUp(lane, songTime);
            }
        }
    }

    private void HandleKeyDown(int lane, double songTime, float judgmentY)
    {
        var notes = FindObjectsByType<NoteMover>(FindObjectsSortMode.None);
        bool beganHold = false;

        // Try to begin a hold note
        foreach (var note in notes)
        {
            if (note.lane != lane || note.isHit) continue;
            if (note.TryBeginHold(songTime, NoteManager.Instance.hitThreshold, judgmentY))
            {
                activeHoldNotes[lane] = note;
                beganHold = true;
                break;
            }
        }

        // If no hold note, try a tap note
        if (!beganHold)
        {
            HandleTap(lane, notes, judgmentY);
        }

        // Trigger glow effect
        NoteManager.Instance
                   .laneJudgementLines[lane]
                   .GetComponent<JudgmentLineGlow>()
                   .TriggerGlow();
    }

    private void HandleTap(int lane, NoteMover[] notes, float judgmentY)
    {
        NoteMover bestTap = null;
        HitRating? bestRating = null;

        foreach (var note in notes)
        {
            if (note.lane != lane || note.isHit) continue;

            var rating = note.TryTapHit(NoteManager.Instance.hitThreshold, NoteManager.Instance.greatThreshold, NoteManager.Instance.perfectThreshold, judgmentY);
            if (rating.HasValue)
            {
                bestTap = note;
                bestRating = rating;
                break;
            }
        }

        if (bestTap != null && bestRating.HasValue)
        {
            HandleHit(bestTap, bestRating.Value);
        }
    }

    private void HandleKeyUp(int lane, double songTime)
    {
        var note = activeHoldNotes[lane];
        if (note != null)
        {
            Debug.Log("key up on hold note: " + note.duration);
            var rating = note.EndHold(songTime);
            HandleHit(note, rating);
            activeHoldNotes[lane] = null;
        }
        NoteManager.Instance
                   .laneJudgementLines[lane]
                   .GetComponent<JudgmentLineGlow>()
                   .CancelGlow();
    }

    private void HandleGroupSwitching()
    {
        for (int g = 0; g < NoteManager.Instance.noteCsvFiles.Count; g++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + g))
            {
                NoteManager.Instance.SwitchGroup(g);
            }
        }

        // Modified arrow key handling
        int currentGroup = NoteManager.Instance.activeGroup;
        int totalGroups = GroupManager.Instance.groups.Count;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Try previous groups until finding one that's not shutdown
            for (int i = 1; i <= totalGroups; i++)
            {
                int newGroup = (currentGroup - i + totalGroups) % totalGroups;
                if (!GroupManager.Instance.groups[newGroup].isShutDown)
                {
                    NoteManager.Instance.SwitchGroup(newGroup);
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Try next groups until finding one that's not shutdown
            for (int i = 1; i <= totalGroups; i++)
            {
                int newGroup = (currentGroup + i) % totalGroups;
                if (!GroupManager.Instance.groups[newGroup].isShutDown)
                {
                    NoteManager.Instance.SwitchGroup(newGroup);
                    break;
                }
            }
        }
    }

    private void HandleHit(NoteMover note, HitRating rating)
    {
        float change = GetChangeAmount(rating);

        // Feedback
        FeedbackAlertUI.Instance.ShowFeedback(rating);

        // Group value
        GroupManager.Instance.ChangeGroupValue(note.groupIndex, change);
        GroupUIManager.Instance.UpdateGroupValue(
            note.groupIndex,
            GroupManager.Instance.GetGroupValue(note.groupIndex)
        );

        // Score
        ScoreManager.Instance.RegisterHit(rating);
    }

    private float GetChangeAmount(HitRating rating)
    {
        switch (rating)
        {
            case HitRating.Perfect: return GroupManager.Instance.perfectHitIncrease;
            case HitRating.Great:   return GroupManager.Instance.greatHitIncrease;
            case HitRating.Good:    return GroupManager.Instance.goodHitIncrease;
            default:                return -GroupManager.Instance.missDecrease;
        }
    }
}
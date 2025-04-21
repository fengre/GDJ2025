// File: InputHandler.cs
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Assign keys for each lane in order.
    public KeyCode[] laneKeys = new KeyCode[4] { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    // A reference to the judgment line (to determine a reference position).
    public Transform judgmentLine;

    // Define how close (in world units) a note must be to be considered a valid hit.
    public float hitThreshold = 1f;
    public float greatThreshold = 0.5f;
    public float perfectThreshold = 0.1f;

    void Update()
    {
        // Check for input on each lane key.
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                AttemptHit(lane);

                var line = NoteManager.Instance.laneJudgementLines[lane];
                line.GetComponent<JudgmentLineGlow>().TriggerGlow();
            }
        }

        // listen for 1–4
        for (int g = 0; g < NoteManager.Instance.noteCsvFiles.Count; g++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + g)) {
                NoteManager.Instance.SwitchGroup(g);
                NoteGroup group = GroupManager.Instance.groups[g];
                SwitchGroupAlertUI.Instance.Show(group.groupName, group.groupColor);
            }
        }

    }

    void AttemptHit(int lane)
    {
        NoteMover[] notes = FindObjectsByType<NoteMover>(FindObjectsSortMode.None);
        NoteMover bestNote = null;
        float smallestDistance = Mathf.Infinity;
        float judgmentY = judgmentLine.position.y;
        
        foreach (NoteMover note in notes)
        {
            if (note.lane == lane && note.isHittable && !note.isHit)
            {
                float distance = Mathf.Abs(note.transform.position.y - judgmentY);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    bestNote = note;
                }
            }
        }
        
        HitRating rating;
        if (bestNote != null && smallestDistance <= hitThreshold)
        {
            bestNote.isHit = true;
            Destroy(bestNote.gameObject);

            // Determine hit rating.
            float changeAmount = 0f;
            if (smallestDistance <= perfectThreshold)
            {
                // Debug.Log("perfect hit");
                rating = HitRating.Perfect;
                changeAmount = GroupManager.Instance.perfectHitIncrease;
            }
            else if (smallestDistance <= greatThreshold)
            {
                // Debug.Log("great hit");
                rating = HitRating.Great;
                changeAmount = GroupManager.Instance.greatHitIncrease;
            }
            else // within good threshold.
            {
                // Debug.Log("good hit");
                rating = HitRating.Good;
                changeAmount = GroupManager.Instance.goodHitIncrease;
            }

            FeedbackAlertUI.Instance.ShowFeedback(rating);

            // then:
            GroupManager.Instance.ChangeGroupValue(bestNote.groupIndex, changeAmount);
            GroupUIManager.Instance.UpdateGroupValue(bestNote.groupIndex, GroupManager.Instance.GetGroupValue(bestNote.groupIndex));
            ScoreManager.Instance.RegisterHit(rating);
        }
        else
        {
            // Debug.Log("miss hit");
            // Vector3 missPos = new Vector3(
            //     0f,
            //     0f,
            //     0f);
            // FeedbackAlertUI.Instance.ShowFeedback(HitRating.Miss, missPos);
        }
    }

}

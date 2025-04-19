// File: JudgmentLine.cs
using UnityEngine;

public class JudgmentLine : MonoBehaviour
{
    // Called when any collider enters the judgment zone.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            NoteMover note = other.GetComponent<NoteMover>();
            if (note != null)
            {
                note.isHittable = true;
                // Debug.Log("Note entered judgment zone: Lane " + note.lane);
            }
        }
    }

    // Called when a collider exits the judgment zone.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            NoteMover note = other.GetComponent<NoteMover>();
            if (note != null)
            {
                // If the note exits without being hit, it’s a miss.
                if (!note.isHit)
                {
                    // Debug.Log("Missed note in lane " + note.lane);
                    // Additional miss handling logic (such as reducing score) can go here.
                    Vector3 missPos = new Vector3(
                        SpawnManager.Instance.laneSpawnPoints[note.lane].position.x,
                        transform.position.y,
                        0f);
                    FeedbackManager.Instance.ShowFeedback(HitRating.Miss, missPos);
                }
                note.isHittable = false;
            }
        }
    }
}

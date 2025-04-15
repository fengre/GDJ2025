// File: InputHandler.cs
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Assign keys for each lane in order.
    public KeyCode[] laneKeys = new KeyCode[4] { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    // A reference to the judgment line (to determine a reference position).
    public Transform judgmentLine;

    // Define how close (in world units) a note must be to be considered a valid hit.
    public float hitThreshold = 0.5f;

    void Update()
    {
        // Check for input on each lane key.
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                AttemptHit(lane);
            }
        }
    }

    void AttemptHit(int lane)
    {
        // Find all active notes in the scene.
        NoteMover[] notes = FindObjectsOfType<NoteMover>();
        NoteMover bestNote = null;
        float smallestDistance = Mathf.Infinity;
        float judgementY = judgmentLine.position.y;
        
        foreach (NoteMover note in notes)
        {
            // Check if this note is in the correct lane and is within the judgment zone.
            if (note.lane == lane && note.isHittable && !note.isHit)
            {
                // We compare the y-distance to the judgment line as a simple measure.
                float distance = Mathf.Abs(note.transform.position.y - judgementY);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    bestNote = note;
                }
            }
        }
        
        if (bestNote != null && smallestDistance <= hitThreshold)
        {
            // Register a successful hit.
            bestNote.isHit = true;
            Destroy(bestNote.gameObject);
            Debug.Log("Hit note in lane " + lane + " with accuracy of " + smallestDistance);
            // Additional scoring logic can be added here (e.g., awarding points for perfect/great/good).
        }
        else
        {
            Debug.Log("Missed input in lane " + lane);
            // You can also handle a miss here (e.g., reducing score or showing a visual cue).
        }
    }
}

// File: NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float moveSpeed = 300f; // This is set from GameManager.
    public int lane;             // Which lane (0–3) this note belongs to.
    public bool isHittable = false; // Becomes true when the note is within the judgment zone.
    public bool isHit = false;      // To prevent re-hitting the same note.

    void Update()
    {
        // Move downward. Adjust if your coordinate system differs.
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}

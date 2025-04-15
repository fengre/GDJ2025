// File: JudgmentLine.cs
using UnityEngine;

public class JudgmentLine : MonoBehaviour
{
    // Called when a note enters the judgment line collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            Debug.Log("Note reached the judgment line in lane!");
            // Here you can mark the note as ready for a hit.
            // You might optionally remove or disable movement to await input.
        }
    }

    // Called when a note exits the judgment line without being hit.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            Debug.Log("Missed note in lane!");
            // Handle miss logic, such as reducing score, triggering an animation, etc.
        }
    }
}

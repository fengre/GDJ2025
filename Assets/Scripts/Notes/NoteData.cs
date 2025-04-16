using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float timeToHit; // The time at which the note should be hit.
    public int lane;        // The lane index (0 to 3).
    public Color noteColor; // Color for the note.
}
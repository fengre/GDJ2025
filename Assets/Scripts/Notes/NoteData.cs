using UnityEngine;

// File: NoteData.cs
[System.Serializable]
public class NoteData
{
    public float timeToHit;  // The exact time the note is to be hit.
    public int lane;         // The lane index (0 to 3) for the note.
    public Color noteColor = Color.white; // Customize the note's color.
}

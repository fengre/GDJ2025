// File: GameManager.cs
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public AudioSource music;                // Your audio source assigned via Inspector.
    public GameObject notePrefab;            // Note prefab for instantiation.
    public float noteTravelTime = 2f;          // Time (in seconds) for a note to travel from spawn to the judgment line.
    
    // List to hold note timing and lane data.
    public List<NoteData> notes = new List<NoteData>();

    // Array of lane spawn points. Ensure you set exactly four in the Inspector.
    public Transform[] laneSpawnPoints;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        music.Play();
    }

    void Update()
    {
        float songTime = Time.time - startTime;
        
        // Spawn notes when the song time (plus the lead time) meets the note's hit time.
        while (notes.Count > 0 && notes[0].timeToHit - noteTravelTime <= songTime)
        {
            SpawnNote(notes[0]);
            notes.RemoveAt(0);
        }
    }

    void SpawnNote(NoteData data)
    {
        // Ensure the lane index is valid.
        if (data.lane >= 0 && data.lane < laneSpawnPoints.Length)
        {
            // Instantiate the note at the designated lane spawn point.
            GameObject note = Instantiate(notePrefab, laneSpawnPoints[data.lane].position, laneSpawnPoints[data.lane].rotation);
            
            // Get the SpriteRenderer component (or Image component if using UI)
            SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = data.noteColor; // Apply the custom color.
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on the note prefab to set color.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid lane number: " + data.lane);
        }
    }
}

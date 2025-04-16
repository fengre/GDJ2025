// File: GameManager.cs
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource music; // Attach your AudioSource (with your music clip)

    [Header("Note Settings")]
    public GameObject notePrefab; // Your note prefab
    public float noteSpeed = 300f; // Speed at which notes travel (units per second)
    
    // List of note events – each note defines when it should be hit.
    public List<NoteData> notes = new List<NoteData>();

    [Header("Lane Settings")]
    public Transform[] laneSpawnPoints; // Set 4 lane spawn points via inspector.
    public Transform judgmentLine;      // Reference to the Judgment Line GameObject

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        music.Play();

        // For demonstration, add a test note: lane 0 to be hit at 5 seconds.
        // notes.Add(new NoteData { timeToHit = 5f, lane = 0, noteColor = Color.red });
        // Add more notes as needed...
    }

    void Update()
    {
        float songTime = Time.time - startTime;

        // Check if there are any notes left to spawn.
        int index = 0;
        while (notes.Count > 0)
        {
            // Get the next note (assuming they are sorted by timeToHit).
            NoteData nextNote = notes[0];

            // Compute the travel time based on distance from the selected lane spawn to the judgment line.
            // First, get the appropriate spawn point for this note.
            if (nextNote.lane < 0 || nextNote.lane >= laneSpawnPoints.Length)
            {
                Debug.LogWarning("Invalid lane number in note data: " + nextNote.lane);
                notes.RemoveAt(0);
                continue;
            }

            Transform spawnPoint = laneSpawnPoints[nextNote.lane];
            float distance = Mathf.Abs(spawnPoint.position.y - judgmentLine.position.y);
            float travelTime = distance / noteSpeed;
            Debug.Log(index + ": " + distance + " " + travelTime);

            // Determine when the note should be spawned.
            float spawnTime = nextNote.timeToHit - travelTime;

            if (songTime >= spawnTime)
            {
                SpawnNote(nextNote, spawnPoint);
                notes.RemoveAt(0);
                index++;
            }
            else
            {
                // If it’s not yet time to spawn the next note, exit the loop.
                break;
            }
        }
    }

    // Inside the SpawnNote method in GameManager.cs
    void SpawnNote(NoteData data, Transform spawnPoint)
    {
        // Instantiate the note at the given spawn point.
        GameObject note = Instantiate(notePrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Set up the note's visual color if applicable.
        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = data.noteColor;
        }
        
        // Pass the note speed to the NoteMover.
        NoteMover mover = note.GetComponent<NoteMover>();
        if (mover != null)
        {
            mover.moveSpeed = noteSpeed;
            mover.lane = data.lane;  // Assign the lane
        }
    }
}

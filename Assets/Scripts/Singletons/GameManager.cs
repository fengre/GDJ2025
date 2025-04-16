using UnityEngine;
using System.Collections.Generic;
using System.Globalization; // For NumberStyles and InvariantCulture

public class GameManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource music;

    [Header("Note Settings")]
    public GameObject notePrefab; // Your note prefab.
    public float noteSpeed = 300f; // Note travel speed (units per second).

    // Instead of a preset Inspector list, we fill this list from CSV.
    public List<NoteData> notes = new List<NoteData>();

    [Header("Lane Settings")]
    public Transform[] laneSpawnPoints; // Set these in the Inspector (four lanes).
    public Transform judgmentLine;      // Reference to your judgment line.

    private float startTime;
    private float standardTravelTime;

    void Start()
    {
        LoadNotesFromCSV("NoteData"); // CSV file name without extension, in Resources folder.

        // Sort the list by timeToHit.
        notes.Sort((a, b) => a.timeToHit.CompareTo(b.timeToHit));

        startTime = Time.time;
        music.Play();

        // Compute a standard travel time using the vertical distance from one lane's spawn point.
        float verticalDistance = Mathf.Abs(laneSpawnPoints[0].position.y - judgmentLine.position.y);
        standardTravelTime = verticalDistance / noteSpeed;
    }

    void Update()
    {
        float songTime = Time.time - startTime;

        // Spawn notes when their scheduled spawn time is reached.
        while (notes.Count > 0 && notes[0].timeToHit - standardTravelTime <= songTime)
        {
            NoteData nextNote = notes[0];
            Transform spawnPoint = laneSpawnPoints[nextNote.lane];
            SpawnNote(nextNote, spawnPoint);
            notes.RemoveAt(0);
        }
    }

    /// <summary>
    /// Loads note data from a CSV file located in the Resources folder.
    /// </summary>
    /// <param name="fileName">CSV file name without extension</param>
    void LoadNotesFromCSV(string fileName)
    {
        // Load the file.
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("Could not find " + fileName + ".csv in Resources folder.");
            return;
        }

        // Split into lines.
        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file " + fileName + " is empty or missing data.");
            return;
        }

        // Loop through lines (skip header line).
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] tokens = line.Split(',');

            if (tokens.Length < 6)
            {
                Debug.LogWarning("Skipping invalid line " + i + " in CSV: " + line);
                continue;
            }

            NoteData note = new NoteData();
            // Parse using invariant culture to avoid localization issues.
            if (float.TryParse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float timeToHit))
                note.timeToHit = timeToHit;
            else
                Debug.LogWarning("Invalid timeToHit on line " + i);

            if (int.TryParse(tokens[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int lane))
                note.lane = lane;
            else
                Debug.LogWarning("Invalid lane on line " + i);

            // Parse color components.
            if (float.TryParse(tokens[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float r) &&
                float.TryParse(tokens[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float g) &&
                float.TryParse(tokens[4], NumberStyles.Float, CultureInfo.InvariantCulture, out float b) &&
                float.TryParse(tokens[5], NumberStyles.Float, CultureInfo.InvariantCulture, out float a))
            {
                note.noteColor = new Color(r, g, b, a);
            }
            else
            {
                Debug.LogWarning("Invalid color on line " + i);
                note.noteColor = Color.white; // default
            }

            notes.Add(note);
        }
    }

    void SpawnNote(NoteData data, Transform spawnPoint)
    {
        GameObject note = Instantiate(notePrefab, spawnPoint.position, spawnPoint.rotation);

        // Example with SpriteRenderer.
        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = data.noteColor;
        }

        // Get the NoteMover script and assign properties.
        NoteMover mover = note.GetComponent<NoteMover>();
        if (mover != null)
        {
            mover.moveSpeed = noteSpeed;
            mover.lane = data.lane;
            mover.noteColor = data.noteColor;
        }
    }

}

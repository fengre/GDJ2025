using UnityEngine;
using System.Collections.Generic;
using System.Globalization; // For NumberStyles and InvariantCulture

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadFromCSV("SongData"); // CSV file name without extension, in Resources folder.

        // Sort the list by timeToHit.
        notes.Sort((a, b) => a.timeToHit.CompareTo(b.timeToHit));

        startTime = Time.time;

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

    void LoadFromCSV(string filepath)
    {
        TextAsset file = Resources.Load<TextAsset>(filepath);
        string[] lines = file.text.Split('\n');

        List<NoteGroupData> groupDataList = new List<NoteGroupData>();
        List<NoteData> noteDataList = new List<NoteData>();

        bool parsingGroups = false;
        bool parsingNotes = false;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;

            if (line.StartsWith("groupIndex"))
            {
                parsingGroups = true;
                parsingNotes = false;
                continue;
            }

            if (line.StartsWith("timeToHit"))
            {
                parsingNotes = true;
                parsingGroups = false;
                continue;
            }

            string[] tokens = line.Split(',');

            if (parsingGroups && tokens.Length >= 6)
            {
                int groupIndex = int.Parse(tokens[0]);
                string name = tokens[1];
                float r = float.Parse(tokens[2]);
                float g = float.Parse(tokens[3]);
                float b = float.Parse(tokens[4]);
                float a = float.Parse(tokens[5]);
                groupDataList.Add(new NoteGroupData(groupIndex, name, r, g, b, a));
            }
            else if (parsingNotes && tokens.Length >= 3)
            {
                NoteData note = new NoteData();
                note.timeToHit = float.Parse(tokens[0]);
                note.lane = int.Parse(tokens[1]);
                note.groupIndex = int.Parse(tokens[2]);
                noteDataList.Add(note);
            }
        }

        notes = noteDataList;
        GroupManager.Instance.InitializeGroups(groupDataList);
        GroupUIManager.Instance.InitializeGroups(GroupManager.Instance.groups);
    }

    void SpawnNote(NoteData data, Transform spawnPoint)
    {
        // Get the NoteGroup by index
        NoteGroup group = GroupManager.Instance.groups[data.groupIndex];

        // Instantiate
        GameObject note = Instantiate(notePrefab, spawnPoint.position, spawnPoint.rotation);

        // Apply the group’s color:
        var sr = note.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = group.groupColor;

        // Tell the mover which group it is:
        var mover = note.GetComponent<NoteMover>();
        if (mover != null)
        {
            mover.moveSpeed = noteSpeed;
            mover.lane      = data.lane;
            mover.groupIndex = data.groupIndex;
        }
    }
}

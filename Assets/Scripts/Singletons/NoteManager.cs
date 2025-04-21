using UnityEngine;
using System.Collections.Generic;
using System.Globalization; // For NumberStyles and InvariantCulture

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }


    [Header("Per‑Group CSV files (in Resources/)")]
    [Tooltip("1_Notes_Bass.csv, …")]
    public List<string> noteCsvFiles;

    [Header("Spawn Settings")]
    public Transform[] laneSpawnPoints;
    public GameObject notePrefab;
    public float noteSpeed = 300f;

    [Header("Lane Settings")]
    public GameObject[] laneJudgementLines; // Set these in the Inspector (four lanes).
    public Transform judgmentLine;      // Reference to your judgment line.


    private Dictionary<int, List<NoteData>> groupNoteData;
    private int activeGroup = 0;
    private int spawnIndex = 0; 
    private float standardTravelTime;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadAllGroupNoteData(); // CSV file name without extension, in Resources folder.

        // Compute a standard travel time using the vertical distance from one lane's spawn point.
        float verticalDistance = Mathf.Abs(laneSpawnPoints[0].position.y - judgmentLine.position.y);
        standardTravelTime = verticalDistance / noteSpeed;
    }

    void Update()
    {
        double songTime = GameManager.Instance.GetSongTime();
        var notes = groupNoteData[activeGroup];

        while (spawnIndex < notes.Count &&
            notes[spawnIndex].timeToHit <= songTime + standardTravelTime)
        {
            SpawnNote(notes[spawnIndex]);
            spawnIndex++;

            ScoreManager.Instance.RegisterNote();
        }
    }

    void LoadAllGroupNoteData()
    {
        groupNoteData = new Dictionary<int, List<NoteData>>();
        for (int g = 0; g < noteCsvFiles.Count; g++)
        {
            var list = LoadNoteDataFromCSV(noteCsvFiles[g]);
            list.Sort((a,b) => a.timeToHit.CompareTo(b.timeToHit));
            groupNoteData[g] = list;

            GroupManager.Instance.groups[g].earliestNoteTime = list[0].timeToHit;
        }
    }

    List<NoteData> LoadNoteDataFromCSV(string resourcePath)
    {
        var txt = Resources.Load<TextAsset>(resourcePath);
        if (txt == null) return new List<NoteData>();

        var lines = txt.text.Split('\n');
        var result = new List<NoteData>();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith("#") || line.StartsWith("timeToHit"))
                continue;

            var t = line.Split(',');
            if (float.TryParse(t[0], out float time))
                result.Add(new NoteData { timeToHit = time });
        }
        return result;
    }


    void SpawnNote(NoteData data)
    {
        // pick a random lane
        int lane = Random.Range(0, laneSpawnPoints.Length);
        var spawnPoint = laneSpawnPoints[lane];

        // instantiate
        var go = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        // color it using the currently active group's color
        var col = GroupManager.Instance.groups[activeGroup].groupColor;
        var sr  = go.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = col;

        // configure mover
        var mover = go.GetComponent<NoteMover>();
        mover.moveSpeed  = noteSpeed;
        mover.lane       = lane;
        mover.groupIndex = activeGroup;
    }

    public void SwitchGroup(int g)
    {
        // 1) Destroy any existing notes
        // foreach (var n in FindObjectsByType<NoteMover>(FindObjectsSortMode.None))
        //     Destroy(n.gameObject);

        // 2) Change the active group
        activeGroup = g;

        // 3) Recompute spawnIndex based on current song time
        double songTime = GameManager.Instance.GetSongTime();              // how many seconds into the track we are

        var notes = groupNoteData[activeGroup];
        // find the first note whose timeToHit is still in the future
        int idx = notes.FindIndex(n => n.timeToHit >= songTime + standardTravelTime);
        spawnIndex = idx < 0 ? notes.Count : idx;            // if none left, set past the end

        // 4) (don’t touch your audio—let it keep playing)
    }

}

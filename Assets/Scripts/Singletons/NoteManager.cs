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
    public GameObject tapNotePrefab;
    public GameObject holdNotePrefab;
    public float noteSpeed = 300f;

    [Header("Lane Settings")]
    public GameObject[] laneJudgementLines; // Set these in the Inspector (four lanes).
    public Transform judgmentLine;      // Reference to your judgment line.

    [Header("Timing Thresholds (world units)")]
    public float hitThreshold     = 1f;
    public float greatThreshold   = 0.5f;
    public float perfectThreshold = 0.1f;


    private Dictionary<int, List<NoteData>> groupNoteData;
    public int activeGroup = 0;
    private int spawnIndex = 0; 
    private float standardTravelTime;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
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
            if (notes[spawnIndex].duration > 0f)
            {
                // Spawn a hold note
                SpawnNote(notes[spawnIndex], holdNotePrefab);
            }
            else
            {
                // Spawn a regular note
                SpawnNote(notes[spawnIndex], tapNotePrefab);
            }
            spawnIndex++;

            ScoreManager.Instance.RegisterNote();
        }
    }

    public void LoadAllGroupNoteData()
    {
        groupNoteData = new Dictionary<int, List<NoteData>>();
        for (int g = 0; g < noteCsvFiles.Count; g++)
        {
            var list = LoadNoteDataFromCSV(noteCsvFiles[g]);
            list.Sort((a,b) => a.timeToHit.CompareTo(b.timeToHit));
            groupNoteData[g] = list;

            GroupManager.Instance.groups[g].earliestNoteTime = list[0].timeToHit;
            GroupManager.Instance.UpdateEarliestGroup(list[0].timeToHit, g);

            GroupManager.Instance.groups[g].latestNoteTime = list[list.Count - 1].timeToHit;
            GroupManager.Instance.UpdateLatestGroup(list[list.Count - 1].timeToHit, g);
        }
    }

    private List<NoteData> LoadNoteDataFromCSV(string resourcePath)
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
            if (t.Length >= 3 &&
                float.TryParse(t[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float startTime) &&
                float.TryParse(t[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float endTime) &&
                int.TryParse(t[2], out int lane))
            {
                result.Add(new NoteData
                {
                    timeToHit = startTime,
                    lane = lane,
                    duration = endTime - startTime
                });
            }
        }
        return result;
    }


    void SpawnNote(NoteData data, GameObject notePrefab)
    {
        // pick a random lane
        var spawnPoint = laneSpawnPoints[data.lane];

        // instantiate
        var go = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        // color it using the currently active group's color
        var col = GroupManager.Instance.groups[activeGroup].groupColor;

        // configure mover
        var mover = go.GetComponent<NoteMover>();
        mover.Initialize(data, activeGroup, noteSpeed, col);

        // if disabled, grey it out
        if (GroupManager.Instance.groups[activeGroup].isShutDown) {
            mover.GrayOut();
        }

        
    }

    public void SwitchGroup(int g)
    {
        // 1) Destroy any existing notes
        // foreach (var n in FindObjectsByType<NoteMover>(FindObjectsSortMode.None))
        //     Destroy(n.gameObject);
        GroupUIManager.Instance.groupUIs[activeGroup].Deselect();
        

        NoteGroup group = GroupManager.Instance.groups[g];
        if (group.isShutDown) {
            SwitchGroupAlertUI.Instance.Show(group.groupName + " Shutdown", Color.red);
            return; // don't switch to a shut down group
        }

        // 2) Change the active group
        activeGroup = g;        

        // 3) Recompute spawnIndex based on current song time
        double songTime = GameManager.Instance.GetSongTime();              // how many seconds into the track we are

        var notes = groupNoteData[activeGroup];
        // find the first note whose timeToHit is still in the future
        int idx = notes.FindIndex(n => n.timeToHit >= songTime + standardTravelTime);
        spawnIndex = idx < 0 ? notes.Count : idx;            // if none left, set past the end        

        // 4) (don’t touch your audio—let it keep playing)

        // Show alert
        SwitchGroupAlertUI.Instance.Show(group.groupName, group.groupColor);

        // Update group UI
        GroupUIManager.Instance.groupUIs[activeGroup].Select();
    }

}

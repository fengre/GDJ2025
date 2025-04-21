using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class GroupManager : MonoBehaviour
{
    public static GroupManager Instance;

    [Header("Audio Mixer")]
    [Tooltip("Drag your MainMixer asset here")]
    [SerializeField] private AudioMixer mainMixer;
    [Tooltip("The exposed group name in your mixer")]
    [SerializeField] private string musicGroupName = "Music";
    private double startDspTime;
    
    // List of groups; you can pre-populate these via the Inspector or create them in code.
    public List<NoteGroup> groups = new List<NoteGroup>();

    // How much to change a group on a hit or miss.
    public float perfectHitIncrease = 5f;
    public float greatHitIncrease   = 3f;
    public float goodHitIncrease    = 2f;
    public float missDecrease       = 5f;

    public int maxAllowedShutdowns = 5;
    private int currentShutdowns = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeGroups(string csvPath)
    {
        groups.Clear();
        LoadGroupDataFromCSV(csvPath);
    }

    private void LoadGroupDataFromCSV(string filepath)
    {
        TextAsset file = Resources.Load<TextAsset>(filepath);
        string[] lines = file.text.Split('\n');

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("groupIndex"))
                continue;

            string[] tokens = line.Split(',');
            if (tokens.Length < 5) continue;

            int groupIndex = int.Parse(tokens[0]);
            string name = tokens[1];
            string hex = tokens[2];
            float decayRate = float.Parse(tokens[3]);
            string audioClipPath = tokens[4];
            string notesPath = tokens[5];

            NoteManager.Instance.noteCsvFiles.Add(notesPath);

            if (!hex.StartsWith("#")) hex = "#" + hex;
            Color color = Color.white;
            if (!ColorUtility.TryParseHtmlString(hex, out color))
            {
                Debug.LogWarning($"Invalid hex color on line: {hex}");
            }

            AudioSource source = GetAudioSource(name, audioClipPath);

            NoteGroup group = new NoteGroup
            {
                groupIndex = groupIndex,   // ✅ Set it here
                groupName = name,
                groupColor = color,
                groupDecayRate = decayRate,
                groupValue = 50f,
            };

            group.audioSource = source;
            groups.Add(group);
        }
    }

    private AudioSource GetAudioSource(string name, string audioClipPath)
    {
        // Create GameObject + AudioSource
        GameObject audioGO = new GameObject(name + "_AudioSource");
        AudioSource source = audioGO.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioClipPath);
        if (clip != null)
            source.clip = clip;
        else
            Debug.LogWarning($"Could not load AudioClip at path: Resources/Audio/{audioClipPath}");

        // assign it to the “Music” group on your main mixer
        var mixerGroups = mainMixer.FindMatchingGroups(musicGroupName);
        if (mixerGroups.Length > 0)
            source.outputAudioMixerGroup = mixerGroups[0];
        else
            Debug.LogWarning($"Mixer group '{musicGroupName}' not found in {mainMixer.name}");

        source.playOnAwake = false;
        return source;
    }

    public double PlayAllGroupAudio()
    {
        // record the upcoming DSP time to start at
        startDspTime = AudioSettings.dspTime + 0.1; // small delay to ensure setup

        double songLength = 0;

        foreach (var g in groups)
        {
            if (g.audioSource.clip != null)
            {
                g.audioSource.PlayScheduled(startDspTime);
                songLength = g.audioSource.clip.length;
            }
        }

        return songLength;
    }

    public void PauseAllGroupAudio()
    {
        foreach (var group in groups)
        {
            if (group.audioSource.isPlaying)
            {
                group.audioSource.Pause();
            }
        }
    }

    public void ResumeAllGroupAudio()
    {
        foreach (var group in groups)
        {
            if (!group.audioSource.isPlaying && group.audioSource.time > 0f)
            {
                group.audioSource.UnPause();
            }
        }
    }



    private void Update()
    {
        foreach (var group in groups)
        {
            if (group.isShutDown)
                continue;

            // Regular decay
            float decay = group.groupDecayRate * Time.deltaTime;
            ChangeGroupValue(group.groupIndex, -decay);
            GroupUIManager.Instance.UpdateGroupValue(group.groupIndex, GroupManager.Instance.GetGroupValue(group.groupIndex));

            // Check red zone
            if (group.groupValue < ScoreManager.Instance.okayMin || group.groupValue > ScoreManager.Instance.okayMax)
            {
                group.redZoneTimer += Time.deltaTime;

                if (group.redZoneTimer >= group.maxRedZoneTime)
                {
                    ShutdownGroup(group);
                }
            }
            else
            {
                group.redZoneTimer = 0f;
            }
        }
    }

    private void ShutdownGroup(NoteGroup group)
    {
        group.isShutDown = true;
        group.audioSource.Stop(); // Turn off the music
        currentShutdowns++;

        Debug.Log($"{group.groupName} has shut down due to being in the red zone too long!");

        if (currentShutdowns >= maxAllowedShutdowns)
        {
            GameManager.Instance.EndGame("Too many groups shut down!");
        }
    }

    /// <summary>
    /// Adjusts the group value for the group with the specified color.
    /// </summary>
    public void ChangeGroupValue(int groupIndex, float delta)
    {
        groups[groupIndex].Adjust(delta);
    }

    /// <summary>
    /// Compares two Colors using a tolerance.
    /// </summary>
    private bool ColorsEqual(Color c1, Color c2)
    {
        return Mathf.Approximately(c1.r, c2.r) &&
               Mathf.Approximately(c1.g, c2.g) &&
               Mathf.Approximately(c1.b, c2.b) &&
               Mathf.Approximately(c1.a, c2.a);
    }

    public float GetGroupValue(int groupIndex)
    {
        NoteGroup group = groups.Find(g => g.groupIndex == groupIndex);
        return group != null ? group.groupValue : 0f;
    }
}

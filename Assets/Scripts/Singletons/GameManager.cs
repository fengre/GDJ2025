using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Audio Settings")]
    // public AudioSource musicSource;
    private double songStartDspTime;
    private bool isPaused = false;
    private double pauseStartTime;
    private double totalPauseDuration = 0.0;
    private double songLength;

    private bool hasEnded = false;

    private void Awake()
    {
        //GroupManager.Instance.InitializeGroups("1_Groups");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        var song = LevelManager.Instance.currentSong;
        if (song == null)
        {
            Debug.LogError("No song selected!");
            return;
        }

        // ✅ Initialize group data using the song's groupCSVName
        GroupManager.Instance.InitializeGroups(song.groupCSVName);
    }

    private void Start()
    {
        // musicSource.Play();
        songLength = GroupManager.Instance.PlayAllGroupAudio();
        songStartDspTime = AudioSettings.dspTime;

        ScoreManager.Instance.ResetValues();
        NoteManager.Instance.LoadAllGroupNoteData();
        GroupUIManager.Instance.InitializeGroups(GroupManager.Instance.groups);
        NoteManager.Instance.SwitchGroup(GroupManager.Instance.earliestGroup);

        totalPauseDuration = 0.0;
        isPaused = false;
    }

    public void PauseGame()
    {
        if (isPaused) return;
        pauseStartTime = AudioSettings.dspTime;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        totalPauseDuration += AudioSettings.dspTime - pauseStartTime;
        isPaused = false;
    }

    private void Update()
    {
        if (!hasEnded && GetSongTime() >= songLength)
        {
            hasEnded = true;
            EndGame("Song finished");
        }
    }

    public double GetSongTime()
    {
        if (isPaused)
        {
            return pauseStartTime - songStartDspTime - totalPauseDuration;
        }
        else
        {
            return AudioSettings.dspTime - songStartDspTime - totalPauseDuration;
        }
    }


    public void EndGame(string reason)
    {
        Debug.Log("Game Over: " + reason);
        int finalScore = FindFirstObjectByType<ScoreManager>().GetTotalScore();
        PlayerPrefs.SetInt("FinalScore", finalScore);
        LeaderboardManager.SaveScore(finalScore);
        SceneManager.LoadScene("End");
    }

}

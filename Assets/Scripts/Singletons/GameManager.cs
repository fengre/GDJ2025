using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Audio Settings")]
    // public AudioSource musicSource;
    private double songStartDspTime;
    private double songLength;

    private bool hasEnded = false;

    private void Awake()
    {
        GroupManager.Instance.InitializeGroups("1_Groups");
    }

    private void Start()
    {
        // musicSource.Play();
        ScoreManager.Instance.ResetValues();
        
        songLength = GroupManager.Instance.PlayAllGroupAudio();
        GroupUIManager.Instance.InitializeGroups(GroupManager.Instance.groups);

        songStartDspTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (!hasEnded && AudioSettings.dspTime - songStartDspTime >= 25f)
        {
            hasEnded = true;
            EndGame("Song finished");
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource musicSource;
    private double songStartDspTime;
    private double songLength;

    private bool hasEnded = false;

    void Start()
    {
        musicSource.Play();
        ScoreManager.Instance.ResetValues();
        songStartDspTime = AudioSettings.dspTime;
        songLength = musicSource.clip.length;
    }

    void Update()
    {
        if (!hasEnded && (AudioSettings.dspTime - songStartDspTime >= songLength || musicSource.time > 25f) && musicSource.time > 0f)
        {
            hasEnded = true;
            EndGame();
        }
    }

    void EndGame()
    {
        int finalScore = FindFirstObjectByType<ScoreManager>().GetTotalScore();
        PlayerPrefs.SetInt("FinalScore", finalScore);
        LeaderboardManager.SaveScore(finalScore);
        SceneManager.LoadScene("End");
    }

}

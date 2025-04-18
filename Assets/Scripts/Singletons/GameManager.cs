using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource musicSource;

    private bool hasEnded = false;

    void Start()
    {
        musicSource.Play();
    }

    void Update()
    {
        if (!hasEnded && (!musicSource.isPlaying || musicSource.time > 25f) && musicSource.time > 0f)
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public SongData currentSong;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only allow LevelManager in Levels, Gameplay, and End scenes
        if (scene.name != "Levels" && scene.name != "Gameplay" && scene.name != "End")
        {
            Destroy(gameObject);
        }
    }

    public void SetSong(SongData song)
    {
        currentSong = song;
    }

    public void ClearSong()
    {
        currentSong = null;
    }
}

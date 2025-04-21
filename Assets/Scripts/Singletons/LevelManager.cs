using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public SongData currentSong;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else { Instance = this; DontDestroyOnLoad(gameObject); }
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

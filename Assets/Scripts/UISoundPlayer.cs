using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    public static UISoundPlayer Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip scorePageSound;
    public AudioClip levelSelectSound;
    public AudioClip levelUnselectSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional, if used across scenes
        }
        else Destroy(gameObject);
    }

    public void PlayHover() => Play(hoverSound);
    public void PlayClick() => Play(clickSound);
    public void PlayWin() => Play(winSound);
    public void PlayLose() => Play(loseSound);
    public void PlayScorePage() => Play(scorePageSound);
    public void PlayLevelSelect() => Play(levelSelectSound);
    public void PlayLevelUnselect() => Play(levelUnselectSound);

    public void Play(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    public void Play(AudioClip clip, float volume)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip, volume);
    }
}

using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanelManager : MonoBehaviour
{
    public static SettingsPanelManager Instance { get; private set; }

    public AudioMixer audioMixer;

    public float musicVolume = 80f;
    public float sfxVolume = 80f;

    private const string MUSIC_PARAM = "MusicVol";
    private const string SFX_PARAM = "SFXVol";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); //persist

        ApplyVolume(MUSIC_PARAM, musicVolume);
        ApplyVolume(SFX_PARAM, sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyVolume(MUSIC_PARAM, value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        ApplyVolume(SFX_PARAM, value);
    }

    private void ApplyVolume(string parameter, float value)
    {
        audioMixer.SetFloat(parameter, value);
    }
}

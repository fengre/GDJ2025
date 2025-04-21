using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanelManager : MonoBehaviour
{
    public static SettingsPanelManager Instance { get; private set; }

    public AudioMixer audioMixer;

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

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
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        //float dB = Mathf.Lerp(-80f, 0f, value);  
        audioMixer.SetFloat(parameter, dB);
    }

}

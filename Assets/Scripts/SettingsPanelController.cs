// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Audio;

// public class SettingsPanelController : MonoBehaviour
// {
//     public AudioMixer audioMixer;
//     public Slider musicSlider;
//     public Slider sfxSlider;

//     private const string MUSIC_KEY = "MusicVol";
//     private const string SFX_KEY = "SFXVol";

//     void Start()
//     {
//         Debug.Log("musicSlider is null? " + (musicSlider == null));
//         Debug.Log("sfxSlider is null? " + (sfxSlider == null));
//         Debug.Log("audioMixer is null? " + (audioMixer == null));

//         // load data or default to full volume
//         float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 80f);
//         float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 80f);

//         //load to slider and audio mixer volume
//         musicSlider.value = musicVol;
//         sfxSlider.value = sfxVol;
//         audioMixer.SetFloat(MUSIC_KEY, musicVol);
//         audioMixer.SetFloat(SFX_KEY, sfxVol);

//         //add listeners
//         musicSlider.onValueChanged.AddListener((v) => UpdateVolume(MUSIC_KEY, v));
//         sfxSlider.onValueChanged.AddListener((v) => UpdateVolume(SFX_KEY, v));
//     }

//     void UpdateVolume(string parameter, float value)
//     {
//         audioMixer.SetFloat(parameter, value);
//         PlayerPrefs.SetFloat(parameter, value);
//     }

//     public void CloseSettingsPanel()
//     {
//         gameObject.SetActive(false);
//     }
// }

using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (SettingsPanelManager.Instance == null)
        {
            Debug.LogError("SettingsPanelManager.Instance is null — did you forget to add it to the scene?");
            return;
        }
        if(musicSlider == null){
            Debug.Log("null music slifer");
        }
        // Load from SettingsPanelManager
        musicSlider.value = SettingsPanelManager.Instance.musicVolume;
        sfxSlider.value = SettingsPanelManager.Instance.sfxVolume;

        // Hook up sliders
        musicSlider.onValueChanged.AddListener(SettingsPanelManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SettingsPanelManager.Instance.SetSFXVolume);
    }

    public void CloseSettingsPanel()
    {
        gameObject.SetActive(false);
    }
}


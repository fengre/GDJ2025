using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
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


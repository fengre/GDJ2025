using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bpmText;
    public Image[] starImages;

    [Header("Star Sprites")]
    public Sprite filledStar;
    public Sprite emptyStar;

    [HideInInspector] public SongData assignedSong;

    public void Setup(SongData song)
    {
        assignedSong = song;
        titleText.text = song.songTitle;
        bpmText.text = $"BPM: {song.bpm}";

        // Update star icons
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < song.difficulty)
                starImages[i].sprite = filledStar;
            else
                starImages[i].sprite = emptyStar;
        }
    }

    public void OnClick()
    {
        LevelManager.Instance.SetSong(assignedSong);
        //SceneManager.LoadScene("GameplayScene");
    }
}

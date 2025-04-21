using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bpmText;
    public Image backgroundImage;
    public Image[] starImages;

     [Header("Star Sprites")]
    public Sprite filledStar;
    public Sprite emptyStar;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.85f, 0.6f); // light gold or highlight

    [HideInInspector] public SongData assignedSong;
    private bool isSelected = false;

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

        UpdateVisual();
    }

    public void OnClick()
    {
        if (!isSelected)
        {
            foreach (var item in Object.FindObjectsByType<LevelItemUI>(FindObjectsSortMode.None))
                item.Deselect();

            isSelected = true;
            LevelManager.Instance.SetSong(assignedSong);
        }
        else
        {
            isSelected = false;
            LevelManager.Instance.ClearSong();
        }

        UpdateVisual();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        backgroundImage.color = isSelected ? selectedColor : normalColor;
    }

    
}

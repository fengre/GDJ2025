using UnityEngine;
using UnityEngine.UI;

public class SongProgressBar : MonoBehaviour
{
    public Slider progressBar;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        double songTime = GameManager.Instance.GetSongTime();
        double songLength = GameManager.Instance.GetSongLength(); // You'll need to expose this

        if (songLength > 0)
        {
            progressBar.value = (float)(songTime / songLength);
        }
    }
}

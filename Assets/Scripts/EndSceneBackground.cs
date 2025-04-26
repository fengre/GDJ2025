using UnityEngine;
using UnityEngine.UI;

public class EndSceneBackground : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite finishedSongBackground;
    public Sprite interruptedSongBackground;

    void Start()
    {
        bool finished = MusicTracker.songFinished;

        backgroundImage.sprite = finished
            ? finishedSongBackground
            : interruptedSongBackground;

        // Play corresponding sound
        if (UISoundPlayer.Instance != null)
        {
            if (finished)
                UISoundPlayer.Instance.PlayWin();
            else
                UISoundPlayer.Instance.PlayLose();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class EndSceneBackground : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite finishedSongBackground;
    public Sprite interruptedSongBackground;

    void Start()
    {
        backgroundImage.sprite = MusicTracker.songFinished
            ? finishedSongBackground
            : interruptedSongBackground;
    }
}

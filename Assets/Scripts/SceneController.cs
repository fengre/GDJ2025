using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReturnToStartMenu()
    {
        SceneManager.LoadScene("StartMenu"); // loads the first scene in Build Settings
    }

    //return to levels menu method

    public void GoToLevelsMenu()
    {
        SceneManager.LoadScene("Levels");
    }

    public void GoToGame()
    {
        if (LevelManager.Instance.currentSong != null)
        {
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            Debug.LogWarning("No song selected!");
            // Optional: Show UI feedback (like a popup or shake effect)
        }
    }
}

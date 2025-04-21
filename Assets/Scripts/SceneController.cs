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
        SceneManager.LoadScene("Levels"); // loads the first scene in Build Settings
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Gameplay"); // loads the first scene in Build Settings
    }
}

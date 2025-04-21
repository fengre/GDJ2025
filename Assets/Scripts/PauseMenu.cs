using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    public void Resume()
    {
        // If trying to resume and settings panel is still open -> deny
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            Debug.Log("Can't resume yet, settings panel is still open.");
            return;
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GroupManager.Instance.ResumeAllGroupAudio();
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GroupManager.Instance.PauseAllGroupAudio();
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..");
        Application.Quit();
    }
}

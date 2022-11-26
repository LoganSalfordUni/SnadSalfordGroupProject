using ScriptsSJD;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuScene;
    public GameObject pauseMenu;
    private static BackgroundMusic _backgroundMusic;

    [FormerlySerializedAs("Paused")] public bool paused;
    
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                paused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;

            }
        }
        
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        BackgroundMusic._backgroundMusic.GetComponent<AudioSource>().Play();
    }
}

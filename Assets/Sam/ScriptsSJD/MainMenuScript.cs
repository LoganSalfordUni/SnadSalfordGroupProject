using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    /* Hello im Script :)
    beep bop
    */
    public void StartButton()
    {
        SceneManager.LoadScene("MainLevel");
        Debug.Log("Loading Game!");
        // Change "Insert a name here" to the name of the next level you want / Cutscene etc
        // Debug log is just for knowledge that the game is running!
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Successfully Quit");
        // Simple 1 line of code ;)
        // Debug entails the game has successfully quit!
    }
}

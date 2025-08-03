using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Sound buttonPressSound;

    public void StartGame()
    {
        //AudioManager.instance.PlaySound(buttonPressSound, transform);
        GameManager.instance.StartGame();
        SceneManager.LoadScene("Movement");
    }

    public void OpenSettings()
    {
        //AudioManager.instance.PlaySound(buttonPressSound, transform);
        SceneManager.LoadScene("Settings");
    }

    public void CloseSettings()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PAUSED)
        {
            GameManager.instance.TogglePause();
        }
        else if (GameManager.instance.currentGameState == GameManager.GameState.MAIN_MENU)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void CloseTutorial ()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

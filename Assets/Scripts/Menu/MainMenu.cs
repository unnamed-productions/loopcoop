using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Sound buttonPressSound;

    [SerializeField]
    Sound mainMenuMusic;

    public void Awake()
    {
        //AudioManager.instance.PlayMusic(mainMenuMusic.clip);
    }

    public void StartGame()
    {
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        GameManager.instance.StartGame();
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        SceneManager.LoadScene("Settings");
    }

    public void CloseSettings()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        if (GameManager.instance.currentGameState == GameManager.GameState.PAUSED)
        {
            GameManager.instance.TogglePause();
        }
        else if (GameManager.instance.currentGameState == GameManager.GameState.MAIN_MENU)
        {
            GameManager.instance.ToggleBackToMainMenu();
        }
    }

    public void OpenTutorial()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        SceneManager.LoadScene("Tutorial");
    }

    public void CloseTutorial ()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        SceneManager.LoadScene("Main Menu");
        GameManager.instance.ToggleBackToMainMenu();
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        Application.Quit();
    }
}

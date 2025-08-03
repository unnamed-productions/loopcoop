using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
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
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

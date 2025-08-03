using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string currentMainScene = "Health";

    public enum GameState
    {
        MAIN_MENU,
        PAUSED,
        PLAYING,
        GAME_OVER,

    }

    public GameState currentGameState;

    [SerializeField]
    private PlayerCombat player;
    [SerializeField]
    private GameObject pauseScreen;

    public int score;

    void Start()
    {
        currentGameState = GameState.MAIN_MENU;
        instance = this;
    }

    public void StartGame()
    {
        currentGameState = GameState.PLAYING;
        SceneManager.LoadScene(currentMainScene);
        score = 0;
    }

    public void TogglePause()
    {
        if (currentGameState == GameState.PAUSED)
        {
            currentGameState = GameState.PAUSED;
            pauseScreen.SetActive(true);
        }
        else
        {
            currentGameState = GameState.PLAYING;
            pauseScreen.SetActive(false);
        }
    }
    
    public void ToggleGameOver()
    {
        currentGameState = GameState.GAME_OVER;
        SceneManager.LoadScene("GameOver");
    }

    public void ToggleRestart()
    {
        SceneManager.LoadScene(currentMainScene);
        currentGameState = GameState.PLAYING;
        score = 0;
    }

    public void ToggleBackToMainMenu()
    {
        AudioManager.instance.StopMusic();
        currentGameState = GameState.MAIN_MENU;
        SceneManager.LoadScene("Main Menu");
    }

    public PlayerCombat GetPlayer()
    {
        return player;
    }
}
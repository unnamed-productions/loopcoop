using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        MAIN_MENU,
        PAUSED,
        PLAYING,
        GAME_OVER,

    }

    public GameState currentGameState;

    public int score;
    public int health;

    [SerializeField]
    private PlayerCombat player;
    [SerializeField]
    private GameObject pauseScreen;

    void Start()
    {
        currentGameState = GameState.MAIN_MENU;
        instance = this;
    }

    public void StartGame()
    {
        currentGameState = GameState.PLAYING;
        score = 0;
        health = 100; 
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
        SceneManager.LoadScene("Movement");
        currentGameState = GameState.PLAYING;
        score = 0;
        health = 100; 
    }

    public void ToggleBackToMainMenu()
    {
        currentGameState = GameState.MAIN_MENU;
        SceneManager.LoadScene("Main Menu");
    }

    public PlayerCombat GetPlayer()
    {
        return player;
    }
}
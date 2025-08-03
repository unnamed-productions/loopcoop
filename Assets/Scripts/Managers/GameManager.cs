using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverScreen;

    public enum GameState
    {
        MAIN_MENU,
        PAUSED,
        PLAYING,
        GAME_OVER,
        TUTORIAL,

    }

    public GameState currentGameState;

    public int score;
    public int health;

    [SerializeField]
    private TopDownPlayer player;

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

    public void StartTutorial()
    {
        currentGameState = GameState.TUTORIAL;
        score = 0;
        health = 100; 
    }

    public void ToggleTutorialOver()
    {
        if (currentGameState == GameState.TUTORIAL)
        {
            currentGameState = GameState.PLAYING;
        }
        else
        {
            currentGameState = GameState.TUTORIAL;
        }
    }

    public void TogglePause()
    {
        if (currentGameState == GameState.PAUSED)
        {
            currentGameState = GameState.PAUSED;
        }
        else
        {
            currentGameState = GameState.PLAYING;
        }
    }

    public void ToggleGameOver()
    {
        currentGameState = GameState.GAME_OVER;
        gameOverScreen.SetActive(true);
    }

    public void ToggleRestart()
    {
        currentGameState = GameState.PLAYING;
        gameOverScreen.SetActive(false);
        score = 0;
        health = 100; 
    }

    public TopDownPlayer GetPlayer()
    {
        return player;
    }
}
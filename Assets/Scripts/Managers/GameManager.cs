using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string currentMainScene = "Health";

    public int lastScore = 0;

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


    [SerializeField]
    public TextMeshProUGUI scoreText;

    [SerializeField]
    public TextMeshProUGUI multText;
    public int gameScore = 0;
    float gameMult = 1;
    private float ogJitter;

    void Start()
    {
        currentGameState = GameState.MAIN_MENU;
        instance = this;
        ogJitter = multText.GetComponent<Jitter>().maxDisplacement;
    }

    public void StartGame()
    {
        currentGameState = GameState.PLAYING;
        SceneManager.LoadScene(currentMainScene);
        gameScore = 0;
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
            lastScore = gameScore;
            gameScore = 0;
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
        lastScore = 0;
        gameScore = 0;
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

    public void addScore(int score)
    {
        gameScore += (int)(gameMult * score);
        scoreText.text = "Score: " + gameScore;
        player.GetComponentInChildren<ScorePopup>().PopUpScore(score);
    }

    public void AddMult(float multToAdd)
    {
        gameMult *= 1 + multToAdd;
        multText.text = "Multiplier: " + gameMult;
        multText.GetComponent<Jitter>().maxDisplacement = ogJitter * gameMult;

    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        MAIN_MENU,
        PAUSED,
        PLAYING
    }

    public GameState currentGameState;

    public int score;

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

    public TopDownPlayer GetPlayer()
    {
        return player;
    }
}
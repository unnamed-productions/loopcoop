using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool paused = false;
    [SerializeField]
    OptionsMenu o;

    [SerializeField]
    Sound buttonPressSound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.pauseJustPressed) {
            TogglePause();
        }
    }

    public void OpenPauseMenu()
    {
        if (paused) {
            return;
        }
        paused = true;
        o.enabled = true;
        Time.timeScale = 0;
        o.setUp();
        GetComponentInChildren<Image>().enabled = false;
        GetComponent<Button>().enabled = false;
    }

    public void ClosePauseMenu() {
        if (!paused) {
            return;
        }
        o.closeMenu();
        paused = false;
        o.enabled = false;
        Time.timeScale = 1;
        GetComponentInChildren<Image>().enabled = true;
        GetComponent<Button>().enabled = true;
    }

    public void TogglePause()
    {
        AudioManager.instance.PlaySound(buttonPressSound, transform);
        if (!paused)
        {
            if (!(GameManager.instance.currentGameState == GameManager.GameState.PAUSED)) {
                //Dont pause on the tutorial
                OpenPauseMenu();
            }
        }
        else
        {
            ClosePauseMenu();
        }
        GameManager.instance.TogglePause();
    }
}

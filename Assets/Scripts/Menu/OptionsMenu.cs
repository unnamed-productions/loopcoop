using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

//Have to shout out the brackeys influence
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Color baseColor;
    [SerializeField] Color greyColor;
    [SerializeField] GameObject FullScreenButton;

    [Header("Volume")]
    public AudioMixer music_mxr;
    public AudioMixer sfx_mxr;
    [SerializeField] GameObject MusicButton;
    [SerializeField] GameObject MusicSlider;
    [SerializeField] GameObject SFXButton;
    [SerializeField] GameObject SFXSlider;

    [SerializeField] GameObject SFXMuter; //The button to mute sfx
    [SerializeField] GameObject SFXSliderBG;
    [SerializeField] GameObject SFXSliderHandle;

    [SerializeField] GameObject MusicMuter; //The button to mute music
    [SerializeField] GameObject MusicSliderBG;
    [SerializeField] GameObject MusicSliderHandle;

    [SerializeField] GameObject pauseMenu;

    #region inputs
    private bool getSelectInput()
    {
        return InputManager.instance.workJustPressed;
    }
    #endregion

    public void toggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen)
        {
            FullScreenButton.GetComponent<Image>().color = baseColor;
        }
        else
        {
            FullScreenButton.GetComponent<Image>().color = greyColor;
        }
    }

    public void SetMusicVolume(float volume)
    {
        music_mxr.SetFloat("Volume", volume);
    }

    public void toggleMuteMusic()
    {
        if (AudioManager.instance._MuteMusic)
        {
            //Unmuting
            MusicMuter.GetComponent<Image>().color = baseColor;
            MusicSlider.SetActive(true);

            AudioManager.instance.unMuteMusic();
        }
        else
        {
            //Muting
            MusicMuter.GetComponent<Image>().color = greyColor;
            MusicSlider.SetActive(false);

            AudioManager.instance.muteMusic();
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfx_mxr.SetFloat("Volume", volume);
    }

    public void toggleMuteSFX()
    {
        if (AudioManager.instance._MuteSFX)
        {
            print("unmuting sfx");
            //Unmuting
            SFXMuter.GetComponent<Image>().color = baseColor;
            SFXSlider.SetActive(true);

            AudioManager.instance.unMuteSFX();
        }
        else
        {
            print("muting sfx");
            //Muting
            SFXMuter.GetComponent<Image>().color = greyColor;
            SFXSlider.SetActive(false);

            AudioManager.instance.muteSFX();
        }
    }

    private void Update()
    {
        if ((EventSystem.current.currentSelectedGameObject &&
            EventSystem.current.currentSelectedGameObject == SFXSlider)
            && getSelectInput())
        {
            EventSystem.current.SetSelectedGameObject(SFXButton);
        }
        else if ((EventSystem.current.currentSelectedGameObject &&
            EventSystem.current.currentSelectedGameObject == MusicSlider)
            && getSelectInput())
        {
            EventSystem.current.SetSelectedGameObject(MusicButton);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        setUp();
        closeMenu();
    }

    private void OnDestroy()
    {
        //Uncomment if we do save data
        //GameManager.instance.savePlayer(); //Save changes we made to settings
    }

    public void setUp() {
        pauseMenu.SetActive(true);
        //Load in values to sliders
        float musicVolume;
        music_mxr.GetFloat("Volume", out musicVolume);
        MusicSlider.GetComponent<Slider>().value = musicVolume;

        float sfxVolume;
        sfx_mxr.GetFloat("Volume", out sfxVolume);
        SFXSlider.GetComponent<Slider>().value = sfxVolume;

        //Load in button stuff
        //Only need to use half of if statements because in these cases the other side of the if
        // would create the default state of the assets
        if (!Screen.fullScreen)
        {
            FullScreenButton.GetComponent<Image>().color = greyColor;
        }
        if (AudioManager.instance._MuteMusic)
        {
            MusicMuter.GetComponent<Image>().color = greyColor;
            MusicSlider.SetActive(true);
        }
        if (AudioManager.instance._MuteSFX)
        {
            SFXMuter.GetComponent<Image>().color = greyColor;
            SFXSlider.SetActive(false);
        }
    }

    public void closeMenu() {
        pauseMenu.SetActive(false);
    }
}


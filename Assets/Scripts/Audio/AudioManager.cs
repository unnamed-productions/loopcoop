using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public bool _MuteSFX { get; private set; }
    public bool _MuteMusic { get; private set; }

    [Header("Music control")]
    [SerializeField] AudioSource music;

    [SerializeField]
    private AudioSource sfxGameObject;

    [Header("Volume")]
    public AudioMixer music_mxr;
    public AudioMixer sfx_mxr;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioSource PlaySound(Sound s, Transform t)
    {
        //print("Playing sound " + s.clip.name);
        AudioSource audioSource = Instantiate(sfxGameObject, t.position, Quaternion.identity);
        if (_MuteSFX)
        {
            audioSource.volume = 0;
        }
        s.initializeSource(audioSource);
        return (audioSource);
    }


    //Similar to the above method, plays a sound, but uses a slightly more runtime
    //  expensive coroutine and supports unscaled time.
    //  This is useful for playing sounds on pause menus and other places where time slows / stops
    public AudioSource PlaySoundUnscaledTime(Sound s, Transform t)
    {
        if (!_MuteSFX)
        {
            AudioSource audioSource = Instantiate(sfxGameObject, t.position, Quaternion.identity);
            s.initializeSource(audioSource);
            StartCoroutine(DestroyAfterUnscaledDelay(audioSource.gameObject, s.clip.length));
            return (audioSource);
        }
        else
        {
            return null;
        }
    }
    private IEnumerator DestroyAfterUnscaledDelay(GameObject g, float delay)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + delay)
        {
            yield return null; // Wait for the next frame
        }
        UnityEngine.Object.Destroy(g);
    }

    public void PlayMusic(AudioClip musicToPlay, bool loop, float volume=0.5f)
    {
        music.clip = musicToPlay;
        music.loop = loop;
        music.volume = volume;
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void PauseMusic()
    {
        music.mute = true;
    }

    public void UnPauseMusic()
    {
        music.mute = false;
    }

    public AudioClip getCurrentMusic()
    {
        return music.clip;
    }

    public void playRandomSound(List<Sound> sounds, Transform t)
    {
        //print("Playing sounds");
        Sound toPlay = sounds[UnityEngine.Random.Range(0, sounds.Count)];
        PlaySound(toPlay, t);
    }

    public void muteSFX()
    {
        _MuteSFX = true;
    }
    public void unMuteSFX()
    {
        _MuteSFX = false;
    }

    public void toggleSFXMute()
    {
        if (_MuteSFX)
        {
            unMuteSFX();
        }
        else
        {
            muteSFX();
        }
    }

    public void muteMusic()
    {
        _MuteMusic = true;
    }
    public void unMuteMusic()
    {
        _MuteMusic = false;
    }

    public void toggleMusicMute()
    {
        if (_MuteMusic)
        {
            unMuteMusic();
        }
        else
        {
            muteMusic();
        }
    }

    private void Update()
    {

    }

    /**
     *Uncomment the below if we end up using save data
     */

    //public OptionsData getOptionsData()
    //{
    //    float sfxVol;
    //    float musicVol;
    //    sfx_mxr.GetFloat("Volume", out sfxVol);
    //    music_mxr.GetFloat("Volume", out musicVol);
    //    return new OptionsData(
    //            _MuteMusic,
    //            _MuteSFX,
    //            musicVol,
    //            sfxVol,
    //            Screen.fullScreen
    //        );
    //}

    //public void calibrateOptions(OptionsData o)
    //{
    //    if (o.muteMusic)
    //    {
    //        muteMusic();
    //    }
    //    if (o.muteSFX)
    //    {
    //        muteSFX();
    //    }
    //    music_mxr.SetFloat("Volume", o.musicVolume);
    //    sfx_mxr.SetFloat("Volume", o.SFXVolume);
    //    Screen.fullScreen = o.fullScreen;
    //}
}


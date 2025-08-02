using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class Sound
{

    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 0.5f;
    [Range(-3, 3)]
    public float pitch = 1;

    public bool loop;
    //True if we want the sound effect to loop

    public bool useProximity;
    //Set to true for sfx which have their volume affected by proximity

    public void initializeSource(AudioSource a)
    {
        a.clip = clip;
        a.volume = volume;
        a.pitch = pitch;
        a.loop = loop;
        if (useProximity)
        {
            a.spatialBlend = 1;
        }
        else
        {
            a.spatialBlend = 0;
            //Currently no support for in-between values. Maybe later.
        }
        a.Play();
        UnityEngine.Object.Destroy(a.gameObject, clip.length + .1f); //Adding .1 helps sounds that loop not clip
    }
}


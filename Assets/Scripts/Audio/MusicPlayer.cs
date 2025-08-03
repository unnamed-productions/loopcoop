using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip a;

    [SerializeField]
    Sound s;

    [Range(0, 1)]
    public float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        if (a) {
            AudioManager.instance.PlayMusic(a, true, musicVolume);
        }
        if (s.clip) {
            AudioManager.instance.PlaySound(s, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

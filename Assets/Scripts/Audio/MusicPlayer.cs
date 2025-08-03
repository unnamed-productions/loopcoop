using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip a;

    [SerializeField]
    Sound s;

    // Start is called before the first frame update
    void Awake()
    {
        if (a) {
            AudioManager.instance.StopMusic();
            AudioManager.instance.PlayMusic(a);
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

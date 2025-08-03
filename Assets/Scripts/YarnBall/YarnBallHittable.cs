using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YarnBallHittable : MonoBehaviour
{
    [SerializeField]
    CameraShake camShake;
    [SerializeField]
    float cameraShakeDur;
    [SerializeField]
    float cameraShakeIntensity;

    void Start()
    {
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    [SerializeField] public UnityEvent onHit;

    public void hitMe(float intensity)
    {
        camShake.ShakeCamera(cameraShakeDur, intensity);
        onHit.Invoke();
    }

    public void hitMe()
    {
        hitMe(cameraShakeIntensity);
    }

    public void SampleEvent()
    {
        print("My name is " + gameObject.name + "\nOuuughhh");
    }
}

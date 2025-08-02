using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBallHittable : MonoBehaviour
{
    [SerializeField]
    CameraShake camShake;
    [SerializeField]
    float cameraShakeDur;
    [SerializeField]
    float cameraShakeIntensity;

    public void hitMe(float intensity)
    {
        print("Should be shaking");
        camShake.ShakeCamera(cameraShakeDur, intensity);
    }

    public void hitMe()
    {
        hitMe(cameraShakeIntensity);
    }
}

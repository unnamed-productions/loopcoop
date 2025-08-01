using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Support for a method that starts shaking 
    private bool isShaking = false;
    //How much the camera shakes. For context, .05 is kind of a lot, .01 is not a lot.
    private float magnitude;
    private bool wasShaking = false; //Used to reset after we are done

    private Vector3 originalCameraOffset;
    [SerializeField]
    GameObject player;

    private void Start()
    {
        originalCameraOffset = transform.localPosition;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (enabled) {
            StartCoroutine(Shake(duration, magnitude));
        }
    }

    private IEnumerator Shake(float duration, float mgnitude)
    {
        //Debug.Log("Shaking camera for " + duration);
        //print("Camera local position " + originalPos);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * mgnitude;
            float y = Random.Range(-1f, 1f) * mgnitude;

            Vector3 originalPos = GetNeutralPos();

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = GetNeutralPos();
    }

    public void setShakingTrue(float mgnitude)
    {
        //Debug.Log("Set shaking true with mag " + mgnitude);
        isShaking = true;
        magnitude = mgnitude;
        wasShaking = true;
    }
    public void setShakingFalse()
    {
        isShaking = false;
        magnitude = 0;
    }

    private void Update()
    {
        if (isShaking)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Vector3 originalPos = GetNeutralPos();

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
        }
        else if (wasShaking)
        {
            wasShaking = false;
            transform.localPosition = GetNeutralPos();
        }
    }

    private Vector3 GetNeutralPos() {
        return originalCameraOffset;
    }
}

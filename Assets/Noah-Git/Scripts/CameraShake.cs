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

    [SerializeField]
    GameObject player;

    public void ShakeCamera(float duration, float mgnitude)
    {
        if (enabled)
        {
            StartCoroutine(Shake(duration, mgnitude));
        }
    }

    private IEnumerator Shake(float duration, float mgnitude)
    {
        //Debug.Log("Shaking camera for " + duration);
        //print("Camera local position " + originalPos);

        float elapsed = 0.0f;
        GetComponent<CameraFollow>().enabled = false;

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
        GetComponent<CameraFollow>().enabled = true;
    }

    public void setShakingTrue(float mgnitude)
    {
        //Debug.Log("Set shaking true with mag " + mgnitude);
        isShaking = true;
        magnitude = mgnitude;
        wasShaking = true;
        GetComponent<CameraFollow>().enabled = false;
    }
    public void setShakingFalse()
    {
        isShaking = false;
        magnitude = 0;
        GetComponent<CameraFollow>().enabled = true;
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

    private Vector3 GetNeutralPos()
    {
        return GetComponent<CameraFollow>().getPos();
    }
}
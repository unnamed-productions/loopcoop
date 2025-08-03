using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    [SerializeField]
    public float maxDisplacement;

    [SerializeField]
    float jitterInterval; //Smaller = more jitter

    private Vector3 originalPos;
    private float currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        currentTimer = jitterInterval;
        Debug.Log("original pos is " + originalPos);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0)
        {
            jitterMe();
            currentTimer = jitterInterval;
        }
    }

    private void jitterMe()
    {
        float xPos = Random.Range(originalPos.x - maxDisplacement, originalPos.x + maxDisplacement);
        float yPos = Random.Range(originalPos.y - maxDisplacement, originalPos.y + maxDisplacement);
        transform.localPosition = new Vector3(xPos, yPos, originalPos.z);
    }

}

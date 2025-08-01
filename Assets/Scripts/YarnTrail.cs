using System.Collections.Generic;
using UnityEngine;

public class YarnTrail : MonoBehaviour
{
    [SerializeField] GameObject yarnPrefab;
    [SerializeField] float segmentSpacing = 0.25f;
    [SerializeField] float curveStrength = 0.05f;
    [SerializeField] float wiggleFrequency = 3f;
    [SerializeField] float yOffset = 0.3f;


    private List<Vector3> trailPoints = new List<Vector3>();
    private Vector3 lastSpawnPos;

    void Start()
    {
        lastSpawnPos = transform.position;
        trailPoints.Add(lastSpawnPos);
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(lastSpawnPos, currentPos);

        while (distance >= segmentSpacing)
        {
            // interpolate points
            Vector3 direction = (currentPos - lastSpawnPos).normalized;
            Vector3 nextPos = lastSpawnPos + direction * segmentSpacing;

            DropSegment(lastSpawnPos, nextPos);
            lastSpawnPos = nextPos;
            distance = Vector3.Distance(lastSpawnPos, currentPos);
        }

    }

    void DropSegment(Vector3 from, Vector3 to)
    {
        //slight offset to curve yarn
        Vector3 dir = (to - from).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward).normalized;

        float offsetAmount = Mathf.Sin(Time.time * wiggleFrequency + trailPoints.Count) * curveStrength;
        Vector3 midPoint = Vector3.Lerp(from, to, 0.5f) + normal * offsetAmount;

        Vector3 upwardOffset = Vector3.up * yOffset;
        Vector3 spawnPos = midPoint + upwardOffset;

        GameObject seg = Instantiate(yarnPrefab, spawnPos, Quaternion.identity);

        // rotate to face direction from last point to current
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        seg.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

    }
}

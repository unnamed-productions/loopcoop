using System.Collections.Generic;
using UnityEngine;

public class YarnTrail : MonoBehaviour
{
    [SerializeField] GameObject yarnPrefab;
    [SerializeField] float segmentSpacing = 0.2f;
    [SerializeField] float curveStrength = 10f;

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

        if (Vector3.Distance(lastSpawnPos, currentPos) >= segmentSpacing)
        {
            trailPoints.Add(currentPos);
            DropSegment(lastSpawnPos, currentPos);
            lastSpawnPos = currentPos;
        }
    }

    void DropSegment(Vector3 from, Vector3 to)
    {
        //slight offset to curve yarn
        Vector3 dir = (to - from).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward).normalized; // perpendicular
        float offsetAmount = Mathf.Sin(Time.time * 3f + trailPoints.Count) * 0.05f;

        Vector3 midPoint = Vector3.Lerp(from, to, 0.5f) + normal * offsetAmount;
        GameObject seg = Instantiate(yarnPrefab, midPoint, Quaternion.identity);

        // rotate to face direction from last point to current
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        seg.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

    }
}

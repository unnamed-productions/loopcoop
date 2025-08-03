using System.Collections.Generic;
using UnityEngine;

public class YarnTrail : MonoBehaviour
{
    [Header("Yarn Settings")]
    [SerializeField] private GameObject yarnPrefab;
    [SerializeField] private float segmentSpacing = 0.25f;
    [SerializeField] private float curveStrength = 0.05f;
    [SerializeField] private float wiggleFrequency = 3f;
    [SerializeField] private float yOffset = 0.3f;

    [Header("Loop Settings")]
    [SerializeField] private float loopCloseThreshold = 0.5f;
    [SerializeField] private int minSegmentsForLoop = 8;

    public int loopedEnemyCount { get; private set; }

    private List<Vector3> trailPoints = new();
    private List<GameObject> yarnSegments = new();
    private Vector3 lastSpawnPos;

    void Start()
    {
        lastSpawnPos = transform.position;
        trailPoints.Add(lastSpawnPos);
    }

    void Update()
    {
        UpdateYarnTrail();
        TryCloseLoop();
    }

    void UpdateYarnTrail()
    {
        Vector3 currentPos = transform.position;
        float dist = Vector3.Distance(lastSpawnPos, currentPos);

        while (dist >= segmentSpacing)
        {
            Vector3 dir = (currentPos - lastSpawnPos).normalized;
            Vector3 nextPos = lastSpawnPos + dir * segmentSpacing;

            DropSegment(lastSpawnPos, nextPos);
            lastSpawnPos = nextPos;
            dist = Vector3.Distance(lastSpawnPos, currentPos);
        }
    }

    void DropSegment(Vector3 from, Vector3 to)
    {
        trailPoints.Add(to);

        Vector3 dir = (to - from).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward).normalized;
        float wiggle = Mathf.Sin(Time.time * wiggleFrequency + trailPoints.Count) * curveStrength;

        //vertical offset
        Vector3 mid = Vector3.Lerp(from, to, 0.5f) + normal * wiggle;
        Vector3 spawnAt = mid + Vector3.up * yOffset;

        //rotate to face movement direction
        var seg = Instantiate(yarnPrefab, spawnAt, Quaternion.identity);
        yarnSegments.Add(seg);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        seg.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryCloseLoop()
    {
        int count = trailPoints.Count;
        if (count < minSegmentsForLoop + 2) return;

        Vector3 lastPoint = trailPoints[^1];
        int loopStart = -1;
        int searchLimit = count - 1 - minSegmentsForLoop;

        // scan all older points beyond the “recent” ones
        for (int i = 0; i <= searchLimit; i++)
        {
            if (Vector3.Distance(trailPoints[i], lastPoint) <= loopCloseThreshold)
            {
                loopStart = i;
                break;
            }
        }

        if (loopStart < 0) return;

        Debug.Log($"Loop detected between {loopStart} and {count - 1}");

        // Build the closed-loop polygon
        var polygon = new List<Vector2>();
        for (int i = loopStart; i < count; i++)
            polygon.Add(trailPoints[i]);

        // close the loop back to the first point
        polygon.Add(trailPoints[loopStart]);

        HandleLoopedEnemies(polygon);

        ClearTrail();
    }


    void ClearTrail()
    {
        // destroy visuals
        foreach (var seg in yarnSegments)
            Destroy(seg);
        yarnSegments.Clear();

        // reset for new trail
        trailPoints.Clear();
        Vector3 p = transform.position;
        trailPoints.Add(p);
        lastSpawnPos = p;

        Debug.Log("Trail reset. Ready for next loop.");
    }

    bool IsPointInPolygon(Vector2 point, List<Vector2> poly)
    {
        int crossings = 0;
        int n = poly.Count;
        for (int i = 0; i < n; i++)
        {
            Vector2 a = poly[i];
            Vector2 b = poly[(i + 1) % n];
            bool cond = (a.y > point.y) != (b.y > point.y);
            if (cond)
            {
                float t = (point.y - a.y) / (b.y - a.y);
                float xCross = a.x + t * (b.x - a.x);
                if (point.x < xCross) crossings++;
            }
        }
        return (crossings & 1) == 1;
    }

    void HandleLoopedEnemies(List<Vector2> loopPolygon)
    {
        // reset count
        loopedEnemyCount = 0;

        // collect snared enemies
        var snared = new List<GameObject>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (IsPointInPolygon(enemy.transform.position, loopPolygon))
            {
                loopedEnemyCount++;
                snared.Add(enemy);
                //Destroy(enemy);
            }
        }

        Debug.Log($"Enemies looped: {loopedEnemyCount}");

        // foreach (var e in snared) {
        //     e.GetComponent<EnemyController>().Snare();
        //     e.GetComponent<Animator>().SetTrigger("Looped");
        // }

    }
}

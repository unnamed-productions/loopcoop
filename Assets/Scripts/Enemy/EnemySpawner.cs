using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private PolygonCollider2D spawnAreaCollider;

    [Header("Player Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float initialMinSpawnDistance = 5f;
    [SerializeField] private float finalMinSpawnDistance = 1f;

    [Header("Spawn Timing")]
    [SerializeField] private float initialMinInterval = 3f;
    [SerializeField] private float initialMaxInterval = 6f;
    [SerializeField] private float finalMinInterval = 0.5f;
    [SerializeField] private float finalMaxInterval = 2f;
    [Tooltip("Time in seconds over which spawn rate & distance ramp to final values")]
    [SerializeField] private float accelerationDuration = 300f;

    [Header("Initial Wave")]
    [Tooltip("Enemies to spawn immediately on Start")]
    [SerializeField] private int initialEnemyCount = 5;

    [Header("Enemies & Weights")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<int> enemyWeights;

    private GameObject player;
    private float elapsedTime;
    private float spawnTimer;
    private int totalWeight;

    void Start()
    {
        // Cache player
        player = GameObject.FindGameObjectWithTag(playerTag);

        // Pre-compute sum of weights
        totalWeight = 0;
        foreach (var w in enemyWeights) totalWeight += w;

        // First spawn delay
        ResetSpawnTimer();
    }

    void Update()
    {
        // Only spawn while the game is in PLAYING state
        //if (GameManager.instance.currentGameState != GameManager.GameState.PLAYING)
        //    return;

        elapsedTime += Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            ResetSpawnTimer();
        }
    }

    // Compute next random spawn delay based on elapsed time
    private void ResetSpawnTimer()
    {
        float t = Mathf.Clamp01(elapsedTime / accelerationDuration);
        float minI = Mathf.Lerp(initialMinInterval, finalMinInterval, t);
        float maxI = Mathf.Lerp(initialMaxInterval, finalMaxInterval, t);
        spawnTimer = Random.Range(minI, maxI);
    }

    // Core spawn logic
    private void SpawnEnemy()
    {
        // Compute current min-distance from player
        float t = Mathf.Clamp01(elapsedTime / accelerationDuration);
        float minDist = Mathf.Lerp(initialMinSpawnDistance, finalMinSpawnDistance, t);

        // Find a valid point
        Vector2 spawnPos = GetValidSpawnPosition(minDist);

        // Pick by weight
        int idx = PickWeightedIndex();
        Instantiate(enemyPrefabs[idx], spawnPos, Quaternion.identity);
    }

    // Samples up to 30 times within the collider bounds, checks inside poly & min distance
    private Vector2 GetValidSpawnPosition(float minDistance)
    {
        Bounds b = spawnAreaCollider.bounds;
        for (int i = 0; i < 30; i++)
        {
            Vector2 p = new Vector2(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y)
            );

            if (spawnAreaCollider.OverlapPoint(p) &&
                Vector2.Distance(p, player.transform.position) >= minDistance)
                return p;
        }

        // Fallback: push out along boundary if unlucky
        Vector2 fallback = spawnAreaCollider.ClosestPoint(player.transform.position);
        Vector2 dir = (fallback - (Vector2)player.transform.position).normalized;
        return (Vector2)player.transform.position + dir * minDistance;
    }

    // Standard weighted-random index picker
    private int PickWeightedIndex()
    {
        int point = Random.Range(0, totalWeight);
        for (int i = 0; i < enemyWeights.Count; i++)
        {
            if (point < enemyWeights[i]) return i;
            point -= enemyWeights[i];
        }
        return enemyWeights.Count - 1; // fallback
    }
}

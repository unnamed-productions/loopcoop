using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBallSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject yarnPrefab;

    public List<GameObject> snared;

    public void setSnared(List<GameObject> snard)
    {
        snared = snard;
    }

    public void spawnYarn()
    {
        Instantiate(yarnPrefab, transform.position, Quaternion.identity).GetComponent<YarnBall>().RegisterEnemies(snared);
        Destroy(gameObject);
    }
}

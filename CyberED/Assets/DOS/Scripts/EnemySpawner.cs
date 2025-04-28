using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;           // Assign Boss prefab here
    public Transform[] spawnPoints;          // Drop your spawn points here
    public int totalEnemiesToSpawn = 10;
    public float spawnDelay = 5f;

    private int enemiesSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            // Randomly pick a spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            enemiesSpawned++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

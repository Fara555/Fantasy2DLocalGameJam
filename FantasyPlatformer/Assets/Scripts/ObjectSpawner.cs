using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPoolManager enemyPool;
    public int enemiesToSpawn = 5;
    public float spawnRate = 2f;

    private int enemiesSpawned = 0;
    private Coroutine spawnCoroutine;
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        if (spawnPoints.Length < enemiesToSpawn)
        {
            Debug.LogError("Not enough spawn points for the number of enemies to spawn.");
            return;
        }

        // ��������� ����� ������
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        while (enemiesSpawned < enemiesToSpawn)
        {
            SpawnEnemy(availableSpawnPoints);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnEnemy(List<Transform> availableSpawnPoints)
    {
        if (availableSpawnPoints.Count == 0) return;

        GameObject enemy = enemyPool.GetPooledObject();
        if (enemy != null)
        {
            // �������� ��������� ����� �� ���������� ���������
            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[spawnIndex];

            // ������������� ������� ������ � ���������� �����
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);

            // ������� �������������� ����� �� ������
            availableSpawnPoints.RemoveAt(spawnIndex);

            enemiesSpawned++;
        }
    }
}

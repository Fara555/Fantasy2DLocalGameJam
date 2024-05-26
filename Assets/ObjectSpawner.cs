
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPoolManager objectPoolManager;
    public int enemiesToSpawn = 5;
    public float spawnRate = 2f;

    private int enemiesSpawned = 0;
    private Coroutine spawnCoroutine;

    void Start()
    {
        // ��������� ����� ������
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesToSpawn)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = objectPoolManager.GetPooledEnemy();

        if (enemy != null)
        {
            // ������������� ������� ������ � ���������� �����
            enemy.transform.position = transform.position;
            enemy.SetActive(true);
            enemiesSpawned++;
        }
    }

    // �����, ������� ���������� ��� "������" �����
    public void EnemyDied(GameObject enemy)
    {
        // ������������ ����� � ���������� � ���
        enemy.SetActive(false);
        objectPoolManager.ReturnEnemyToPool(enemy);
        enemiesSpawned--;

        // ���� ���������� ���������� �������, ���������� �����
        if (enemiesSpawned <= 0)
        {
            StopCoroutine(spawnCoroutine);
            Debug.Log("All enemies spawned and died.");
        }
    }
}


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
        // Запускаем спавн врагов
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
            // Устанавливаем позицию спауна и активируем врага
            enemy.transform.position = transform.position;
            enemy.SetActive(true);
            enemiesSpawned++;
        }
    }

    // Метод, который вызывается при "смерти" врага
    public void EnemyDied(GameObject enemy)
    {
        // Деактивируем врага и возвращаем в пул
        enemy.SetActive(false);
        objectPoolManager.ReturnEnemyToPool(enemy);
        enemiesSpawned--;

        // Если достигнуто количество спаунов, прекращаем спавн
        if (enemiesSpawned <= 0)
        {
            StopCoroutine(spawnCoroutine);
            Debug.Log("All enemies spawned and died.");
        }
    }
}

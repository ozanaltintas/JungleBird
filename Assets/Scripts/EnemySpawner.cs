using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 7f;
    [SerializeField] private float heightRange = 3f;

    private float timer = 0f;
    private float nextSpawnTime;

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnEnemy();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnEnemy()
    {
        float randomY = Random.Range(-heightRange, heightRange);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
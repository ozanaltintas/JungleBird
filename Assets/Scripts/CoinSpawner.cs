using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 6f;
    [SerializeField] private float heightRange = 2f; // Ne kadar yukarı/aşağı çıksın

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
            SpawnCoin();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnCoin()
    {
        // Yüksekliği rastgele belirle
        float randomY = Random.Range(-heightRange, heightRange);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);

        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
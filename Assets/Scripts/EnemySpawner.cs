using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Düşman Çeşitleri")]
    // İŞTE İSTEDİĞİN ARRAY BURADA:
    [SerializeField] private GameObject[] enemyPrefabs; 

    [Header("Spawn Ayarları")]
    [SerializeField] private float spawnInterval = 4f; // Kaç saniyede bir gelsin?
    [SerializeField] private float spawnX = 12f;       // Nerede doğsun?
    [SerializeField] private float minY = -3f;         // En aşağı yükseklik
    [SerializeField] private float maxY = 3f;          // En yukarı yükseklik

    private float timer;

    void Update()
    {
        if (Time.timeScale == 0) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }
    }

    void SpawnRandomEnemy()
    {
        // 1. Array boşsa hata vermesin diye kontrol
        if (enemyPrefabs.Length == 0) return;

        // 2. RASTGELE SEÇİM (0 ile Array uzunluğu arasında)
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[randomIndex];

        // 3. Rastgele Yükseklik
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        // 4. Sahneye Yarat
        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }
}
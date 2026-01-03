using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    // Başlangıç değerleri (Inspector'dan değiştirebilirsin)
    public float currentMoveSpeed = 3f;
    public float currentSpawnRate = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void IncreaseDifficulty()
    {
        // 1. Oyun hızını arttır (Borular daha hızlı kaysın)
        currentMoveSpeed += 0.5f;

        // 2. Boru çıkma süresini azalt (Daha sık boru gelsin)
        // Oyun hızlanınca aradaki mesafe açılmasın diye bunu da kısmamız lazım.
        currentSpawnRate -= 0.2f;

        // Çok aşırı hızlanıp oyun bozulmasın diye sınır koyalım
        if (currentSpawnRate < 0.6f) currentSpawnRate = 0.6f;
        
        Debug.Log("Zorluk Arttı! Yeni Hız: " + currentMoveSpeed);
    }
}
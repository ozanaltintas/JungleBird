using UnityEngine;

public class PipeItemSpawner : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private GameObject itemPrefab; // Kiraz Prefabı
    [SerializeField][Range(0, 100)] private int spawnChance = 20; // %20 şansla çıksın

    void Start()
    {
        // Zar at: 0 ile 100 arasında sayı seç
        int roll = Random.Range(0, 100);

        // Eğer sayı şans oranından küçükse kiraz oluştur
        if (roll < spawnChance)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        // Kirazı bu boru setinin tam ortasında oluştur
        // (Pipe objesinin child'ı yapıyoruz ki boruyla beraber sola kaysın)
        GameObject item = Instantiate(itemPrefab, transform);

        // Konum ayarı: X=0 (Borunun ortası), Y=0 (Yükseklik ortası)
        // Eğer kiraz borulara çok yakınsa Y değerini Random.Range(-1f, 1f) yapabilirsin.
        item.transform.localPosition = Vector3.zero;
    }
}
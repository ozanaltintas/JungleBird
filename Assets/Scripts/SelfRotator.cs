using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    [Header("Dönüş Ayarı")]
    [Tooltip("Saniyede kaç derece dönecek? (Eksi değer ters yöne döndürür)")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Rastgelelik (Opsiyonel)")]
    [Tooltip("İşaretlenirse oyun başlayınca hızı rastgele belirler")]
    [SerializeField] private bool randomizeSpeed = false;
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 100f;

    void Start()
    {
        // Eğer rastgelelik istiyorsan her gezegen farklı hızda ve yönde döner
        if (randomizeSpeed)
        {
            // %50 ihtimalle ters yöne (-1) veya düz yöne (1) dönsün
            float direction = Random.value > 0.5f ? 1f : -1f;
            rotationSpeed = Random.Range(minSpeed, maxSpeed) * direction;
        }
    }

    void Update()
    {
        // Oyun durmuşsa döndürme
        if (Time.timeScale == 0) return;

        // 2D Oyun olduğu için Z ekseninde (Vector3.forward) döndürüyoruz
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
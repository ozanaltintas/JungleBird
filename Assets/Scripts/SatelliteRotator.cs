using UnityEngine;

public class SatelliteRotator : MonoBehaviour
{
    [Header("Hedef")]
    [Tooltip("Etrafında dönülecek merkez gezegen")]
    [SerializeField] private Transform target;

    [Header("Ayarlar")]
    [SerializeField] private float orbitSpeed = 50f; // Dönüş hızı
    [SerializeField] private bool clockwise = true;  // Saat yönünde mi dönsün?

    void Update()
    {
        // Hedef yoksa veya oyun durduysa işlem yapma
        if (target == null || Time.timeScale == 0) return;

        // Yön belirleme (2D oyunlar için Z ekseni etrafında döneriz)
        // Saat yönü için -1, tersi için 1 ile çarparız
        float direction = clockwise ? -1f : 1f;

        // --- SİHİRLİ SATIR ---
        // (Hedefin Pozisyonu, Dönüş Ekseni, Hız)
        transform.RotateAround(target.position, Vector3.forward, orbitSpeed * direction * Time.deltaTime);
        
        // ÖNEMLİ NOT: RotateAround objenin yüzünü de merkeze döndürür.
        // Eğer uydunun yüzü hep aynı kalsın istiyorsan, kendi eksenindeki dönüşü sıfırlayabilirsin:
        // transform.rotation = Quaternion.identity; // (Bunu istersen açarsın)
    }
}
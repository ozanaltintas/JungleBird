using UnityEngine;

public class BirdRotator : MonoBehaviour
{
    [Header("Dönüş Ayarları")]
    [SerializeField] private float rotationSpeed = 5f;   // Hızın açıya dönüşme çarpanı
    [SerializeField] private float lerpSpeed = 10f;      // Dönüşün yumuşaklık hızı
    [SerializeField] private float minAngle = -90f;      // Maksimum aşağı bakma açısı
    [SerializeField] private float maxAngle = 25f;       // Maksimum yukarı bakma açısı
    
    private Rigidbody2D rb;

    private void Awake()
    {
        // Aynı obje üzerindeki Rigidbody'yi bul
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kuşun anlık dikey hızını al
        float currentVelocity = rb.linearVelocity.y;

        // Hızı bir açı değerine dönüştür. 
        // Örneğin hız 5 ise, açı 25 olacak (5 * 5).
        float targetAngle = currentVelocity * rotationSpeed;

        // Açıyı sınırla. Kuş takla atmasın.
        // En fazla 25 derece yukarı, en fazla -90 derece aşağı baksın.
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        // Hedef dönüşü hesapla
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // Mevcut dönüşten hedef dönüşe yumuşak bir geçiş yap (Lerp)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
}
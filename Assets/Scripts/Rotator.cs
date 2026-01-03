using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Dönüş Hızı (Pozitif: Sola, Negatif: Sağa)")]
    [SerializeField] private float rotationSpeed = 200f;

    private void Update()
    {
        // Z ekseni etrafında (2D'de saat yönü veya tersi) dön
        // Vector3.forward = (0, 0, 1) demektir.
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float rotateSpeed = 50f; // Düşmanlar uzayda dönerek gitsin
    private float destroyX = -15f;

    private void Update()
    {
        // Oyun durmuşsa hareket etme
        if (Time.timeScale == 0) return;

        float currentSpeed = DifficultyManager.Instance.currentMoveSpeed * speedMultiplier;

        // 1. Sola Git (Space.World önemli!)
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);

        // 2. Kendi ekseninde dön (Görsel şov için)
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        // 3. Ekrandan çıkınca yok ol
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
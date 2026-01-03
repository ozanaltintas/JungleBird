using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private AudioClip collectSound; // Toplama sesi
    private float destroyX = -15f;

    private void Update()
    {
        // Zorluk sistemine bağlı olarak sola kayma hareketi
        // Coinler de borularla aynı hızda gitsin ki senkronize dursun
        float currentSpeed = DifficultyManager.Instance.currentMoveSpeed * speedMultiplier;

        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);

        // Ekrandan çıkınca yok et
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpan şey "Player" (yani Kuş) ise
        if (other.CompareTag("Player")) // Kuşun tag'ini kontrol edeceğiz
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        // ESKİSİ: GameManager.Instance.IncreaseScore(); 

        // YENİSİ: Sadece Coin arttır
        GameManager.Instance.AddCoin();

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        Destroy(gameObject);
    }
}
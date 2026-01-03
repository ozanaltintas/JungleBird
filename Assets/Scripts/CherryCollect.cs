using UnityEngine;

public class CherryCollect : MonoBehaviour
{
    [SerializeField] private float shieldDuration = 5f; // Kaç saniye korusun?

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpan şey Kuş mu?
        BirdController bird = collision.GetComponent<BirdController>();

        if (bird != null)
        {
            // Kuşun kalkanını aktif et
            bird.ActivateShield(shieldDuration);

            // Kendini yok et (Yendi)
            Destroy(gameObject);
        }
    }
}
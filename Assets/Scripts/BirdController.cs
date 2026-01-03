using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Ses Efektleri")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    public bool isShielded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 0 || isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = Vector2.up * jumpForce;
        if (jumpSound != null) audioSource.PlayOneShot(jumpSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // KALKAN KONTROLÜ
        if (isShielded)
        {
            // Ölümsüzlük devrede, çarpışmayı yoksay
            Debug.Log("Kalkan korudu!");
            return;
        }

        isDead = true;
        if (hitSound != null) audioSource.PlayOneShot(hitSound);
        VibrationManager.VibrateHeavy();
        GameManager.Instance.GameOver();
    }

    public void ActivateShield(float duration)
    {
        // Eğer zaten kalkanlıysa süreyi sıfırlamak için önce eski rutini durdur
        StopAllCoroutines();
        StartCoroutine(ShieldRoutine(duration));
    }

    IEnumerator ShieldRoutine(float duration)
    {
        isShielded = true;

        // --- FOSFORLU ALTIN RENGİ ---
        // Normalde renkler 0 ile 1 arasındadır. 
        // Buraya 1'den büyük değerler girerek "HDR" (Aşırı Parlak) etkisi yaratıyoruz.
        // R(Kırmızı): 3.0, G(Yeşil): 2.5, B(Mavi): 0.5 -> Kör edici bir altın sarısı!
        Color glowingGold = new Color(3.0f, 2.5f, 0.5f, 1f);

        // Kuşu boya
        if (spriteRenderer != null) spriteRenderer.color = glowingGold;

        if (shieldSound != null) audioSource.PlayOneShot(shieldSound);
        VibrationManager.VibrateLight();

        // Sürenin bitimine 1 saniye kalana kadar bekle
        yield return new WaitForSeconds(duration - 1f);

        // --- Son 1 Saniye Yanıp Sönme Efekti ---
        for (int i = 0; i < 5; i++) // 5 kere hızlıca yanıp sön
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.white; // Normal
            yield return new WaitForSeconds(0.1f);
            if (spriteRenderer != null) spriteRenderer.color = glowingGold; // Parlak
            yield return new WaitForSeconds(0.1f);
        }

        // Normale dön
        isShielded = false;
        if (spriteRenderer != null) spriteRenderer.color = Color.white;
    }
}
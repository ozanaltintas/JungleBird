using UnityEngine;
using UnityEngine.UI; 
using System.Collections; // IEnumerator için gerekli

public class BirdController : MonoBehaviour
{
    [Header("Jetpack Ayarları")]
    [SerializeField] private float flyForce = 12f;      
    [SerializeField] private float maxFuel = 100f;      
    [SerializeField] private float fuelBurnRate = 30f;  
    [SerializeField] private float fuelRegenRate = 50f; 
    [SerializeField] private float ignitionCost = 15f; 

    [Header("UI & Görsel")]
    [SerializeField] private Image fuelBarFill; 
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldSound; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    
    private float currentFuel;
    private bool canFly = true; 
    private bool isDead = false;
    
    // CherryCollect.cs'in erişmesi gereken değişken
    public bool isShielded = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        currentFuel = maxFuel;
    }

    void Update()
    {
        if (Time.timeScale == 0 || isDead) return;

        HandleFlight();
        UpdateUI();
    }

    private void HandleFlight()
    {
        // 1. TIKLAMA CEZASI
        if (Input.GetMouseButtonDown(0) && canFly)
        {
            currentFuel -= ignitionCost;
        }

        // 2. UÇUŞ
        if (Input.GetMouseButton(0) && canFly && currentFuel > 0)
        {
            rb.linearVelocity = Vector2.up * flyForce; 
            currentFuel -= fuelBurnRate * Time.deltaTime; 

            if (currentFuel <= 0)
            {
                currentFuel = 0;
                canFly = false; // Motor kilitlendi
            }
        }
        else
        {
            // 3. DOLUM
            if (currentFuel < maxFuel)
            {
                currentFuel += fuelRegenRate * Time.deltaTime;
            }

            // KİLİT AÇILMA ŞARTI: Sadece %100 dolunca
            if (currentFuel >= maxFuel)
            {
                currentFuel = maxFuel;
                canFly = true; 
            }
        }
    }

    private void UpdateUI()
    {
        if (fuelBarFill != null)
        {
            fuelBarFill.fillAmount = currentFuel / maxFuel;
            // Kilitliyse Kırmızı, Açıksa Yeşil
            fuelBarFill.color = canFly ? Color.green : Color.red;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // KALKAN VARSA ÖLME
        if (isShielded) return;

        if (isDead) return;
        
        isDead = true;
        if (hitSound != null) audioSource.PlayOneShot(hitSound);
        
        // Titreşim kontrolü (VibrationButton ile entegre)
        if (VibrationButton.CanVibrate())
        {
            // Handheld.Vibrate() veya kendi VibrationManager kodun:
             // VibrationManager.VibrateHeavy(); 
        }

        GameManager.Instance.GameOver();
    }

    // --- CHERRY COLLECT TARAFINDAN ÇAĞRILAN EKSİK FONKSİYON ---
    public void ActivateShield(float duration)
    {
        StopAllCoroutines(); 
        StartCoroutine(ShieldRoutine(duration));
    }

    IEnumerator ShieldRoutine(float duration)
    {
        isShielded = true;
        
        // Altın rengi efekt
        Color glowingGold = new Color(1f, 0.8f, 0.2f, 1f); 
        if (spriteRenderer != null) spriteRenderer.color = glowingGold;
        if (shieldSound != null && audioSource != null) audioSource.PlayOneShot(shieldSound);
        
        // Sürenin çoğunu bekle
        yield return new WaitForSeconds(duration - 1f);

        // Son saniye yanıp sön
        for (int i = 0; i < 5; i++)
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.white; 
            yield return new WaitForSeconds(0.1f);
            if (spriteRenderer != null) spriteRenderer.color = glowingGold; 
            yield return new WaitForSeconds(0.1f);
        }

        // Normale dön
        isShielded = false;
        if (spriteRenderer != null) spriteRenderer.color = Color.white;
    }
}
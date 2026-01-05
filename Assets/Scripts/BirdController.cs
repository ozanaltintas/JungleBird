using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class BirdController : MonoBehaviour
{
    [Header("Uçuş (Jetpack) Ayarları")]
    [SerializeField] private float flyForce = 15f;      
    [SerializeField] private float maxFuel = 100f;      
    [SerializeField] private float fuelBurnRate = 30f;  // Normal uçuşta harcanan (Biraz düşürdüm)
    [SerializeField] private float fuelRegenRate = 40f; // Dolma hızı (Biraz yavaşlattım)
    
    [Header("Zorlaştırma Ayarı")]
    [Tooltip("Her tıklamada anında düşecek yakıt miktarı")]
    [SerializeField] private float ignitionCost = 15f; // TIKLAMA CEZASI!

    [Header("UI Bağlantıları")]
    [SerializeField] private Image fuelBarFill; 

    [Header("Ses Efektleri")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldSound; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    
    private float currentFuel;
    private bool isDead = false;
    private bool canFly = true; 
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
        UpdateFuelUI();
    }

    private void HandleFlight()
    {
        // 1. İLK TIKLAMA CEZASI (Ignition Cost)
        if (Input.GetMouseButtonDown(0) && canFly)
        {
            currentFuel -= ignitionCost;
            // Ufak bir duman efekti veya ateşleme sesi buraya eklenebilir
        }

        // 2. SÜREKLİ UÇUŞ
        if (Input.GetMouseButton(0) && canFly)
        {
            rb.linearVelocity = Vector2.up * flyForce; 
            currentFuel -= fuelBurnRate * Time.deltaTime; 

            // Yakıt biterse
            if (currentFuel <= 0)
            {
                currentFuel = 0;
                canFly = false; 
            }
        }
        else
        {
            // Elini çektiyse yakıt dolsun
            currentFuel += fuelRegenRate * Time.deltaTime;

            if (currentFuel >= maxFuel)
            {
                currentFuel = maxFuel;
                canFly = true;
            }
            // Bar cezaya girdiyse %20 dolmadan uçamasın
            else if (!canFly && currentFuel > 20f)
            {
                canFly = true;
            }
        }
    }

    private void UpdateFuelUI()
    {
        if (fuelBarFill != null)
        {
            fuelBarFill.fillAmount = currentFuel / maxFuel;
            fuelBarFill.color = canFly ? Color.green : Color.red;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isShielded) return;
        if (isDead) return;

        isDead = true;
        if (hitSound != null) audioSource.PlayOneShot(hitSound);
        VibrationManager.VibrateHeavy();
        GameManager.Instance.GameOver();
    }

    public void ActivateShield(float duration)
    {
        StopAllCoroutines(); 
        StartCoroutine(ShieldRoutine(duration));
    }

    IEnumerator ShieldRoutine(float duration)
    {
        isShielded = true;
        Color glowingGold = new Color(6.0f, 4.5f, 0.5f, 1f); 

        if (spriteRenderer != null) spriteRenderer.color = glowingGold;
        if (shieldSound != null) audioSource.PlayOneShot(shieldSound);
        
        yield return new WaitForSeconds(duration - 1f);

        for (int i = 0; i < 5; i++)
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.white; 
            yield return new WaitForSeconds(0.1f);
            if (spriteRenderer != null) spriteRenderer.color = glowingGold; 
            yield return new WaitForSeconds(0.1f);
        }

        isShielded = false;
        if (spriteRenderer != null) spriteRenderer.color = Color.white;
    }
}
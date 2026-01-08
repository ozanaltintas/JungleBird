using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    [Header("Hız Ayarları")]
    [SerializeField] private float normalSpeed = 4f; 
    [SerializeField] private float boostSpeed = 10f; 

    [Header("Boost Hissiyatı")]
    [SerializeField] private float boostKickForce = 5f; 

    [Header("Yakıt & Ceza Sistemi")]
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float fuelBurnRate = 40f;   
    [SerializeField] private float fuelRegenRate = 30f;  
    [SerializeField] private float ignitionCost = 15f;   

    [Header("Referanslar")]
    [SerializeField] private BoostUI boostUI; // Buraya FuelBarBackground'u sürükle

    [Header("Görsel & Dönüş")]
    [SerializeField] private float rotationAngle = 25f;        
    [SerializeField] private float boostRotationAngle = 45f;  
    [SerializeField] private float turnSmoothness = 12f;
    [SerializeField] private Color boostColor = new Color(1f, 0.5f, 0.5f); 

    [Header("Sesler")]
    [SerializeField] private AudioClip moveSound; 
    [SerializeField] private AudioClip boostStartSound; 
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldSound; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    
    private float currentFuel;
    private bool isMovingUp = true; 
    private bool isDead = false;
    public bool isShielded = false;
    private bool isOverheated = false; 
    
    private bool wasBoostingLastFrame = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb.gravityScale = 0; 
        currentFuel = maxFuel;
    }

    void Update()
    {
        if (Time.timeScale == 0 || isDead) return;

        HandleInput();
        HandleFuelAndMovement();
        
        // --- KUSURSUZ GEÇİŞ BURADA TETİKLENİYOR ---
        if (boostUI != null)
        {
            boostUI.UpdateBoostBar(currentFuel, maxFuel);
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMovingUp = !isMovingUp;
            
            if (!isOverheated && currentFuel > 0)
            {
                currentFuel -= ignitionCost;
                if (currentFuel <= 0) { currentFuel = 0; isOverheated = true; }
            }

            if (moveSound != null && audioSource != null) 
                audioSource.PlayOneShot(moveSound);
        }
    }

    private void HandleFuelAndMovement()
    {
        float targetSpeed = normalSpeed;
        float targetAngle = rotationAngle; 

        bool isHolding = Input.GetMouseButton(0);
        bool isBoosting = false;

        if (isHolding && !isOverheated && currentFuel > 0)
        {
            isBoosting = true;
            targetSpeed = boostSpeed;
            targetAngle = boostRotationAngle;
            
            currentFuel -= fuelBurnRate * Time.deltaTime;
            if (currentFuel <= 0) { currentFuel = 0; isOverheated = true; }

            if (!wasBoostingLastFrame)
            {
                if (boostStartSound != null) audioSource.PlayOneShot(boostStartSound);
            }
        }
        else
        {
            isBoosting = false;
            targetSpeed = normalSpeed;

            if (currentFuel < maxFuel) currentFuel += fuelRegenRate * Time.deltaTime;
            
            if (isOverheated && currentFuel >= maxFuel)
            {
                currentFuel = maxFuel;
                isOverheated = false; 
            }
        }

        wasBoostingLastFrame = isBoosting;

        if (isMovingUp)
        {
            rb.linearVelocity = Vector2.up * targetSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.down * targetSpeed;
            targetAngle = -targetAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothness * Time.deltaTime);

        if (!isShielded)
            spriteRenderer.color = isBoosting ? boostColor : Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isShielded || isDead) return;
        isDead = true;
        if (hitSound != null) audioSource.PlayOneShot(hitSound);
        GameManager.Instance.GameOver();
    }

    public void ActivateShield(float duration) { StopAllCoroutines(); StartCoroutine(ShieldRoutine(duration)); }
    IEnumerator ShieldRoutine(float duration) { isShielded = true; Color gold = new Color(1f,0.8f,0.2f); if(spriteRenderer) spriteRenderer.color=gold; yield return new WaitForSeconds(duration-1); for(int i=0;i<5;i++){if(spriteRenderer)spriteRenderer.color=Color.white; yield return new WaitForSeconds(0.1f); if(spriteRenderer)spriteRenderer.color=gold; yield return new WaitForSeconds(0.1f);} isShielded=false; if(spriteRenderer)spriteRenderer.color=Color.white;}
}
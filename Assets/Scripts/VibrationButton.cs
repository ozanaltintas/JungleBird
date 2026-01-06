using UnityEngine;
using UnityEngine.UI; // UI işlemleri için şart

public class VibrationButton : MonoBehaviour
{
    [Header("İkonlar")]
    [SerializeField] private Sprite vibrationOnSprite;  // Titreşim Açık ikonu (Dalgalı telefon)
    [SerializeField] private Sprite vibrationOffSprite; // Titreşim Kapalı ikonu (Çizgili telefon)

    private Image buttonImage;
    private bool isVibrationOn = true;

    // Bu anahtar kelimeyi hafızaya kaydederken kullanacağız
    private const string VIB_KEY = "VibrationEnabled";

    void Start()
    {
        buttonImage = GetComponent<Image>();

        // Oyun başladığında hafızadaki eski ayarı hatırla
        // GetInt(Anahtar, Varsayılan Değer) -> Bulamazsa 1 (Açık) kabul etsin
        isVibrationOn = PlayerPrefs.GetInt(VIB_KEY, 1) == 1;

        UpdateUI();
    }

    // Bu fonksiyonu Butonun "On Click" olayına bağlayacağız
    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn; // Durumu tersine çevir
        
        // Durumu hafızaya kaydet (Açıksa 1, kapalıysa 0)
        PlayerPrefs.SetInt(VIB_KEY, isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateUI();
        Debug.Log("Titreşim Durumu: " + (isVibrationOn ? "Açık" : "Kapalı"));
    }

    void UpdateUI()
    {
        if (isVibrationOn)
        {
            // İkonu "Açık" yap
            buttonImage.sprite = vibrationOnSprite; 
        }
        else
        {
            // İkonu "Kapalı" yap
            buttonImage.sprite = vibrationOffSprite; 
        }
    }

    // --- DİĞER SCRİPTLERİN KULLANMASI İÇİN YARDIMCI FONKSİYON ---
    // Başka bir kod (mesela kuş çarpınca) titreşim yapıp yapmayacağını
    // bu fonksiyona sorarak öğrenecek.
    public static bool CanVibrate()
    {
        // Kayıtlı ayarı oku, bulamazsan 1 (true) döndür
        return PlayerPrefs.GetInt(VIB_KEY, 1) == 1;
    }
}
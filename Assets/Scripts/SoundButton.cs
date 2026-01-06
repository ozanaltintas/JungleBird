using UnityEngine;
using UnityEngine.UI; // UI işlemleri için şart

public class SoundButton : MonoBehaviour
{
    [Header("İkonlar")]
    [SerializeField] private Sprite soundOnSprite;  // Sesli ikonunu buraya sürükle
    [SerializeField] private Sprite soundOffSprite; // Sessiz ikonunu buraya sürükle

    private Image buttonImage;
    private bool isMuted = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();

        // Oyun başladığında hafızadaki eski ayarı hatırla
        // (0 = Ses Var, 1 = Ses Yok diye kabul edelim)
        if (PlayerPrefs.HasKey("Muted"))
        {
            isMuted = PlayerPrefs.GetInt("Muted") == 1;
        }
        else
        {
            isMuted = false; // Varsayılan olarak ses açık başlasın
        }

        UpdateSoundState();
    }

    // Bu fonksiyonu Butonun "On Click" olayına bağlayacağız
    public void ToggleSound()
    {
        isMuted = !isMuted; // Durumu tersine çevir (Açıksa kapat, kapalıysa aç)
        
        // Durumu hafızaya kaydet
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSoundState();
    }

    void UpdateSoundState()
    {
        if (isMuted)
        {
            // SESİ KAPAT
            AudioListener.volume = 0; // Oyunun global sesini sıfırlar
            buttonImage.sprite = soundOffSprite; // İkonu değiştir
        }
        else
        {
            // SESİ AÇ
            AudioListener.volume = 1; // Oyunun global sesini açar
            buttonImage.sprite = soundOnSprite; // İkonu değiştir
        }
    }
}
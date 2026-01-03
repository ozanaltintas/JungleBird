using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    [Header("Bileşenler")]
    [SerializeField] private Image buttonImage; // Butonun üzerindeki Image bileşeni

    [Header("Renk Ayarları")]
    // Ses açıkkenki renk (Genelde Beyaz iyidir)
    [SerializeField] private Color soundOnColor = Color.white;

    // Ses kapalıykenki renk (Gri veya yarı saydam yapabilirsin)
    // 0.5f, 0.5f, 0.5f = Orta ton gri
    [SerializeField] private Color soundOffColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private bool isMuted = false;

    void Start()
    {
        // Oyun açıldığında hafızadaki ses ayarını oku (0: Açık, 1: Kapalı)
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateIconAndVolume();
    }

    // Butona basılınca bu çalışır
    public void ToggleSound()
    {
        // Durumu tam tersine çevir
        isMuted = !isMuted;

        // Hafızaya kaydet (1 veya 0 olarak)
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateIconAndVolume();
    }

    // Duruma göre sesi ve rengi ayarlar
    private void UpdateIconAndVolume()
    {
        if (isMuted)
        {
            // SESİ KAPAT
            AudioListener.volume = 0;

            // RENGİ GRİ YAP (Pasif görünüm)
            if (buttonImage != null) buttonImage.color = soundOffColor;
        }
        else
        {
            // SESİ AÇ
            AudioListener.volume = 1;

            // RENGİ BEYAZ YAP (Aktif görünüm)
            if (buttonImage != null) buttonImage.color = soundOnColor;
        }
    }
}
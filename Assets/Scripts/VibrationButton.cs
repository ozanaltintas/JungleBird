using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
    [Header("Bileşenler")]
    [SerializeField] private Image buttonImage; // Butonun üzerindeki Image

    [Header("Renk Ayarları")]
    [SerializeField] private Color onColor = Color.white; // Açıkken (Beyaz)
    [SerializeField] private Color offColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Kapalıyken (Gri)

    private bool isVibrationOff = false;

    void Start()
    {
        // 1. Hafızadan durumu oku (0: Açık, 1: Kapalı)
        // Varsayılan olarak 0 (Açık) gelir.
        isVibrationOff = PlayerPrefs.GetInt("IsVibrationOff", 0) == 1;

        // 2. Yöneticiye durumu bildir
        VibrationManager.hapticsEnabled = !isVibrationOff;

        // 3. Buton rengini güncelle
        UpdateIcon();
    }

    public void ToggleVibration()
    {
        // Durumu tersine çevir
        isVibrationOff = !isVibrationOff;

        // Hafızaya kaydet
        PlayerPrefs.SetInt("IsVibrationOff", isVibrationOff ? 1 : 0);
        PlayerPrefs.Save();

        // Yöneticiye yeni durumu bildir
        VibrationManager.hapticsEnabled = !isVibrationOff;

        // Görseli güncelle
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (buttonImage == null) return;

        if (isVibrationOff)
        {
            // TİTREŞİM KAPALI -> Rengi Gri yap
            buttonImage.color = offColor;
        }
        else
        {
            // TİTREŞİM AÇIK -> Rengi Beyaz yap
            buttonImage.color = onColor;
        }
    }
}
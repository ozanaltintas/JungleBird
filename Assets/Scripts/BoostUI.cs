using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Image fillImage;

    [Header("Renk Ayarları")]
    public Gradient colorGradient;

    public void UpdateBoostBar(float current, float max)
    {
        float ratio = current / max;
        fillImage.fillAmount = ratio;

        // Editörde ayarladığın gradient neyse, %100 uyumlu şekilde rengi basar.
        fillImage.color = colorGradient.Evaluate(ratio);
    }
}
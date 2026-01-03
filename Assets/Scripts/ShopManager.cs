using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class BirdItem
    {
        public string name;
        public int price;
        public bool isUnlocked;
        public Button buyButton;
        public TextMeshProUGUI buttonText;
    }

    public BirdItem[] birds;
    public TextMeshProUGUI coinText;
    public GameObject shopPanel;

    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        UpdateUI();
    }

    public void UpdateUI()
    {
        // Cüzdan Bakiyesi
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (coinText != null)
        {
            // Sadece sayıyı gösterir (Örn: 150)
            coinText.text = currentCoins.ToString();
        }

        // Seçili Kuş Kontrolü
        int selectedBirdIndex = PlayerPrefs.GetInt("SelectedBird", 0);

        if (birds != null)
        {
            for (int i = 0; i < birds.Length; i++)
            {
                // Kilit Durumu Kontrolü (0. kuş her zaman açıktır)
                if (i == 0)
                    birds[i].isUnlocked = true;
                else
                {
                    int unlocked = PlayerPrefs.GetInt("Bird_" + i + "_Unlocked", 0);
                    if (unlocked == 1) birds[i].isUnlocked = true;
                }

                // Buton Görünümü Ayarlama
                if (birds[i].isUnlocked)
                {
                    // Eğer zaten açıksa
                    if (selectedBirdIndex == i)
                    {
                        birds[i].buttonText.text = "SECILI";
                        birds[i].buyButton.interactable = false; // Zaten seçili, tekrar basılmasın
                    }
                    else
                    {
                        birds[i].buttonText.text = "SEC";
                        birds[i].buyButton.interactable = true;
                    }
                }
                else
                {
                    // Eğer kilitliyse
                    birds[i].buttonText.text = birds[i].price + " COIN";

                    // Para yetiyorsa buton aktif, yetmiyorsa pasif
                    if (currentCoins >= birds[i].price)
                        birds[i].buyButton.interactable = true;
                    else
                        birds[i].buyButton.interactable = false;
                }
            }
        }
    }

    public void OnBirdBtnClicked(int birdIndex)
    {
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (birds == null || birdIndex >= birds.Length) return;

        // 1. Durum: Kuş zaten açıksa -> SEÇ
        if (birds[birdIndex].isUnlocked)
        {
            PlayerPrefs.SetInt("SelectedBird", birdIndex);
            PlayerPrefs.Save();
            UpdateUI();
        }
        // 2. Durum: Kapalıysa ve para yetiyorsa -> SATIN AL
        else if (currentCoins >= birds[birdIndex].price)
        {
            // Parayı düş
            currentCoins -= birds[birdIndex].price;
            PlayerPrefs.SetInt("TotalCoins", currentCoins);

            // Kilidi aç
            PlayerPrefs.SetInt("Bird_" + birdIndex + "_Unlocked", 1);

            // Otomatik seç
            PlayerPrefs.SetInt("SelectedBird", birdIndex);

            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
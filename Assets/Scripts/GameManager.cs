using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Skor & Coin UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gameCoinText;     // Oyun içi coin yazısı
    [SerializeField] private TextMeshProUGUI gameOverCoinText; // Game Over panelindeki coin yazısı

    [Header("Paneller & Butonlar")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject reviveButton;

    private int score = 0;
    private int currentCoins = 0; // Bu tur toplananlar
    private int startTotalCoins = 0; // Cüzdandaki toplam para

    private void Awake()
    {
        if (Instance == null) Instance = this;
        // Sahne yüklenince zamanın aktığından emin ol
        Time.timeScale = 1f;
    }

    private void Start()
    {
        // 1. SES AYARINI YÜKLE
        // Eğer "IsMuted" 1 ise sesi kapat, 0 ise aç.
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        AudioListener.volume = isMuted ? 0 : 1;

        // 2. PARAYI YÜKLE
        startTotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // 3. REVIVE DURUMUNU KONTROL ET (Skoru geri yükle)
        if (PlayerPrefs.HasKey("TempScore"))
        {
            // Bu bir "Revive" başlangıcıdır
            score = PlayerPrefs.GetInt("TempScore");

            // Skoru ekrana yaz
            scoreText.text = score.ToString();

            // ZORLUK SEVİYESİNİ SENKRONİZE ET
            // Skor her 5'te zorluk artıyorsa, o ana kadar kaç kere arttığını hesapla ve uygula
            for (int i = 5; i <= score; i += 5)
            {
                if (DifficultyManager.Instance != null)
                {
                    DifficultyManager.Instance.IncreaseDifficulty();
                }
            }

            // Not: TempScore'u oyun bitince sileceğiz, şimdilik dursun.
        }
        else
        {
            // Bu tamamen "Yeni" bir oyundur
            score = 0;
            scoreText.text = "0";

            // Yeni oyun olduğu için eski revive hakkı kaydını temizle
            PlayerPrefs.DeleteKey("ReviveUsed");
        }

        UpdateCoinUI();
    }

    // Oyun içinde coin toplandığında çağrılır
    public void AddCoin()
    {
        currentCoins++;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        // Toplam bakiye + şu an toplananlar
        if (gameCoinText != null)
            gameCoinText.text = (startTotalCoins + currentCoins).ToString();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        // Her 5 skorda bir zorluğu arttır
        if (score % 5 == 0 && DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.IncreaseDifficulty();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Oyunu dondur
        gameOverCanvas.SetActive(true);

        int totalWallet = startTotalCoins + currentCoins;

        // --- REVIVE BUTONU KONTROLÜ ---
        // 1. Daha önce revive kullanıldı mı? (1=Evet)
        bool hasRevivedBefore = PlayerPrefs.GetInt("ReviveUsed", 0) == 1;

        // 2. Para yetiyor mu? (10 Coin)
        bool hasEnoughMoney = totalWallet >= 10;

        // Eğer hak kullanılmadıysa VE para yetiyorsa butonu göster
        if (!hasRevivedBefore && hasEnoughMoney)
        {
            reviveButton.SetActive(true);
        }
        else
        {
            reviveButton.SetActive(false);
        }

        // Parayı kaydet
        PlayerPrefs.SetInt("TotalCoins", totalWallet);
        PlayerPrefs.Save();

        // Game Over ekranındaki yazıları güncelle
        if (gameOverCoinText != null) gameOverCoinText.text = "+" + currentCoins.ToString();
        CheckHighScore();

        // TempScore artık işe yaramaz, silebiliriz (Restart veya Menu için)
        // Ancak ReviveWithCoin fonksiyonu yeniden oluşturacağı için sorun yok.
    }

    // REVIVE BUTONU BU FONKSİYONU ÇAĞIRIR
    public void ReviveWithCoin()
    {
        int total = PlayerPrefs.GetInt("TotalCoins", 0);

        if (total >= 10)
        {
            // 1. Parayı Düş
            total -= 10;
            PlayerPrefs.SetInt("TotalCoins", total);

            // 2. Mevcut Skoru Hafızaya At
            PlayerPrefs.SetInt("TempScore", score);

            // 3. "REVIVE KULLANILDI" İşaretini Koy (Bir daha çıkmasın)
            PlayerPrefs.SetInt("ReviveUsed", 1);

            PlayerPrefs.Save();

            // 4. Sahneyi Yeniden Yükle (En temiz yöntem)
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void CheckHighScore()
    {
        int high = PlayerPrefs.GetInt("HighScore", 0);
        if (score > high)
        {
            PlayerPrefs.SetInt("HighScore", score);
            high = score;
        }
        highScoreText.text = "Best: " + high;
    }

    // NORMAL "TEKRAR OYNA" BUTONU
    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Tüm geçici kayıtları sil (Sıfırdan başla)
        PlayerPrefs.DeleteKey("TempScore");
        PlayerPrefs.DeleteKey("ReviveUsed");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // MENÜYE DÖN BUTONU
    public void GoToMenu()
    {
        Time.timeScale = 1f;

        // Menüye dönerken de temizlik yap
        PlayerPrefs.DeleteKey("TempScore");
        PlayerPrefs.DeleteKey("ReviveUsed");

        SceneManager.LoadScene("Menu");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    private void Start()
    {
       
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + bestScore.ToString();
        }

        int wallet = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoinsText != null)
        {
            // Sadece sayiyi goster
            totalCoinsText.text = wallet.ToString();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
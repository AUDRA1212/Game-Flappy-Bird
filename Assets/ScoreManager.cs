using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    public TMP_Text scoreText;

    public int totalCoin = 0;
    public GameObject winPanel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        totalCoin = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
        if (winPanel != null)
            winPanel.SetActive(false);

        new GameObject("_LevelDisplay").AddComponent<LevelDisplay>();
        new GameObject("_ScreenEffects").AddComponent<ScreenEffects>();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();

        if (score >= totalCoin)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    void WinGame()
    {
        if (ScreenEffects.instance != null)
            ScreenEffects.instance.FlashScreen(Color.yellow, 0.4f);

        Time.timeScale = 0;

        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
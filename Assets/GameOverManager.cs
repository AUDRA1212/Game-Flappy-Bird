using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public void LoseAndRetry()
    {
        if (playerHealth != null)
            playerHealth.LoseHealth();

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called from Unity Button onClick (keep for compatibility)
    public void Retry()
    {
        LoseAndRetry();
    }

    public void Menu()
    {
        PlayerHealth.health = 3;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void SetGameOverActive(bool isGameOver) { }
}

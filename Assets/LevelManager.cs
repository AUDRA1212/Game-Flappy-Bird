using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxLevel = 6;

    public void NextLevel()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        if (!currentScene.StartsWith("Level"))
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        if (!int.TryParse(currentScene.Replace("Level", ""), out int level))
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        if (level >= maxLevel)
        {
            PlayerHealth.health = 3;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene("Level" + (level + 1));
        }
    }
}
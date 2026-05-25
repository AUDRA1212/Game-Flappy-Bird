using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        if (FindObjectOfType<MenuBeautifier>() == null)
            new GameObject("_MenuBeautifier").AddComponent<MenuBeautifier>();
    }

    // ke menu utama
    public void OpenMenu()
    {
        PlayerHealth.health = 3; // reset nyawa
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    // ke level berdasarkan angka
    public void OpenLevel(int levelId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level" + levelId);
    }
}
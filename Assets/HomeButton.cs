using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
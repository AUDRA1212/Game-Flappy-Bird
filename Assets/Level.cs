using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void OpenLevel(int levelid)
    {
        string levelname = "Level" + levelid;
        SceneManager.LoadScene(levelname);
    }
}

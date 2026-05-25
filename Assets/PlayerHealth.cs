using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int health = 3;

    public GameObject[] hearts;

    void Start()
    {
        UpdateHearts(); // biar UI update saja
    }

    public void LoseHealth()
    {
        if (health <= 0) return;

        health--;
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
        }
    }
}
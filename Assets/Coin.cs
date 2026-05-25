using UnityEngine;

public class Coin : MonoBehaviour
{
    public float speed = 2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!Bird.gameStarted || Bird.gameOver) return;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayCoinSound();
            ScoreManager.instance?.AddScore(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCoinSound();
            ScoreManager.instance?.AddScore(1);
            Destroy(gameObject);
        }
    }

    private void PlayCoinSound()
    {
        if (audioSource != null && audioSource.clip != null)
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, 2f);
    }
}
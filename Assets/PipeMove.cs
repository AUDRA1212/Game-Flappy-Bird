using UnityEngine;

public class PipeMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        if (!Bird.gameStarted || Bird.gameOver) return;

        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
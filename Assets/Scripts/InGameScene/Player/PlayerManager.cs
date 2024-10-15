using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health = 3;
    public GameObject ballPrefab;
    public Transform paddleTransform;
    public float ballOffsetY = 0.5f;
    private bool isAlive = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAlive)
        {
            LaunchBall();
        }
    }

    public void LaunchBall()
    {
        Vector2 spawnPosition = new Vector2(paddleTransform.position.x, paddleTransform.position.y + ballOffsetY);
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

    public void DecreaseHealthOnMiss()
    {
        if (!isAlive)
        {
            return;
        }

        health--;

        if (health <= 0)
        {
            HandlePlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        isAlive = false;
    }
}

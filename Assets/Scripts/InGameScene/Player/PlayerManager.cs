using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int health = 3;
    public GameObject ballPrefab;
    public Transform paddleTransform;
    public float ballOffsetY = 0.5f;
    public Image ballImage;
    private bool isAlive = true;
    private bool ballLaunched = false;
    private GameObject currentBall = null;

    private void Start()
    {
        ballLaunched = false;
        ballImage.gameObject.SetActive(true);

        FollowPaddleWithImage();
    }

    private void Update()
    {
        if (!ballLaunched)
        {
            FollowPaddleWithImage();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAlive && !ballLaunched)
        {
            LaunchBall();
        }
    }

    private void FollowPaddleWithImage()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(paddleTransform.position.x, paddleTransform.position.y + ballOffsetY));

        ballImage.rectTransform.position = screenPosition;
    }

    public void LaunchBall()
    {
        if (currentBall == null)
        {
            Vector2 spawnPosition = new Vector2(paddleTransform.position.x, paddleTransform.position.y + ballOffsetY);
            currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            ballImage.gameObject.SetActive(false);
            ballLaunched = true;
        }
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
        ballLaunched = false;
        currentBall = null;
        ballImage.gameObject.SetActive(true);
    }
}
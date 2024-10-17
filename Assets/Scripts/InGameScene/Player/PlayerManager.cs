using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int[] playerLifes = new int[2] { 3, 3 };
    [SerializeField] private Transform paddleTransform;
    [SerializeField] private float ballOffsetY = 0.5f;
    [SerializeField] private BallContainer ballContainer;

    private bool isAlive = true;
    private bool ballLaunched = false;
    private GameObject currentBall = null;

    public event Action<int> OnDeathEvent;

    private void Start()
    {
        ballLaunched = false;
        SpawnAndAttachBall();
    }

    private void Update()
    {
        if (!ballLaunched)
        {
            FollowPaddleWithBall();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAlive && !ballLaunched)
        {
            LaunchBall();
        }
    }

    private void SpawnAndAttachBall()
    {
        if (currentBall == null)
        {
            currentBall = ballContainer.SpawnFromPool("Ball");

            if (currentBall != null)
            {
                Vector2 spawnPosition = new Vector2(paddleTransform.position.x, paddleTransform.position.y + ballOffsetY);
                currentBall.transform.position = spawnPosition;
                currentBall.SetActive(true);
                ballLaunched = false;
            }
        }
    }

    private void FollowPaddleWithBall()
    {
        if (currentBall != null)
        {
            Vector2 paddlePosition = new Vector2(paddleTransform.position.x, paddleTransform.position.y + ballOffsetY);
            currentBall.transform.position = paddlePosition;
        }
    }

    public void LaunchBall()
    {
        if (currentBall != null)
        {
            Rigidbody2D ballRigidbody = currentBall.GetComponent<Rigidbody2D>();
            if (ballRigidbody != null)
            {
                Vector2 launchDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1f).normalized;
                ballRigidbody.velocity = launchDirection * 5;
                ballLaunched = true;
            }
        }
    }

    public void DecreasePlayerLife(int playerID)
    {
        int playerIndex = playerID - 1;

        if (!isAlive || playerIndex < 0 || playerIndex >= playerLifes.Length)
        {
            return;
        }

        playerLifes[playerIndex]--;

        if (playerLifes[playerIndex] <= 0)
        {
            HandlePlayerDeath(playerID);
        }
    }

    public void IncreasePlayerLife(int playerID)
    {
        int playerIndex = playerID - 1;

        if (playerIndex >= 0 && playerIndex < playerLifes.Length)
        {
            playerLifes[playerIndex]++;
        }
    }

    private void HandlePlayerDeath(int playerID)
    {
        isAlive = false;
        ballLaunched = false;
        currentBall = null;
        SpawnAndAttachBall();
        OnDeathEvent?.Invoke(playerID);
    }
}
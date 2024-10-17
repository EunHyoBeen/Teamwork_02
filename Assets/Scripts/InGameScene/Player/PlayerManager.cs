using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int[] playerLifes = new int[2] { 3, 3 };
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform paddleTransform;
    [SerializeField] private float ballOffsetY = 0.5f;
    [SerializeField] private Image ballImage;
    [SerializeField] private GameObject ballcontainer;

    private bool isAlive = true;
    private bool ballLaunched = false;
    private GameObject currentBall = null;

    public event Action<int> OnDeathEvent;

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
        ballImage.gameObject.SetActive(true);
        OnDeathEvent?.Invoke(playerID);
    }
}
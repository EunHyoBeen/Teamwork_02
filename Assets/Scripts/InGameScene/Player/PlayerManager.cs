using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int[] playerLifes = new int[2] { 3, 3 };
    [SerializeField] private Transform paddleTransform;
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private float ballOffsetY = 0.5f;
    [SerializeField] private BallContainer ballContainer;
    [SerializeField] private float arrowRotationSpeed = 45f;
    [SerializeField] private float maxRotationAngle = 45f;

    private bool isAlive = true;
    private bool ballLaunched = false;
    private bool rotatingRight = true;
    private GameObject currentBall = null;

    public event Action<int> OnDeathEvent;
    public event Action OnLaunchEvent;

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
            RotateArrow();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAlive && !ballLaunched)
        {
            LaunchBall();
        }
    }

    private void RotateArrow()
    {
        float currentAngle = arrowTransform.localEulerAngles.z;

        if (currentAngle > 180)
        {
            currentAngle -= 360;
        }

        if (rotatingRight)
        {
            currentAngle += arrowRotationSpeed * Time.deltaTime;
            if (currentAngle >= maxRotationAngle)
            {
                currentAngle = maxRotationAngle;
                rotatingRight = false;
            }
        }
        else
        {
            currentAngle -= arrowRotationSpeed * Time.deltaTime;
            if (currentAngle <= -maxRotationAngle)
            {
                currentAngle = -maxRotationAngle;
                rotatingRight = true;
            }
        }

        arrowTransform.localRotation = Quaternion.Euler(0, 0, currentAngle);
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

            arrowTransform.position = paddlePosition;
        }
    }

    public void LaunchBall()
    {
        if (currentBall != null)
        {
            Rigidbody2D ballRigidbody = currentBall.GetComponent<Rigidbody2D>();
            if (ballRigidbody != null)
            {
                Vector2 launchDirection = arrowTransform.up;

                //currentBall.GetComponent<BallController>.initializeBall(launchDirection);

                ballRigidbody.velocity = launchDirection * 5;

                arrowTransform.gameObject.SetActive(false);

                ballLaunched = true;

                OnLaunchEvent?.Invoke();
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
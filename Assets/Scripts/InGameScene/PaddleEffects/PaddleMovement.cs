using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    private Rigidbody2D movementRigidbody;
    private Vector2 movementDirection = Vector2.zero;

    [SerializeField] private float baseSpeed = 5f;
    public float speedMultiplier = 1f;

    public bool isStopped = false;

    private void Awake()
    {
        movementRigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void SetMovementDirection(Vector2 direction)
    {
        if (!isStopped)
        {
            movementDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (!isStopped)
        {
            ApplyMovement();
        }
        else
        {
            movementRigidbody.velocity = Vector2.zero;
        }
    }

    private void ApplyMovement()
    {
        Vector2 velocity = movementDirection * baseSpeed * speedMultiplier;
        movementRigidbody.velocity = velocity;
    }

    public void AdjustPaddleSpeed(float newSpeedMultiplier)
    {
        speedMultiplier = newSpeedMultiplier;
    }
}
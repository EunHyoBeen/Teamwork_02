using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    private Rigidbody2D movementRigidbody;
    private Vector2 movementDirection = Vector2.zero;

    public float baseSpeed = 5f;
    public float speedMultiplier = 1f;

    private void Awake()
    {
        movementRigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetMovementDirection(Vector2 direction)
    {
        movementDirection = direction;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
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

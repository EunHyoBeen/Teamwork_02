using UnityEngine;

public class PaddleSizeHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector2 originalSize;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = spriteRenderer.size;
    }

    public void AdjustPaddleSize(float widthChange)
    {
        float newWidth = Mathf.Clamp(spriteRenderer.size.x + widthChange, originalSize.x * 0.5f, originalSize.x * 2f);

        spriteRenderer.size = new Vector2(newWidth, originalSize.y);

        AdjustColliderSize();
    }

    private void AdjustColliderSize()
    {
        boxCollider.size = spriteRenderer.size;
        boxCollider.offset = spriteRenderer.bounds.center - transform.position;
    }

    public void ResetPaddleSize()
    {
        spriteRenderer.size = originalSize;
        AdjustColliderSize();
    }
}
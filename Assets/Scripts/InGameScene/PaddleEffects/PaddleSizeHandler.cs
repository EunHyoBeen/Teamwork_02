using UnityEngine;

public class PaddleSizeHandler : MonoBehaviour
{
    private Vector2 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void AdjustPaddleSize(float xMultiplier)
    {
        transform.localScale = new Vector2(originalScale.x * xMultiplier, originalScale.y);
    }
}
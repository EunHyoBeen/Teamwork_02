using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        _NONE = -1,

        BonusLife,
        PaddleSizeUp, PaddleSizeDown,
        PaddleSpeedUp, PaddleSpeedDown,
        BallPowerUp, BallSpeedUp, BallTriple,
        PaddleStopDebuff,

        _MAX
    }

    [SerializeField] private Sprite[] itemImages;

    public Type itemType { get; private set; }

    public void InitializeItem(float x, float y, Type _itemType, Vector2 initialSpeed)
    {
        itemType = _itemType;
        transform.position = new Vector3(x, y, 0);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (0 <= _itemType && _itemType < Type._MAX)
        {
            spriteRenderer.sprite = itemImages[(int)_itemType];
        }

        if (initialSpeed != Vector2.zero)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = initialSpeed;
        }
    }

    public void DestroyItem(bool used)
    {
        if (used)
        {
            // TODO : Fx효과
        }
        else
        {
            // TODO : Fx효과
        }

        Destroy(gameObject);
    }
}

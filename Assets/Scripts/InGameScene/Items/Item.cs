using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
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

    [SerializeField] private ItemImages itemImages;

    private static readonly int animRed = Animator.StringToHash("acquiredRed");
    private static readonly int animBlue = Animator.StringToHash("acquiredBlue");
    private static readonly int animWhite = Animator.StringToHash("acquiredWhite");
    private static readonly int animYellow = Animator.StringToHash("acquiredYellow");
    private static readonly int animDarkRed = Animator.StringToHash("acquiredDarkRed");
    private static readonly int animDarkBlue = Animator.StringToHash("acquiredDarkBlue");

    public Type itemType { get; private set; }

    public void InitializeItem(float x, float y, Type _itemType, Vector2 initialSpeed)
    {
        itemType = _itemType;
        transform.position = new Vector3(x, y, 0);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (0 <= _itemType && _itemType < Type._MAX)
        {
            spriteRenderer.sprite = itemImages.List[(int)_itemType];
        }

        if (initialSpeed != Vector2.zero)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = initialSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bottom"))
        {
            DestroyItem(false);
        }
    }

    public void DestroyItem(bool used)
    {
        if (used)
        {
            if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider)) circleCollider.enabled = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;

            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            switch (itemType)
            {
                case Type.BonusLife:
                    animator.SetTrigger(animWhite);
                    break;
                case Type.PaddleSizeUp:
                case Type.PaddleSpeedUp:
                    animator.SetTrigger(animBlue);
                    break;
                case Type.PaddleSizeDown:
                case Type.PaddleSpeedDown:
                    animator.SetTrigger(animDarkBlue);
                    break;
                case Type.BallPowerUp:
                case Type.BallSpeedUp:
                case Type.BallTriple:
                    animator.SetTrigger(animRed);
                    break;
                case Type.PaddleStopDebuff:
                    animator.SetTrigger(animDarkRed);
                    break;
            }

            Invoke("DestroyObject", 1f);
        }
        else
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}

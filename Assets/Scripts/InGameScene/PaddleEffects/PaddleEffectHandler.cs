using UnityEngine;

public class PaddleEffectHandler : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    private PaddleMovement paddleMovement;
    private PaddleSizeHandler paddleSizeHandler;
    

    private float speedEffectDuration = 0f;
    private float sizeEffectDuration = 0f;
    private float stopEffectDuration = 0f;

    private float originalSpeedMultiplier = 1f;
    private float originalSizeMultiplier = 1f;

    public int playerID;

    private void Start()
    {
        paddleMovement = GetComponent<PaddleMovement>();
        paddleSizeHandler = GetComponent<PaddleSizeHandler>();
    }

    private void Update()
    {
        if (speedEffectDuration > 0)
        {
            speedEffectDuration -= Time.deltaTime;
            if (speedEffectDuration <= 0f)
            {
                paddleMovement.AdjustPaddleSpeed(originalSpeedMultiplier);
            }
        }

        if (sizeEffectDuration > 0)
        {
            sizeEffectDuration -= Time.deltaTime;
            if (sizeEffectDuration <= 0f)
            {
                paddleSizeHandler.AdjustPaddleSize(originalSizeMultiplier);
            }
        }

        if (stopEffectDuration > 0)
        {
            stopEffectDuration -= Time.deltaTime;
            if (stopEffectDuration <= 0f)
            {
                paddleMovement.isStopped = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Item"))
        {
            Item item = target.GetComponent<Item>();

            if (item != null)
            {
                switch (item.itemType)
                {
                    case Item.Type.PaddleSpeedUp:
                        paddleMovement.AdjustPaddleSpeed(2.0f);
                        speedEffectDuration = 15.0f;
                        Debug.Log("스피드업");
                        break;

                    case Item.Type.PaddleSpeedDown:
                        paddleMovement.AdjustPaddleSpeed(0.5f);
                        speedEffectDuration = 15.0f;
                        Debug.Log("스피드다운");
                        break;

                    case Item.Type.PaddleSizeUp:
                        paddleSizeHandler.AdjustPaddleSize(1.5f);
                        sizeEffectDuration = 15.0f;
                        Debug.Log("사이즈업");
                        break;

                    case Item.Type.PaddleSizeDown:
                        paddleSizeHandler.AdjustPaddleSize(0.75f);
                        sizeEffectDuration = 15.0f;
                        Debug.Log("사이즈다운");
                        break;

                    case Item.Type.BallPowerUp:
                        Debug.Log("구현안함");
                        break;

                    case Item.Type.BallSpeedUp:
                        Debug.Log("구현안함");
                        break;

                    case Item.Type.BallTriple:
                        Debug.Log("구현안함");
                        break;

                    case Item.Type.PaddleStopDebuff:
                        paddleMovement.isStopped = true;
                        stopEffectDuration = 3.0f;
                        Debug.Log("멈춰");
                        break;

                    case Item.Type.BonusLife:
                        playerManager.IncreasePlayerLife(playerID);
                        Debug.Log("체력증가");
                        break;

                }
                item.DestroyItem(true);
            }
        }
    }
}
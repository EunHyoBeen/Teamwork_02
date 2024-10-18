using UnityEngine;

public class PaddleEffectHandler : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BallContainer ballContainer;
    private PaddleMovement paddleMovement;
    private PaddleSizeHandler paddleSizeHandler;

    private float paddleSpeedItemDuration = 0f;
    private float ballSpeedItemDuration = 0f;
    private float ballPowerItemDuration = 0f;
    private float stopItemDuration = 0f;

    public int playerID;

    private void Awake()
    {
        paddleMovement = GetComponent<PaddleMovement>();
        paddleSizeHandler = GetComponent<PaddleSizeHandler>();
    }

    private void Update()
    {
        HandleEffectDurations();
    }

    private void HandleEffectDurations()
    {
        HandleItemDuration(ref paddleSpeedItemDuration, () => paddleMovement.AdjustPaddleSpeed(1f), "패들 스피드 복구");
        HandleItemDuration(ref ballSpeedItemDuration, () => ballContainer.SpeedChange(-2), "공 스피드 복구");
        HandleItemDuration(ref ballPowerItemDuration, () => ballContainer.PowerChange(1), "공 파워 복구");
        HandleItemDuration(ref stopItemDuration, () => paddleMovement.ResumeMovement(), "패들 멈춤 해제");
    }

    private void HandleItemDuration(ref float duration, System.Action onEndEffect, string logMessage)
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0f)
            {
                onEndEffect?.Invoke();
                Debug.Log(logMessage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Item") && target.TryGetComponent(out Item item))
        {
            ApplyItemEffect(item);
            item.DestroyItem(true);
        }
    }

    private void ApplyItemEffect(Item item)
    {
        switch (item.itemType)
        {
            case Item.Type.PaddleSpeedUp:
                ApplyPaddleSpeedEffect(2.0f, 10f, "패들 스피드업");
                break;
            case Item.Type.PaddleSpeedDown:
                ApplyPaddleSpeedEffect(0.5f, 10f, "패들 스피드다운");
                break;
            case Item.Type.PaddleSizeUp:
                paddleSizeHandler.AdjustPaddleSize(0.33f);
                Debug.Log("패들 사이즈업");
                break;
            case Item.Type.PaddleSizeDown:
                paddleSizeHandler.AdjustPaddleSize(-0.33f);
                Debug.Log("패들 사이즈다운");
                break;
            case Item.Type.PaddleStopDebuff:
                ApplyPaddleStopEffect(3f);
                break;
            case Item.Type.BallPowerUp:
                ApplyBallPowerEffect(3, 10f);
                break;
            case Item.Type.BallSpeedUp:
                ApplyBallSpeedEffect(2, 10f);
                break;
            case Item.Type.BallTriple:
                ApplyBallTripleEffect();
                break;
            case Item.Type.BonusLife:
                playerManager.IncreasePlayerLife(playerID);
                Debug.Log("체력 증가");
                break;
        }
    }

    private void ApplyPaddleSpeedEffect(float speedMultiplier, float duration, string logMessage)
    {
        paddleMovement.AdjustPaddleSpeed(speedMultiplier);
        paddleSpeedItemDuration = duration;
        Debug.Log(logMessage);
    }

    private void ApplyBallPowerEffect(int powerChange, float duration)
    {
        ballContainer.PowerChange(powerChange);
        ballPowerItemDuration = duration;
        Debug.Log("공 파워업");
    }

    private void ApplyBallSpeedEffect(int speedChange, float duration)
    {
        ballContainer.SpeedChange(speedChange);
        ballSpeedItemDuration = duration;
        Debug.Log("공 스피드업");
    }

    private void ApplyBallTripleEffect()
    {
        ballContainer.MultiplyBalls(2);
        Debug.Log("공 2개 생성");
    }

    private void ApplyPaddleStopEffect(float duration)
    {
        paddleMovement.isStopped = true;
        stopItemDuration = duration;
        Debug.Log("패들 멈춤");
    }
}
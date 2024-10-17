using UnityEngine;

public class PaddleEffectHandler : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BallContainer ballContainer;
    [SerializeField] private ParticleSystem paddleEffectParticle;
    private PaddleMovement paddleMovement;
    private PaddleSizeHandler paddleSizeHandler;

    private float ballSpeedEffectDuration = 0f;
    private float ballPowerEffectDuration = 0f;
    private float stopEffectDuration = 0f;

    private bool isEffectActive = false; // 파티클 상태 확인

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

    // 파티클 시작
    private void StartPaddleEffect()
    {
        if (paddleEffectParticle != null && !paddleEffectParticle.isPlaying)
        {
            paddleEffectParticle.Play();
            isEffectActive = true;
        }
    }

    // 파티클 중지
    private void StopPaddleEffect()
    {
        if (paddleEffectParticle != null && paddleEffectParticle.isPlaying)
        {
            paddleEffectParticle.Stop();
            isEffectActive = false;
        }
    }

    private void HandleEffectDurations()
    {
        if (ballSpeedEffectDuration > 0)
        {
            ballSpeedEffectDuration -= Time.deltaTime;
            if (ballSpeedEffectDuration <= 0f)
            {
                ballContainer.SpeedChange(-2);
                Debug.Log("볼 스피드 복구");
                CheckAndStopEffect();
            }
        }

        if (ballPowerEffectDuration > 0)
        {
            ballPowerEffectDuration -= Time.deltaTime;
            if (ballPowerEffectDuration <= 0f)
            {
                ballContainer.PowerChange(-1);
                Debug.Log("볼 파워 복구");
                CheckAndStopEffect();
            }
        }

        if (stopEffectDuration > 0)
        {
            stopEffectDuration -= Time.deltaTime;
            if (stopEffectDuration <= 0f)
            {
                paddleMovement.isStopped = false;
                Debug.Log("패들 다시 움직임");
                CheckAndStopEffect();
            }
        }
    }

    // 파티클 중지 조건 확인
    private void CheckAndStopEffect()
    {
        // 모든 효과가 종료되었는지 확인하고 파티클을 중지
        if (ballSpeedEffectDuration <= 0f && ballPowerEffectDuration <= 0f && stopEffectDuration <= 0f)
        {
            StopPaddleEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Item"))
        {
            Item item = target.GetComponent<Item>();

            if (item != null)
            {
                Vector3 itemPosition = target.transform.position;
                float distanceToThisPaddle = Vector3.Distance(transform.position, itemPosition);

                PaddleEffectHandler[] allPaddles = FindObjectsOfType<PaddleEffectHandler>();
                foreach (var paddleEffect in allPaddles)
                {
                    if (paddleEffect != this)
                    {
                        float distanceToOtherPaddle = Vector3.Distance(paddleEffect.transform.position, itemPosition);

                        if (distanceToOtherPaddle < distanceToThisPaddle)
                        {
                            Debug.Log("다른 패들이 더 가까워 아이템을 먹지 않음");
                            return;
                        }
                    }
                }

                Debug.Log($"패들 {playerID}가 아이템을 먹음");
                ApplyItemEffect(item);
                item.DestroyItem(true);
            }
        }
    }

    private void ApplyItemEffect(Item item)
    {
        // 새로운 아이템을 먹을 때마다 파티클 실행
        StartPaddleEffect();

        switch (item.itemType)
        {
            case Item.Type.PaddleSpeedUp:
                paddleMovement.AdjustPaddleSpeed(2.0f);
                Debug.Log("스피드업");
                break;

            case Item.Type.PaddleSpeedDown:
                paddleMovement.AdjustPaddleSpeed(0.5f);
                Debug.Log("스피드다운");
                break;

            case Item.Type.PaddleSizeUp:
                paddleSizeHandler.AdjustPaddleSize(1.5f);
                Debug.Log("사이즈업");
                break;

            case Item.Type.PaddleSizeDown:
                paddleSizeHandler.AdjustPaddleSize(0.75f);
                Debug.Log("사이즈다운");
                break;

            case Item.Type.BallPowerUp:
                ballContainer.PowerChange(1);
                ballPowerEffectDuration = 10.0f;
                Debug.Log("볼 파워업");
                break;

            case Item.Type.BallSpeedUp:
                ballContainer.SpeedChange(2);
                ballSpeedEffectDuration = 10.0f;
                Debug.Log("볼 스피드업");
                break;

            case Item.Type.BallTriple:
                ballContainer.MultiplyBalls(2);
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
    }
}

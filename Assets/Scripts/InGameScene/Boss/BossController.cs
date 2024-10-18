using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private ItemContainer itemContainer;
    //[SerializeField] private ClassName healthBar;

    private SpriteRenderer image;
    private Canvas canvas;
    private TextMeshProUGUI healthTxt;

    private int stageIndex;

    // 상태 관련
    private int maxHealth;
    private int health;
    private bool invincibleState;

    // 페이즈
    private int totalActionPhase;
    private int actionPhase;
    private float remainingTimeToNextPhase;
    private Action enteringPhase;
    private bool actionStopped;

    // 이동 관련
    private float moveSpeed;
    private Vector2 moveDestination;
    private Vector2 moveVelocity;
    private MovingType moveType;

    // 사망 관련
    public event Action OnBossBreak;
    private bool alreadyDestroyed;

    public void InitializeBoss(int _stageIndex)
    {
        stageIndex = _stageIndex;

        image = GetComponent<SpriteRenderer>();
        //canvas = transform.GetChild(0).GetComponent<Canvas>();
        //healthTxt = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        switch (stageIndex)
        {
            case 10:
                maxHealth = 100;
                SetHealth(maxHealth);

                invincibleState = false;
                totalActionPhase = 10;
                remainingTimeToNextPhase = 5;

                moveSpeed = 1f;

                enteringPhase = BossActionStage10;
                break;

            default:
                totalActionPhase = 1;
                remainingTimeToNextPhase = 100;
                break;
        }
        actionPhase = 0;

        actionStopped = true;
    }
    public void ActionStop(bool _spawnStopped)
    {
        actionStopped = _spawnStopped;
    }

    // 시간 지나면 다음 페이즈로
    private void Update()
    {
        if (actionStopped) return;

        remainingTimeToNextPhase -= Time.deltaTime;

        if (remainingTimeToNextPhase < 0)
        {
            actionPhase++;
            if (actionPhase >= totalActionPhase) actionPhase = 0;

            enteringPhase?.Invoke();
        }
    }

    private void BossActionStage10()
    {
        switch (actionPhase)
        {
            case 0: // 대기, 무적 해제

                remainingTimeToNextPhase = 5;
                break;
            case 1: // Paddle Stop Debuff 폭격
                StartCoroutine(CallItemCreationRepeatedly(Item.Type.PaddleStopDebuff, 0.5f, 5, 0, 3.36f, -5, 5, 0, -0.5f));
                remainingTimeToNextPhase = 3;
                break;
            case 2: // 대기
                remainingTimeToNextPhase = 5;
                break;
            case 3: // 우로이동

                remainingTimeToNextPhase = 1;
                break;
            case 4: // 좌로 이동
                remainingTimeToNextPhase = 2;
                break;
            case 5: // 우로 이동(원위치로)
                remainingTimeToNextPhase = 1;
                break;
            case 6: // 대기
                remainingTimeToNextPhase = 5;
                break;
            case 7: // Paddle Size Down 폭격
                StartCoroutine(CallItemCreationRepeatedly(Item.Type.PaddleSizeDown, 0.1f, 25, 0, 3.36f, -5, 5, 0, -0.5f));
                remainingTimeToNextPhase = 3;
                break;
            case 8: // 대기
                remainingTimeToNextPhase = 5;
                break;
            case 9: // 무적
                remainingTimeToNextPhase = 3;
                break;
        }
    }

    IEnumerator CallItemCreationRepeatedly(Item.Type itemType, float interval, int count, float x, float y, float speedXmin, float speedXmax, float speedYmin, float speedYmax) 
    {
        for (int i = 0; i < count; i++)
        {
            float speedX = UnityEngine.Random.Range(speedXmin, speedXmax);
            float speedY = UnityEngine.Random.Range(speedYmin, speedYmax);
            itemContainer.ItemCreation(itemType, x, y, new Vector2(speedX, speedY));

            yield return new WaitForSeconds(interval);
        }
    }

    private void SetMove(Vector2 destination, MovingType movingType)
    {
        moveDestination = destination;
        moveType = movingType;
    }

    private void SetHealth(int value)
    {
        value = Mathf.Clamp(value, 0, maxHealth);
        health = value;

        if (health > 0)
        {
            // 체력 닳을 시의 동작 추가
            //healthTxt.text = health.ToString();
        }
        else
        {
            if (alreadyDestroyed == false)
            {
                alreadyDestroyed = true;

                // 파괴됐을 시의 동작 추가
                canvas.gameObject.SetActive(false);
                if (TryGetComponent<PolygonCollider2D>(out PolygonCollider2D boxCollider)) boxCollider.enabled = false;
                GetComponent<Animator>().SetTrigger("bossBreak");
                OnBossBreak?.Invoke();
                Invoke("DestroyObject", 1f);
            }
        }
    }
    private void DestroyObject()
    {
        itemContainer.Clear();
        Destroy(gameObject);
    }

    public void GetDamage(int damage = 1)
    {
        if (invincibleState || alreadyDestroyed) return;

        SetHealth(health - damage);
        // TODO : 피격시 효과
    }


    private enum MovingType
    {
        Constant,
        SmoothDamp
    }
}

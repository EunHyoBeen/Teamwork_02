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
    private Animator animator;

    private int stageIndex;

    // ���� ����
    private int maxHealth;
    private int health;
    private bool invincibleState;

    // ������
    private int totalActionPhase;
    private int actionPhase;
    private float remainingTimeToNextPhase;
    private Action enteringPhase;
    private bool actionStopped;

    // �̵� ����
    private float moveSpeed;
    private MovingType moveType;
    private Vector2 moveVelocity;
    private float moveMaxSpeed;
    private Vector2 moveDestination;

    // ��� ����
    public event Action OnBossBreak;
    private bool alreadyDestroyed;

    public void InitializeBoss(int _stageIndex)
    {
        stageIndex = _stageIndex;

        image = GetComponent<SpriteRenderer>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        healthTxt = canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();

        switch (stageIndex)
        {
            case 10:
                maxHealth = 50;
                SetHealth(maxHealth);

                invincibleState = false;
                totalActionPhase = 10;
                remainingTimeToNextPhase = 5;

                moveSpeed = 1f;
                moveType = MovingType.Constant;

                enteringPhase = BossActionStage10;
                break;

            case 20:
                maxHealth = 100;
                SetHealth(maxHealth);

                invincibleState = false;
                totalActionPhase = 10;
                remainingTimeToNextPhase = 5;

                moveSpeed = 1f;
                moveType = MovingType.Constant;

                enteringPhase = BossActionStage10;
                break;

            default:
                totalActionPhase = 1;
                remainingTimeToNextPhase = 100;
                break;
        }
        actionPhase = 0;

        actionStopped = true;

        moveDestination = transform.position;
    }
    public void ActionStop(bool _actionStopped)
    {
        actionStopped = _actionStopped;
    }

    private void Update()
    {
        if (actionStopped) return;

        // �ð� ������ ���� �������
        remainingTimeToNextPhase -= Time.deltaTime;
        if (remainingTimeToNextPhase < 0)
        {
            actionPhase++;
            if (actionPhase >= totalActionPhase) actionPhase = 0;

            enteringPhase?.Invoke();
        }

        // ���� �̵�
        if (moveType == MovingType.Constant)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveDestination, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, moveDestination, ref moveVelocity, moveMaxSpeed);
        }
    }

    private void BossActionStage10()
    {
        switch (actionPhase)
        {
            case 0: // ���, ���� ����
                invincibleState = false;
                animator.SetBool("isInvincible", false);
                remainingTimeToNextPhase = 5;
                break;
            case 1: // Paddle Stop Debuff ����
                StartCoroutine(CallItemCreationRepeatedly(Item.Type.PaddleStopDebuff, 0.5f, 5, 0, 3.36f, -5, 5, 0, -0.5f));
                remainingTimeToNextPhase = 3;
                break;
            case 2: // ���
                remainingTimeToNextPhase = 5;
                break;
            case 3: // ����̵�
                moveDestination = new Vector2(transform.position.x + 1, transform.position.y);
                remainingTimeToNextPhase = 1;
                break;
            case 4: // �·� �̵�
                moveDestination = new Vector2(transform.position.x - 2, transform.position.y);
                remainingTimeToNextPhase = 2;
                break;
            case 5: // ��� �̵�(����ġ��)
                moveDestination = new Vector2(transform.position.x + 1, transform.position.y);
                remainingTimeToNextPhase = 1;
                break;
            case 6: // ���
                remainingTimeToNextPhase = 5;
                break;
            case 7: // Paddle Size Down ����
                StartCoroutine(CallItemCreationRepeatedly(Item.Type.PaddleSizeDown, 0.1f, 25, 0, 3.36f, -5, 5, 0, -0.5f));
                remainingTimeToNextPhase = 3;
                break;
            case 8: // ���
                remainingTimeToNextPhase = 5;
                break;
            case 9: // ����
                invincibleState = true;
                animator.SetBool("isInvincible", true);
                remainingTimeToNextPhase = 3.35f;
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
            Debug.Log(health);
            // ü�� ���� ���� ���� �߰�
            healthTxt.text = health.ToString();
        }
        else
        {
            if (alreadyDestroyed == false)
            {
                alreadyDestroyed = true;

                actionStopped = true;

                // �ı����� ���� ���� �߰�
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
        // TODO : �ǰݽ� ȿ��
    }


    private enum MovingType
    {
        Constant,
        SmoothDamp
    }
}

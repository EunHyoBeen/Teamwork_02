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
    private Vector2 moveDestination;
    private Vector2 moveVelocity;
    private MovingType moveType;

    // ��� ����
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

    // �ð� ������ ���� �������
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
            case 0: // ���, ���� ����

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

                remainingTimeToNextPhase = 1;
                break;
            case 4: // �·� �̵�
                remainingTimeToNextPhase = 2;
                break;
            case 5: // ��� �̵�(����ġ��)
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
            // ü�� ���� ���� ���� �߰�
            //healthTxt.text = health.ToString();
        }
        else
        {
            if (alreadyDestroyed == false)
            {
                alreadyDestroyed = true;

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LayerMask paddleLayer;
    [SerializeField] private LayerMask bottomLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private LayerMask wallLayer;
    private float initialspeed;
    private float speed;
    //private PlayerManager player;
    private Vector2 direction;
    private int power = 1;
    private Rigidbody2D rb2d;


    [SerializeField][Range(1f, 20f)] private float threshold = 5f;
    [SerializeField][Range(1f, 10f)] private float rotateAngle = 3f;

    private void Awake()
    {
        InitializeBall();   
        rb2d = GetComponent<Rigidbody2D>();
        
    }


    private void OnEnable()
    {
        InitializeBall();
        rb2d.velocity = direction * speed;
    }
    private void Update()                       // ���� ���� ������ ���� �ӵ��� ���������� ���� �ӵ��� ����
    {
        if(rb2d.velocity.magnitude < speed)
        {
            rb2d.velocity = rb2d.velocity.normalized * speed;
        }

    }
    public void SetInitialSpeed(float speed)
    {
        initialspeed = speed;
    }

    public void InitializeBall()
    {
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);
        direction = new Vector2(randomX, randomY).normalized;
        speed = initialspeed;
    }

    public void SpeedChange(float changeSpeed)
    {
        if(changeSpeed > 0 && speed != initialspeed) { return; }        // �̹� ���ǵ���� ���� ����
        direction = rb2d.velocity.normalized;
        speed += changeSpeed;
        rb2d.velocity = direction * speed;
    }

    public void PowerChange(int changePower)
    {
        power = changePower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsLayerMatched(paddleLayer, collision.gameObject.layer))       // �е�� �浹 �� ���� ��ȯ, ���ľ� ��
        {
            direction = DirectionAfterPaddleCollision(collision.transform);
            rb2d.velocity = direction * speed;
        }

        else if (IsLayerMatched(bottomLayer, collision.gameObject.layer))         // �ٴڰ� �浹 �� �����
        {
            gameObject.SetActive(false);

        }

        else                                                                // ���� ���� �浹 �� ������ȯ
        {
            Vector2 normal = collision.contacts[0].normal;                  // �浹�� ���� ����
            direction = DirectionAfterCollision(normal);                    // �ݻ��ؼ� ������ ������ Vector2�� ����

            rb2d.velocity = direction * speed;
        }


        if(IsLayerMatched(wallLayer, collision.gameObject.layer))           // ���� �浹 �� �̼� ���� ����
        {
            direction = rb2d.velocity.normalized;
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            AdjustDirectionWhenHorizon(direction, degree, threshold);       // �ʹ� ���� ����� �� ������ ���� ����
            rb2d.velocity = direction * speed;
        }
        
        if (IsLayerMatched(blockLayer, collision.gameObject.layer))         // ���� �浹 �� ���� ������
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block.GetDamage(power);
        }

    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    private Vector2 DirectionAfterPaddleCollision(Transform transform)          // �е�� �浹 ���� ���� ����
    {
        float difX = this.transform.position.x - transform.position.x;
        difX = transform.localScale.x / 2 - difX;
        float rad = Mathf.Clamp((difX / transform.localScale.x) * 180, 3, 177) * Mathf.Deg2Rad;
        float newdirectionX = Mathf.Cos(rad);
        float newdirectionY = Mathf.Sin(rad);
        if (this.transform.position.y > transform.position.y)
        {
            return new Vector2(newdirectionX, newdirectionY).normalized;
        }
        return new Vector2(newdirectionX, -newdirectionY).normalized;
        
    }

    private Vector2 DirectionAfterCollision(Vector2 normal)       // �浹 �� ���� ���
    {
        float angleBetweenVectors = getAngle(-direction, normal);
        Vector2 reflectDirection = Rotate(-direction, angleBetweenVectors + Mathf.Clamp(angleBetweenVectors, -90 + threshold, 90 - threshold)).normalized;  // �ִ� �ݻ簢�� 90-Threshold�� ����
        return reflectDirection;
    }

    private float getAngle(Vector2 vec1, Vector2 vec2)      // �� ���� ������ ���� ���ϴ� �Լ�
    {
        float angleRad = (Mathf.Atan2(vec2.y, vec2.x) - Mathf.Atan2(vec1.y, vec1.x));
        return angleRad;
    }

    private Vector2 Rotate(Vector2 vec, float rad)       // ���͸� radian��ŭ ȸ���ϴ� �Լ�
    {
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
    }

    private Vector2 AdjustDirectionWhenHorizon(Vector2 direction, float degree, float threshold)
    {
        if (degree > 180 - this.threshold || degree < 0 && degree > -this.threshold)      // ���� ������ ��ƾ��Ҷ� (180���� �����ų�, �����߿� 0���� ����� ��)
        {
            return Rotate(direction, -rotateAngle * Mathf.Deg2Rad);
        }
        else // if (degree < -180 + Threshold || degree < Threshold && degree >= 0)   // ���� ������ +�ؾ��Ҷ� (-180���� �����ų�, ����߿� 0���� ����� ��)
        {
            return Rotate(direction, rotateAngle * Mathf.Deg2Rad);
        }
    }
}

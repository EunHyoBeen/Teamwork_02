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
    private void Update()                       // 공이 물리 엔진에 의해 속도가 내려갔을때 원래 속도로 돌림
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
        if(changeSpeed > 0 && speed != initialspeed) { return; }        // 이미 스피드업을 먹은 상태
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
        if (IsLayerMatched(paddleLayer, collision.gameObject.layer))       // 패들과 충돌 시 방향 전환, 고쳐야 함
        {
            direction = DirectionAfterPaddleCollision(collision.transform);
            rb2d.velocity = direction * speed;
        }

        else if (IsLayerMatched(bottomLayer, collision.gameObject.layer))         // 바닥과 충돌 시 사라짐
        {
            gameObject.SetActive(false);

        }

        else                                                                // 벽과 블럭에 충돌 시 방향전환
        {
            Vector2 normal = collision.contacts[0].normal;                  // 충돌시 법선 구함
            direction = DirectionAfterCollision(normal);                    // 반사해서 나오는 방향을 Vector2로 구함

            rb2d.velocity = direction * speed;
        }


        if(IsLayerMatched(wallLayer, collision.gameObject.layer))           // 벽에 충돌 시 미세 각도 조정
        {
            direction = rb2d.velocity.normalized;
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            AdjustDirectionWhenHorizon(direction, degree, threshold);       // 너무 수평에 가까울 시 각도를 조금 조정
            rb2d.velocity = direction * speed;
        }
        
        if (IsLayerMatched(blockLayer, collision.gameObject.layer))         // 블럭과 충돌 시 블럭에 데미지
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block.GetDamage(power);
        }

    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    private Vector2 DirectionAfterPaddleCollision(Transform transform)          // 패들과 충돌 이후 방향 설정
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

    private Vector2 DirectionAfterCollision(Vector2 normal)       // 충돌 후 방향 계산
    {
        float angleBetweenVectors = getAngle(-direction, normal);
        Vector2 reflectDirection = Rotate(-direction, angleBetweenVectors + Mathf.Clamp(angleBetweenVectors, -90 + threshold, 90 - threshold)).normalized;  // 최대 반사각을 90-Threshold로 잡음
        return reflectDirection;
    }

    private float getAngle(Vector2 vec1, Vector2 vec2)      // 두 벡터 사이의 각을 구하는 함수
    {
        float angleRad = (Mathf.Atan2(vec2.y, vec2.x) - Mathf.Atan2(vec1.y, vec1.x));
        return angleRad;
    }

    private Vector2 Rotate(Vector2 vec, float rad)       // 벡터를 radian만큼 회전하는 함수
    {
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
    }

    private Vector2 AdjustDirectionWhenHorizon(Vector2 direction, float degree, float threshold)
    {
        if (degree > 180 - this.threshold || degree < 0 && degree > -this.threshold)      // 공의 각도를 깎아야할때 (180도에 가깝거나, 음수중에 0도에 가까울 때)
        {
            return Rotate(direction, -rotateAngle * Mathf.Deg2Rad);
        }
        else // if (degree < -180 + Threshold || degree < Threshold && degree >= 0)   // 공의 각도를 +해야할때 (-180도에 가깝거나, 양수중에 0도에 가까울 때)
        {
            return Rotate(direction, rotateAngle * Mathf.Deg2Rad);
        }
    }
}

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
    [SerializeField][Range(0f, 20f)] private float initialspeed = 5f;
    private float speed;
    private PlayerManager player;
    private Vector2 direction;
    private int power = 1;
    private Rigidbody2D rb2d;
    //private bool isShooting = false;

    public event Action OnDeath;

    [SerializeField][Range(1f, 20f)] private float Threshold = 5f;
    [SerializeField][Range(1f, 10f)] private float rotateAngle = 3f;

    private void Awake()
    {
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);
        direction = new Vector2 (randomX, randomY).normalized;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = initialspeed;
        rb2d.velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        if(rb2d.velocity.magnitude < speed)
        {
            rb2d.velocity = rb2d.velocity.normalized * speed;
        }
    }

    public void SpeedChange(float changeSpeed)
    {
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

        else if (IsLayerMatched(bottomLayer, collision.gameObject.layer))         // 바닥과 충돌 시 사라지고 남은 공 계산
        {
            this.gameObject.SetActive(false);   // 오브젝트 풀링
            OnDeath?.Invoke();
            //player.ballCount--
            //if(player.ballCount == 0) player.life--;
            // 블록은 남기고 게임 다시 시작
        }
        else                                                                // 벽과 블럭에 충돌 시 방향전환
        {
            Vector2 normal = collision.contacts[0].normal;                  // 충돌시 법선 구함
            direction = DirectionAfterCollision(normal);                    // 반사해서 나오는 방향을 Vector2로 구함

            rb2d.velocity = direction * speed;
        }
        if(IsLayerMatched(wallLayer, collision.gameObject.layer))
        {
            direction = rb2d.velocity.normalized;
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (degree > 180 - Threshold || degree < 0 && degree > -Threshold)      // 공의 각도를 깎아야할때 (180도에 가깝거나, 음수중에 0도에 가까울 때)
            {
                direction = Rotate(direction, -rotateAngle * Mathf.Deg2Rad);
            }
            else if(degree < -180 + Threshold || degree < Threshold && degree >= 0)   // 공이 각도를 +해야할때 (-180도에 가깝거나, 양수중에 0도에 가까울떄)
            {
                direction = Rotate(direction, rotateAngle * Mathf.Deg2Rad);
            }
            rb2d.velocity = direction * speed;
        }
        
        if (IsLayerMatched(blockLayer, collision.gameObject.layer))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block.GetDamage(power);
        }

    }

    private Vector2 DirectionAfterPaddleCollision(Transform transform)
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

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    private Vector2 DirectionAfterCollision(Vector2 normal)       // 충돌 후 방향 계산
    {
        float angleBetweenVectors = getAngle(-direction, normal);
        //if (Mathf.Abs(angleBetweenVectors) > Mathf.PI / 2 && Mathf.Abs(angleBetweenVectors) < Mathf.PI * 3 / 2)
        //{
        //    Debug.Log($"Error : {angleBetweenVectors * Mathf.Rad2Deg}");
        //}

        Vector2 reflectDirection = Rotate(-direction, angleBetweenVectors + Mathf.Clamp(angleBetweenVectors, -90 + Threshold, 90 - Threshold)).normalized;  // 최대 반사각을 90-Threshold로 잡음
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

}

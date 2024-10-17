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
    [SerializeField][Range(0f, 20f)]private float initialspeed = 5f;
    private float speed;
    private PlayerManager player;
    private Vector2 direction;
    private int power;
    private Rigidbody2D rb2d;
    private bool isShooting = false;
    public event Action OnDeath; 
    private void Awake()
    {
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(0f, 1f);
        direction = new Vector2 (randomX, randomY).normalized;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = initialspeed;
        rb2d.velocity = direction * speed;
    }

    //private void Update()
    //{
    //    if (!isShooting)
    //    {
    //        transform.position = 
    //    }
    //}

    public void SpeedChange(float changeSpeed)
    {
        direction = rb2d.velocity.normalized;
        speed += changeSpeed;
        rb2d.velocity = direction * speed;
    }

    public void PowerChange(int changePower)
    {
        power += changePower;
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
        //else                                                                 // 벽과 블럭에 충돌 시 방향전환
        //{
        //    Vector2 normal = collision.contacts[0].normal;                  // 충돌시 법선 구함
        //    direction = DirectionAfterCollision(normal);                    // 반사해서 나오는 방향을 Vector2로 구함

        //    rb2d.velocity = direction * speed;
        //}
        if (IsLayerMatched(blockLayer, collision.gameObject.layer))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block.GetDamage(1);
        }

    }

    private Vector2 DirectionAfterPaddleCollision(Transform transform)
    {
        float difX = this.transform.position.x - transform.position.x;
        difX = transform.localScale.x / 2 - difX;
        float rad = (difX / transform.localScale.x) * 180 * Mathf.Deg2Rad;
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
    
    //private Vector2 DirectionAfterCollision(Vector2 normal)       // 충돌 후 방향 계산
    //{
    //    float angleBetweenVectors = getAngle(-direction, normal);
    //    if(Mathf.Abs(angleBetweenVectors) > Mathf.PI / 2 && Mathf.Abs(angleBetweenVectors) < Mathf.PI * 3 / 2)
    //    {
    //        Debug.Log($"Error : {angleBetweenVectors * Mathf.Rad2Deg}");
    //    }
        
    //    Vector2 reflectDirection = Rotate(-direction , 2 * angleBetweenVectors).normalized;
    //    return reflectDirection;
    //}

    //private float getAngle(Vector2 vec1, Vector2 vec2)      // 두 벡터 사이의 각을 구하는 함수
    //{

    //    float angleRad = (Mathf.Atan2(vec2.y, vec2.x) - Mathf.Atan2(vec1.y, vec1.x));
    //    return angleRad;
    //}

    //private Vector2 Rotate(Vector2 vec, float degree)       // 벡터를 degree만큼 회전하는 함수
    //{
    //    float sin = Mathf.Sin(degree);
    //    float cos = Mathf.Cos(degree);
    //    return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
    //}

}

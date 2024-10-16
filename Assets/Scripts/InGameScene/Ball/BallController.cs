using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LayerMask paddleLayer;
    [SerializeField] private LayerMask bottomLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField][Range(0f, 20f)]private float speed = 5f;
    //private Player player;
    private Vector2 direction;
    private int power;
    private Rigidbody2D rb2d;
    private bool isShooting = false;

    private void Awake()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(0f, 1f);
        direction = new Vector2 (randomX, randomY).normalized;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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
        speed += changeSpeed;
        rb2d.velocity = direction * speed;
    }

    public void PowerChange(int changePower)
    {
        power += changePower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsLayerMatched(paddleLayer, collision.gameObject.layer))       // �е�� �浹 �� ���� ��ȯ, ���ľ� ��
        {
            direction = DirectionAfterPaddleCollision(collision.transform);
            rb2d.velocity = direction * speed;
        }

        else if (IsLayerMatched(bottomLayer, collision.gameObject.layer))         // �ٴڰ� �浹 �� ������� ���� �� ���
        {
            this.gameObject.SetActive(false);   // ������Ʈ Ǯ��
            //player.ballCount--
            //if(player.ballCount == 0) player.life--;
            // ����� ����� ���� �ٽ� ����
        }
        //else                                                                 // ���� ���� �浹 �� ������ȯ
        //{
        //    Vector2 normal = collision.contacts[0].normal;                  // �浹�� ���� ����
        //    direction = DirectionAfterCollision(normal);                    // �ݻ��ؼ� ������ ������ Vector2�� ����

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
        float difY = this.transform.position.y - transform.position.y;
        return new Vector2(difX, difY).normalized;
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
    
    private Vector2 DirectionAfterCollision(Vector2 normal)
    {
        float angleBetweenVectors = getAngle(-direction, normal);
        if(Mathf.Abs(angleBetweenVectors) > Mathf.PI / 2 && Mathf.Abs(angleBetweenVectors) < Mathf.PI * 3 / 2)
        {
            Debug.Log($"Error : {angleBetweenVectors * Mathf.Rad2Deg}");
        }
        
        Vector2 reflectDirection = Rotate(-direction , 2 * angleBetweenVectors).normalized;
        return reflectDirection;
    }

    private float getAngle(Vector2 vec1, Vector2 vec2)      // �� ���� ������ ���� ���ϴ� �Լ�
    {

        float angleRad = (Mathf.Atan2(vec2.y, vec2.x) - Mathf.Atan2(vec1.y, vec1.x));
        return angleRad;
    }

    private Vector2 Rotate(Vector2 vec, float degree)       // ���͸� degree��ŭ ȸ���ϴ� �Լ�
    {
        float sin = Mathf.Sin(degree);
        float cos = Mathf.Cos(degree);
        return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
    }

}

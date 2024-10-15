using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LayerMask paddleLayer;
    [SerializeField] private LayerMask bottomLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField][Range(0f, 1000f)]private float speed = 5f;
    //private Player player;
    private Vector2 direction;
    private int damage;
    private Rigidbody2D rb2d;
    

    private void Awake()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        direction = new Vector2 (randomX, randomY).normalized;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb2d.velocity = direction * speed;
    }


    public void SetSpeed(float speed)
    {
        this.speed = speed;
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

        else if (IsLayerMatched(blockLayer, collision.gameObject.layer))
        {
            //Block blockk = collision.gameObject.GetComponent<Block>();
            //blockk.getDamaged(damage);
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
    
}

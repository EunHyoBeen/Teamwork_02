using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
    [SerializeField] private LayerMask blockGrinderLayer;

    private static readonly int maxHealth = 10;
    private static readonly Color[] healthColor = new Color[]
        { new Color(0.8f, 0.8f, 0.8f), new Color(0.6f, 0.6f, 0.9f), new Color(0.0f, 1.0f, 0.5f), new Color(0.1f, 0.7f, 0.1f), new Color(0.7f, 0.7f, 0.0f),
          new Color(1.0f, 0.5f, 0.0f), new Color(1.0f, 0.0f, 0.0f), new Color(0.5f, 0.0f, 0.5f), new Color(0.2f, 0.2f, 0.8f), new Color(0.2f, 0.2f, 0.2f) };
    private Vector2 movingVector;

    private SpriteRenderer image;
    private Canvas canvas;
    private TextMeshProUGUI healthTxt;

    public event Action<float, float> OnBreak;
    private int health;
    private bool alreadyDestroyed; // 중복 파괴 방지

    private bool invincible;
    
    public void InitializeBlock(float x, float y, int initialHealth, Vector2 speed)
    {
        if (initialHealth <= 0) return;

        image = GetComponent<SpriteRenderer>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        healthTxt = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        invincible = false;
        SetHealth(initialHealth);
        transform.position = new Vector3(x, y, 0);
        movingVector = speed;
    }
    public void InitializeInvincibleBlock(float x, float y, float width, float height, Vector2 speed)
    {
        invincible = true;

        transform.SetParent(transform);
        transform.position = new Vector3(x, y, 0);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(width, height, 1));
        movingVector = speed;
    }

    private void Update()
    {
        if (alreadyDestroyed || movingVector == Vector2.zero) return;

        transform.Translate(movingVector * Time.deltaTime);
    }

    private void SetHealth(int value)
    {
        value = Mathf.Clamp(value, 0, maxHealth);
        health = value;

        if (health > 0)
        {
            image.color = healthColor[health - 1];
            healthTxt.text = health.ToString();
        }
        else
        {
            if (alreadyDestroyed == false)
            {
                alreadyDestroyed = true;

                canvas.gameObject.SetActive(false);
                if (TryGetComponent<PolygonCollider2D>(out PolygonCollider2D boxCollider)) boxCollider.enabled = false;
                if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider)) circleCollider.enabled = false;
                GetComponent<Animator>().SetTrigger("blockBreak");
                OnBreak?.Invoke(transform.position.x, transform.position.y);
                Invoke("DestroyObject", 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsLayerMatched(blockGrinderLayer, collision.gameObject.layer) == false) return;

        alreadyDestroyed = true;

        DestroyObject();
    }
    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void GetDamage(int damage = 1)
    {
        if (invincible || alreadyDestroyed) return;

        SetHealth(health - damage);
        // TODO : 피격시 효과
    }
}




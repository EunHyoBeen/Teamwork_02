using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
    private static readonly int maxHealth = 10;
    private static readonly Color[] healthColor = new Color[] 
        { new Color(0.8f, 0.8f, 0.8f), new Color(0.6f, 0.6f, 0.9f), new Color(0.0f, 1.0f, 0.5f), new Color(0.1f, 0.7f, 0.1f), new Color(0.7f, 0.7f, 0.0f),
          new Color(1.0f, 0.5f, 0.0f), new Color(1.0f, 0.0f, 0.0f), new Color(0.5f, 0.0f, 0.5f), new Color(0.2f, 0.2f, 0.8f), new Color(0.2f, 0.2f, 0.2f) };

    private SpriteRenderer image;

    public event Action<float, float> OnBreak;


    private int health;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>(); health = 9;
    }
    
    public void InitializeBlock(float x, float y, int initialHealth)
    {
        if (initialHealth <= 0) return;

        SetHealth(initialHealth);
        transform.position = new Vector3(x, y, 0);
    }

    private void SetHealth(int value)
    {
        value = Mathf.Clamp(value, 0, maxHealth);
        health = value;

        if (health > 0)
        {
            image.color = healthColor[health - 1];
        }
        else
        {
            Destroy(gameObject);
            OnBreak?.Invoke(transform.position.x, transform.position.y);
        }
    }
    

    public void GetDamage(int damage = 1)
    {
        SetHealth(health - damage);
        // TODO : 피격시 효과
    }
}




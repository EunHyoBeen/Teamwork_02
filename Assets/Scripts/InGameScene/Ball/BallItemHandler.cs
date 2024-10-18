using System;
using UnityEngine;

public class BallItemHandler : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BallController ballController;
    public event Action<int> OnPowerChange;
    public event Action<float> OnSpeedChange;
    [SerializeField] private Sprite powerSprite;
    [SerializeField] private Sprite normalSprite;
    //private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ballController = GetComponent<BallController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //trailRenderer = GetComponent<TrailRenderer>();
    }
    private void Start()
    {
        OnPowerChange += PowerChange;
        OnPowerChange += ChangeSpriteWhenPowerUp;
        OnSpeedChange += SpeedChange;
        //OnSpeedChange += ChangeRendererWhenSpeedUp;
    }


    public void PowerUpItem(int power)
    {
        OnPowerChange?.Invoke(power);
    }
    private void PowerChange(int changePower)
    {
        ballController.PowerChange(changePower);
    }
    private void ChangeSpriteWhenPowerUp(int power)
    {
        if (power == 1) spriteRenderer.sprite = normalSprite;
        else spriteRenderer.sprite = powerSprite;
    }

    public void SpeedUpItem(float speed)
    {
        OnSpeedChange?.Invoke(speed);
    }
    private void SpeedChange(float speed)
    {
        ballController.SpeedChange(speed);
    }


    //private void ChangeRendererWhenSpeedUp(float speed)
    //{
    //    if (speed < 0) trailRenderer.enabled = false;
    //    else trailRenderer.enabled = true;
    //}
}
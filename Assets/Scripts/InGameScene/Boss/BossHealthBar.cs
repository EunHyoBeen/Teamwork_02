using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Image healthBar;
    private TextMeshProUGUI healthTxt;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
        healthTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetHealthBar(int maxHP, int currentHP)
    {
        healthBar.fillAmount = (float)currentHP / maxHP;

        healthTxt.text = $"({currentHP} / {maxHP})";
    }
}

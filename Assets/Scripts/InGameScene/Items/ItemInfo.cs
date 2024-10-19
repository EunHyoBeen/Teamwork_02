using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInfo : MonoBehaviour
{
    public GameObject panel;
    private Dictionary<Item.Type, String> typeInfo;
    [SerializeField] private ItemContainer itemContainer;
    private void Awake() 
    {
        typeInfo = new Dictionary<Item.Type, String>();

        // typeInfo.Add(Item.Type._NONE,              "생성 안함"); 
        typeInfo.Add(Item.Type.BonusLife,          "목숨 추가"); 
        typeInfo.Add(Item.Type.PaddleSizeUp,       "패들 길이 증가"); 
        typeInfo.Add(Item.Type.PaddleSizeDown,     "패들 길이 감소");
        typeInfo.Add(Item.Type.PaddleSpeedUp,      "패들 이속 증가"); 
        typeInfo.Add(Item.Type.PaddleSpeedDown,    "패들 이속 감소"); 
        typeInfo.Add(Item.Type.BallPowerUp,        "볼 공격력 증가"); 
        typeInfo.Add(Item.Type.BallSpeedUp,        "볼 속도 증가"); 
        typeInfo.Add(Item.Type.BallTriple,         "볼 3개로 분할"); 
        typeInfo.Add(Item.Type.PaddleStopDebuff,   "패들 멈춤"); 
    }
    // Start is called before the first frame update
    public void InActivePalnel()
    {
        panel.SetActive(false);
    }

    public void ActivePalnel()
    {
        panel.SetActive(true);
    }
}

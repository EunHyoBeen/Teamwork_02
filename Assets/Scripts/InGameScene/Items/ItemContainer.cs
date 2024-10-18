using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    public Dictionary<Item.Type, float> typeProbabilityWeight { get; private set; }
    private float typeTotalWeight;

    //아이템 생성 가중치
    public void ResetItemTypeWeight()
    {
        typeProbabilityWeight = new Dictionary<Item.Type, float>();

        typeProbabilityWeight.Add(Item.Type._NONE,             15.0f); // 생성 안함
        typeProbabilityWeight.Add(Item.Type.BonusLife,          0.1f); // 목숨 추가
        typeProbabilityWeight.Add(Item.Type.PaddleSizeUp,       0.5f); // 패들 길이 증가
        typeProbabilityWeight.Add(Item.Type.PaddleSizeDown,     0.5f); // 패들 길이 감소
        typeProbabilityWeight.Add(Item.Type.PaddleSpeedUp,      0.5f); // 패들 이속 증가
        typeProbabilityWeight.Add(Item.Type.PaddleSpeedDown,    0.5f); // 패들 이속 감소
        typeProbabilityWeight.Add(Item.Type.BallPowerUp,        0.5f); // 볼 공격력 증가(기간)
        typeProbabilityWeight.Add(Item.Type.BallSpeedUp,        0.5f); // 볼 속도 증가(기간)
        typeProbabilityWeight.Add(Item.Type.BallTriple,         1.0f); // 볼 3개로 분할
        typeProbabilityWeight.Add(Item.Type.PaddleStopDebuff,   1.0f); // 패들 멈춤(기간)

        typeTotalWeight = 0;
        foreach (KeyValuePair<Item.Type, float> typeWeight in typeProbabilityWeight) typeTotalWeight += typeWeight.Value;
    }
    public void SetItemTypeWeight(Item.Type type, float value) // 특정 아이템의 출현 가중치 설정
    {
        typeProbabilityWeight[type] = value;

        typeTotalWeight = 0;
        foreach (KeyValuePair<Item.Type, float> typeWeight in typeProbabilityWeight) typeTotalWeight += typeWeight.Value;
    }

    public void RandomItemCreation(float x, float y)
    {
        float randomValue = UnityEngine.Random.Range(0, typeTotalWeight);
        float cumulativeWeight = 0;
        Item.Type selectedItemType = Item.Type._NONE;
        bool isSelected = false;
        for (int i = 0; i < (int)Item.Type._MAX; i++)
        {
            cumulativeWeight += typeProbabilityWeight[(Item.Type)i];
            if (randomValue < cumulativeWeight)
            {
                selectedItemType = (Item.Type)i;
                isSelected = true;
                break;
            }
        }
        
        // 아이템 생성 안함
        if (isSelected == false) return; 

        // 아이템 생성
        GameObject itemInstance = Instantiate(itemPrefab);
        itemInstance.transform.SetParent(transform);
        Item item = itemInstance.GetComponent<Item>();
        item.InitializeItem(x, y, selectedItemType, Vector2.zero);
    }

    public void ItemCreation(Item.Type type, float x, float y, Vector2 initialSpeed)
    {
        if (type < 0 || type >= Item.Type._MAX) return;

        GameObject itemInstance = Instantiate(itemPrefab);
        itemInstance.transform.SetParent(transform);
        Item item = itemInstance.GetComponent<Item>();
        item.InitializeItem(x, y, type, initialSpeed);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

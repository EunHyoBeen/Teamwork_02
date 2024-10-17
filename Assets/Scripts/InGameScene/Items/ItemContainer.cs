using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    private Dictionary<Item.Type, float> typeProbabilityWeight;
    private float typeTotalWeight;

    private void Awake()
    {
        typeProbabilityWeight = new Dictionary<Item.Type, float>();

        //������ ���� ����ġ
        typeProbabilityWeight.Add(Item.Type._NONE,             15.0f); // ���� ����
        typeProbabilityWeight.Add(Item.Type.BonusLife,          0.1f); // ��� �߰�
        typeProbabilityWeight.Add(Item.Type.PaddleSizeUp,       0.5f); // �е� ���� ����
        typeProbabilityWeight.Add(Item.Type.PaddleSizeDown,     0.5f); // �е� ���� ����
        typeProbabilityWeight.Add(Item.Type.PaddleSpeedUp,      0.5f); // �е� �̼� ����
        typeProbabilityWeight.Add(Item.Type.PaddleSpeedDown,    0.5f); // �е� �̼� ����
        typeProbabilityWeight.Add(Item.Type.BallPowerUp,        0.5f); // �� ���ݷ� ����(�Ⱓ)
        typeProbabilityWeight.Add(Item.Type.BallSpeedUp,        0.5f); // �� �ӵ� ����(�Ⱓ)
        typeProbabilityWeight.Add(Item.Type.BallTriple,         1.0f); // �� 3���� ����
        typeProbabilityWeight.Add(Item.Type.PaddleStopDebuff,   1.0f); // �е� ����(�Ⱓ)

        typeTotalWeight = 0;
        foreach (KeyValuePair<Item.Type, float> typeWeight in typeProbabilityWeight) typeTotalWeight += typeWeight.Value;
    }

    public void RandomItemCreation(float x, float y)
    {
        float randomValue = Random.Range(0, typeTotalWeight);
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
        
        // ������ ���� ����
        if (isSelected == false) return; 

        // ������ ����
        GameObject itemInstance = Instantiate(itemPrefab);
        itemInstance.transform.SetParent(transform);
        Item item = itemInstance.GetComponent<Item>();
        item.InitializeItem(x, y, selectedItemType);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

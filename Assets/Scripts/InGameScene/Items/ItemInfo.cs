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

        // typeInfo.Add(Item.Type._NONE,              "���� ����"); 
        typeInfo.Add(Item.Type.BonusLife,          "��� �߰�"); 
        typeInfo.Add(Item.Type.PaddleSizeUp,       "�е� ���� ����"); 
        typeInfo.Add(Item.Type.PaddleSizeDown,     "�е� ���� ����");
        typeInfo.Add(Item.Type.PaddleSpeedUp,      "�е� �̼� ����"); 
        typeInfo.Add(Item.Type.PaddleSpeedDown,    "�е� �̼� ����"); 
        typeInfo.Add(Item.Type.BallPowerUp,        "�� ���ݷ� ����"); 
        typeInfo.Add(Item.Type.BallSpeedUp,        "�� �ӵ� ����"); 
        typeInfo.Add(Item.Type.BallTriple,         "�� 3���� ����"); 
        typeInfo.Add(Item.Type.PaddleStopDebuff,   "�е� ����"); 
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

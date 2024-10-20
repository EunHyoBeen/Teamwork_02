using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItem : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemImages itemImages; 

    // Start is called before the first frame update
    void Start()
    {
        Item.Type[] items = {Item.Type.BonusLife, Item.Type.BallPowerUp};
        ShowBoostItem(items);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBoostItem(Item.Type[] items)
    {
        float itemX = -2.5f;
        float itemY = 4.2f;

        if (items.Length != 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                GameObject boostItem = Instantiate(itemPrefab, this.transform);
                boostItem.layer = 5;

                // 부모의 중심에 오브젝트 위치 설정
                RectTransform rtf = boostItem.gameObject.GetComponent<RectTransform>();

                if (rtf != null)
                {
                    rtf.anchoredPosition = new Vector2(0, 200);
                }
                // rtf.localScale = Vector3.one; 
                
                Item item = boostItem.GetComponent<Item>();

                Rigidbody2D rb = boostItem.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0f;

                itemX += 0.5f;
                if (item != null)
                {
                    item.InitializeItem(itemX, itemY, items[i], Vector2.zero);
                }

                SpriteRenderer spriteRenderer = boostItem.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && (int)items[i] < itemImages.List.Count)
                {
                    spriteRenderer.sprite = itemImages.List[(int)items[i]];
                    spriteRenderer.sortingOrder = 2;
                }
            }
        }
        
    }
}
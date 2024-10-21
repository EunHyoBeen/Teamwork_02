using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class BoostItem : MonoBehaviour
{
    // [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemImages itemImages; 
    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;
    [SerializeField] private GameObject boostItem;
    
    public void ShowBoostItem(Item.Type[] items)
    {
        if (items == null)
        {
            items = new Item.Type[0];
        }
        
        Image img1 = item1.GetComponent<Image>();
        Image img2 = item2.GetComponent<Image>();

        switch(items.Length)
        {
            case 0:
                boostItem.SetActive(false);
                break;
            case 1:
                item1.SetActive(true);
                img1.sprite = itemImages.List[(int)items[0]];
                break;

            case 2:
                item1.SetActive(true);
                item2.SetActive(true);
                img1.sprite = itemImages.List[(int)items[0]];
                img2.sprite = itemImages.List[(int)items[1]];
                break;
        }
    }
}
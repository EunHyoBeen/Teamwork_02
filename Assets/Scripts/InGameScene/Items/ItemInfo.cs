using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInfo : MonoBehaviour
{
    public GameObject panel;

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

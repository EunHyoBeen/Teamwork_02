using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private float time;
    // [SerializeField] private float duration = 2f;

    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0.5f)
        {
            textMeshProUGUI.color = new Color(1f, 88 / 255f, 88 / 255f, 1f - time * 2);
        }
        else
        {
            textMeshProUGUI.color = new Color(1f, 88 / 255f, 88 / 255f, (time - 0.5f) * 2);
            if (time > 1f)
            {
                time = 0;
            }
        }

        time += Time.deltaTime;
    }
}

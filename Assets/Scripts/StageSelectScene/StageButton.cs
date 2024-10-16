using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public GameObject stageBtnPrefab;
    public RectTransform buttonParent;
    private int stageCount = 10;
    private int stageNum = 0;
    
    void Start()
    {
        for (int i = 0; i < stageCount; i++)
        {
            GameObject stageBtn = Instantiate(stageBtnPrefab, buttonParent);

            RectTransform rtf = stageBtn.GetComponent<RectTransform>();
            if (rtf != null)
            {
                rtf.anchoredPosition = new Vector2(0, 550 - (200 * i));
                rtf.sizeDelta = new Vector2(700, 150);
            }

            Button button = stageBtn.GetComponent<Button>();
            int stageIdx = i + 1;
            button.onClick.AddListener(() => SelectStageButton(stageIdx));

            stageBtn.name = "StageBtn" + (i + 1);

            TMPro.TextMeshProUGUI buttonText = stageBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = "Stage " + (i + 1);
        }
    }

    public void SelectStageButton(int stageNum)
    {
        Debug.Log(stageNum);
        GameManager.Instance.stageParameter.stageIndex = stageNum;
        SceneManager.LoadScene("InGameScene");
    }
}

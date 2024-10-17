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

        for (int i = 1; i <= stageCount; i++)
        {
            
            GameObject stageBtn = Instantiate(stageBtnPrefab, buttonParent);

            RectTransform rtf = stageBtn.GetComponent<RectTransform>();
            if (rtf != null)
            {
                rtf.anchoredPosition = new Vector2(0, 550 - (200 * i));
                rtf.sizeDelta = new Vector2(700, 150);
            }

            Button button = stageBtn.GetComponent<Button>();

            button.onClick.AddListener(() => SelectStageButton(i));

            if (GameManager.Instance.gameData.StageUnlock[i] != true)
            {   
                InActiveButton(button, stageBtn);
            }

            stageBtn.name = "StageBtn" + i;

            TMPro.TextMeshProUGUI buttonText = stageBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = "Stage " + i;

            // if (GameManager.stageResult.isClear == true)
            // {
            //     ActiveButton(button);
            // }
            
        }
    }

    public void SelectStageButton(int stageNum)
    {
        Debug.Log(stageNum);
        GameManager.Instance.stageParameter.stageIndex = stageNum;
        SceneManager.LoadScene("InGameScene");
    }

    private void ActiveButton(Button button)
    {
        button.interactable = true;
    }

    private void InActiveButton(Button button, GameObject stageBtn)
    {
        button.interactable = false;
        CanvasGroup canvasGroup = stageBtn.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
    }

    
}

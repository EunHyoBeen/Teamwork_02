using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton 
{
    public int idx;
    public GameObject gameObject;
    public Button button;
}
public class StageSelect : MonoBehaviour
{
    public StageButton stageButton;
    public GameObject stageBtnPrefab;
    public RectTransform buttonParent;
    private int stageCount = 10;
    
    void Start()
    {
        // isClear�� ���� stageUnlock ���� ����
        SetStageUnlock();

        // ��ư ���������� ����
        for (int i = 1; i <= stageCount; i++)
        {
            stageButton = new StageButton();
            stageButton.idx = i;
            stageButton.gameObject = Instantiate(stageBtnPrefab, buttonParent);

            RectTransform rtf = stageButton.gameObject.GetComponent<RectTransform>();
            if (rtf != null)
            {
                rtf.anchoredPosition = new Vector2(0, 550 - (200 * i));
                rtf.sizeDelta = new Vector2(700, 150);
            }

            stageButton.button = stageButton.gameObject.GetComponent<Button>();

            stageButton.gameObject.name = "StageBtn" + i;

            TMPro.TextMeshProUGUI buttonText = stageButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = "Stage " + i;

            // Debug.Log(i);
            
            int idx = i;
            stageButton.button.onClick.AddListener(() => SelectStageButton(idx));

            // ���� ó������ ��ư 1�� Ȱ��ȭ�ǰ�, �������� ��� ��Ȱ��ȭ
            if (!GameManager.Instance.gameData.StageUnlock[i])
            {   
                InActiveButton(stageButton);
            }
        }
    }

    public void SelectStageButton(int idx)
    {
        GameManager.Instance.stageParameter.stageIndex = idx;
        SceneManager.LoadScene("InGameScene");
    }

    private void InActiveButton(StageButton stageButton)
    {
        stageButton.button.interactable = false;
        CanvasGroup canvasGroup = stageButton.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
    }

    public void SetStageUnlock()
    {
        InGameManager.StageResult stageResult = GameManager.Instance.stageResult;
        DataManager.GameData gameData = GameManager.Instance.gameData;
//        stageResult.isClear = true;
        if (stageResult.isClear)
        {
            gameData.StageUnlock[stageResult.stageIndex + 1] = true;
//            gameData.StageUnlock[2] = true;
        }
        else 
        {
            gameData.StageUnlock[stageResult.stageIndex + 1] = false;
        }   
    }
}

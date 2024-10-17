using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DataManager.GameData gameData;

    public InGameManager.StageParameter stageParameter; // �������� �ҷ����� �Ű�����
    public InGameManager.StageResult stageResult; // �������� ��� �Ű�����

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ���� ������ �ҷ�����
            if (DataManager.LoadDatafile() == false)
            {
                gameData = new DataManager.GameData();
            }
            
            stageParameter = new InGameManager.StageParameter();
            stageResult = new InGameManager.StageResult();
        }
    }


}

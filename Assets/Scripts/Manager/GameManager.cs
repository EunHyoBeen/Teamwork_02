using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DataManager.GameData gameData;

    public InGameManager.StageParameter stageParameter; // 스테이지 불러오기 매개변수
    public InGameManager.StageResult stageResult; // 스테이지 결과 매개변수

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 게임 데이터 불러오기
            if (DataManager.LoadDatafile() == false)
            {
                gameData = new DataManager.GameData();
            }
            
            stageParameter = new InGameManager.StageParameter();
            stageResult = new InGameManager.StageResult();
        }
    }


}

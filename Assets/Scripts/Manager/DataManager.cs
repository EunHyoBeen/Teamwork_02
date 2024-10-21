using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManager : MonoBehaviour
{
    //저장파일 경로는 실행파일과 같음
    //저장파일 이름은 SaveData1, SaveData2, SaveData3 세개
    public static bool SaveDatafile()
    {
        string filePath = $"SaveData";

        SaveData saveData = new SaveData();
        GameData gameData = GameManager.Instance.gameData;

        Array.Copy(gameData.StageUnlock, saveData.StageUnlock, gameData.StageUnlock.Length);

        try
        {
            // 객체를 JSON으로 직렬화
            string jsonData = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    //저장파일 경로는 실행파일과 같음
    //저장파일 이름은 SaveData1, SaveData2, SaveData3 세개
    public static bool LoadDatafile()
    {
        string filePath = $"SaveData";
        try
        {
            if (File.Exists(filePath))
            {
                // JSON 파일을 읽어서 객체로 역직렬화
                string jsonData = File.ReadAllText(filePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);
                GameData gameData = new GameData();

                if (saveData.StageUnlock != null)
                {
                    Array.Copy(saveData.StageUnlock, gameData.StageUnlock, saveData.StageUnlock.Length);
                }

                GameManager.Instance.gameData = gameData;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    // 인게임 데이터
    public class GameData
    {
        public bool[] StageUnlock;

        public InGameManager.GameMode GameMode;

        public GameData()
        {
            StageUnlock = new bool[100];
            StageUnlock[1] = true;

            GameMode = InGameManager.GameMode.Alone;

            // 일단 디버깅 해야하니까 언락
            for (int i = 2; i < 50; i++)
            {
                StageUnlock[i] = true;
            }
        }
    }
    // 인게임 데이터를 json 형식으로 저장할 수 있도록 변환한 클래스
    [System.Serializable]
    private class SaveData
    {
        public bool[] StageUnlock { get; set; }

        public SaveData()
        {
            StageUnlock = new bool[100];
        }
    }
}

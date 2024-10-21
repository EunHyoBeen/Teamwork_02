using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManager : MonoBehaviour
{
    //�������� ��δ� �������ϰ� ����
    //�������� �̸��� SaveData1, SaveData2, SaveData3 ����
    public static bool SaveDatafile()
    {
        string filePath = $"SaveData";

        SaveData saveData = new SaveData();
        GameData gameData = GameManager.Instance.gameData;

        Array.Copy(gameData.StageUnlock, saveData.StageUnlock, gameData.StageUnlock.Length);

        try
        {
            // ��ü�� JSON���� ����ȭ
            string jsonData = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    //�������� ��δ� �������ϰ� ����
    //�������� �̸��� SaveData1, SaveData2, SaveData3 ����
    public static bool LoadDatafile()
    {
        string filePath = $"SaveData";
        try
        {
            if (File.Exists(filePath))
            {
                // JSON ������ �о ��ü�� ������ȭ
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

    // �ΰ��� ������
    public class GameData
    {
        public bool[] StageUnlock;

        public InGameManager.GameMode GameMode;

        public GameData()
        {
            StageUnlock = new bool[100];
            StageUnlock[1] = true;

            GameMode = InGameManager.GameMode.Alone;

            // �ϴ� ����� �ؾ��ϴϱ� ���
            for (int i = 2; i < 50; i++)
            {
                StageUnlock[i] = true;
            }
        }
    }
    // �ΰ��� �����͸� json �������� ������ �� �ֵ��� ��ȯ�� Ŭ����
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

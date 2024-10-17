using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private BlockContainer blockContainer;
    [SerializeField] private PlayerManager player1;
    [SerializeField] private PlayerManager player2;

    public GameMode gameMode { get; private set; }//���� ��� ����

    public int stageIndex { get; private set; }

    public bool player1Alive, player2Alive;


    private void Start()
    {
        StageParameter stageParameter = GameManager.Instance.stageParameter;
        ExecuteStage(stageParameter.stageIndex);
    }

    private void ExecuteStage(int _stageIndex)
    {
        stageIndex = _stageIndex;

        player1Alive = true;
        player2Alive = true;
        player1.OnDeathEvent += PlayerOnDeath;
        player1.OnDeathEvent += PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed += GameClear;
        blockContainer.DeployBlock(stageIndex);

        Time.timeScale = 1;
    }

    private void GameClear()
    {
        Time.timeScale = 0;
        // TODO : Ŭ���� �޴� ����


        StageResult stageResult = GameManager.Instance.stageResult;
        stageResult.stageIndex = stageIndex;
        stageResult.isClear = true;

        // �̺�Ʈ ����
        player1.OnDeathEvent -= PlayerOnDeath;
        player1.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;

        SceneManager.LoadScene("StageSelectScene");
    }

    private void PlayerOnDeath(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1Alive = false;
        }
        else
        {
            player2Alive = false;
        }
        
        if (player1Alive == false && player2Alive == false)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        Time.timeScale = 1;
        // TODO : ���� �޴� ����


        StageResult stageResult = GameManager.Instance.stageResult;
        stageResult.stageIndex = stageIndex;
        stageResult.isClear = false;

        // �̺�Ʈ ����
        player1.OnDeathEvent -= PlayerOnDeath;
        player1.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;

        SceneManager.LoadScene("StageSelectSample");
    }


    public enum GameMode
    {
        Default_Alone, Default_Duo
    }

    public class StageParameter
    {
        public int stageIndex;
    }

    public class StageResult
    {
        public int stageIndex;
        public bool isClear;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        stageIndex = stageParameter.stageIndex;
        ExecuteStage();
    }

    private void Update()
    {
        if (Input.GetKeyDown("Escape"))
        {
            PauseGame();
        }
    }

    private void ExecuteStage()
    {
        player1Alive = true;
        player2Alive = true;
        player1.OnDeathEvent += PlayerOnDeath;
        player1.OnDeathEvent += PlayerOnDeath;

        blockContainer.OnAllBlockDestroyed += GameClear;
        blockContainer.DeployBlock(stageIndex);

        Time.timeScale = 1;
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

    private void GameClear()
    {
        //// �̺�Ʈ ����
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player1.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear;

        Invoke("GameClearSequence", 1);
    }
    private void GameClearSequence()
    {
        Time.timeScale = 0;

        StageResult stageResult = GameManager.Instance.stageResult;
        stageResult.stageIndex = stageIndex;
        stageResult.isClear = true;

        // TODO : Ŭ���� �޴� ����


    }

    private void GameOver()
    {
        //// �̺�Ʈ ����
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player1.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear;

        Invoke("GameOverSequence", 1);
    }
    private void GameOverSequence()
    {
        Time.timeScale = 0;

        StageResult stageResult = GameManager.Instance.stageResult;
        stageResult.stageIndex = stageIndex;
        stageResult.isClear = false;

        // TODO : ���ӿ��� �޴� ����


    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        // TODO : ���� �޴� ����

    }
    public void ResumeGame()
    {
        // TODO : ���� �޴� �ݱ�

        Time.timeScale = 1;
    }
    public void RestartStage()
    {
        // TODO : ���� �޴� �ݱ�

        blockContainer.Clear();
        player1.OnDeathEvent -= PlayerOnDeath;
        player1.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;
        ExecuteStage();
    }
    public void ExitStage()
    {
        blockContainer.Clear();
        player1.OnDeathEvent -= PlayerOnDeath;
        player1.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;

        SceneManager.LoadScene("StageSelectScene");
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

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

    public GameMode gameMode { get; private set; }//현재 사용 안함

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
        //// 이벤트 해제
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

        // TODO : 클리어 메뉴 띄우기


    }

    private void GameOver()
    {
        //// 이벤트 해제
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

        // TODO : 게임오버 메뉴 띄우기


    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        // TODO : 게임 메뉴 띄우기

    }
    public void ResumeGame()
    {
        // TODO : 게임 메뉴 닫기

        Time.timeScale = 1;
    }
    public void RestartStage()
    {
        // TODO : 게임 메뉴 닫기

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

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

    [SerializeField] private RectTransform inGameMenu;
    [SerializeField] private RectTransform clearMenu;
    [SerializeField] private RectTransform gameOverMenu;

    public GameMode gameMode { get; private set; }//현재 사용 안함

    public int stageIndex { get; private set; }

    public bool player1Alive, player2Alive;

    private bool isPlaying;

    private void Start()
    {
        StageParameter stageParameter = GameManager.Instance.stageParameter;
        Debug.Log(stageParameter.stageIndex);
        stageIndex = stageParameter.stageIndex;
        ExecuteStage();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying == true)
        {
            if (inGameMenu.gameObject.activeSelf == false) PauseGame();
            else ResumeGame();
        }
    }

    private void ExecuteStage()
    {
        isPlaying = true;

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
        isPlaying = false;
        // 이벤트 해제
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

        clearMenu.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        isPlaying = false;
        // 이벤트 해제
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

        
        gameOverMenu.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        inGameMenu.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        inGameMenu.gameObject.SetActive(false);

        Time.timeScale = 1;
    }
    public void RestartStage()
    {
        inGameMenu.gameObject.SetActive(false);
        clearMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);

        // 이벤트 및 오브젝트 해제
        //blockContainer.Clear();
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player1.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear;

        ExecuteStage();
    }
    public void ExitStage()
    {
        // 이벤트 및 오브젝트 해제
        //blockContainer.Clear();
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player1.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear;

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

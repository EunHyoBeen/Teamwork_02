using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private BlockContainer blockContainer;
    [SerializeField] protected PlayerManager player1;
    [SerializeField] protected PlayerManager player2;

    [SerializeField] protected RectTransform inGameMenu;
    [SerializeField] protected RectTransform clearMenu;
    [SerializeField] protected RectTransform gameOverMenu;

    protected GameMode gameMode; //현재 사용 안함

    public int stageIndex;

    protected int player1Life, player2Life;

    protected bool isPlaying;
    protected bool isRally; // 플레이어가 첫 공을 발사할 때 true로 전환됨

    protected virtual void Start()
    {
        StageParameter stageParameter = GameManager.Instance.stageParameter;
        stageIndex = stageParameter.stageIndex;
        ExecuteStage();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying == true)
        {
            if (inGameMenu.gameObject.activeSelf == false) PauseGame();
            else ResumeGame();
        }
    }

    protected virtual void ExecuteStage()
    {
        isPlaying = true;
        isRally = false;

        player1Life = 3;
        player2Life = 3;
        // TODO : 플레이어 런치 이벤트
        //player1.OnLaunch += PlayerOnLaunch;
        //player2.OnLaunch += PlayerOnLaunch;
        player1.OnDeathEvent += PlayerOnDeath;
        player2.OnDeathEvent += PlayerOnDeath;

        blockContainer.OnAllBlockDestroyed += GameClear;
        blockContainer.SetStage(stageIndex);

        Time.timeScale = 1;

        player1.InitializePlayer();
    }

    protected virtual void PlayerOnLaunch()
    {
        if (isRally == false)
        {
            isRally= true;

            // TODO : 첫 공 발사 후 있어야 할 일들. 타이머를 작동시킨다던지 등
        }
    }

    protected void PlayerOnDeath(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1Life--;
        }
        else
        {
            player2Life--;
        }

        if (player1Life <= 0 && player2Life <= 0 && isPlaying == true)
        {
            GameOver();
        }
    }

    protected void GameClear()
    {
        isPlaying = false;
        // 이벤트 해제
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player2.OnDeathEvent -= PlayerOnDeath;
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
        //player2.OnDeathEvent -= PlayerOnDeath;
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
    public virtual void RestartStage()
    {
        inGameMenu.gameObject.SetActive(false);
        clearMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);

        // 이벤트 및 오브젝트 해제
        blockContainer.Clear();
        //player1.OnLaunch -= PlayerOnLaunch;
        //player2.OnLaunch -= PlayerOnLaunch;
        player1.OnDeathEvent -= PlayerOnDeath;
        player2.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;

        ExecuteStage();
    }
    public void ExitStage()
    {
        // 이벤트 및 오브젝트 해제
        //blockContainer.Clear();
        //player1.OnDeathEvent -= PlayerOnDeath;
        //player2.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear;

        SceneManager.LoadScene("StageSelectScene");
    }



    public enum GameMode
    {
        Alone,
        Duo_community, Duo_individual
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

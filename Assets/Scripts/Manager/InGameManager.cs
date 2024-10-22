using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static InGameManager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private BlockContainer blockContainer;
    [SerializeField] protected PlayerManager player1;
    [SerializeField] protected PlayerManager player2;
    [SerializeField] protected PlayerHealth playerHealth;

    [SerializeField] protected RectTransform inGameMenu;
    [SerializeField] protected RectTransform clearMenu;
    [SerializeField] protected RectTransform gameOverMenu;

    protected GameMode gameMode;

    public int stageIndex;

    protected int player1Life, player2Life;
    protected bool player1HasBall, player2HasBall;

    protected bool isPlaying;
    protected bool isRally; // 플레이어가 첫 공을 발사할 때 true로 전환됨

    protected virtual void Start()
    {
        StageParameter stageParameter = GameManager.Instance.stageParameter;
        gameMode = stageParameter.gameMode;
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

        player1.PlayerID = 1;
        player2.PlayerID = 2;
        player1.OnLaunchEvent += PlayerOnLaunch;
        player2.OnLaunchEvent += PlayerOnLaunch;
        player1.OnLifeUpEvent += PlayerOnLifeUp;
        player2.OnLifeUpEvent += PlayerOnLifeUp;
        player1.OnDeathEvent += PlayerOnDeath;
        player2.OnDeathEvent += PlayerOnDeath;

        blockContainer.OnAllBlockDestroyed += GameClear;
        blockContainer.SetStage(stageIndex);

        if (gameMode == GameMode.Alone)
        {
            player1Life = 3;
            player2Life = 0;
            player1HasBall = true;
            player1.InitializePlayer(0, -4);
        }
        else
        {
            player1Life = 3;
            player2Life = 0;
            player1HasBall = true;
            player2HasBall = true;
            player1.InitializePlayer(-1.5f, -4);
            player2.InitializePlayer(1.5f, -4);
        }
        playerHealth.DisplayHealth(player1Life, player2Life);

        Time.timeScale = 1;
    }

    protected virtual void PlayerOnLaunch()
    {
        if (isRally == false)
        {
            isRally= true;

            // TODO : 첫 공 발사 후 있어야 할 일들. 타이머를 작동시킨다던지 등
        }
    }

    protected virtual void PlayerOnLifeUp(int playerIndex)
    {
        player1Life = Mathf.Clamp(player1Life + 1, 0, 3);

        playerHealth.DisplayHealth(player1Life, player2Life);
    }

    protected void PlayerOnDeath(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1HasBall = false;
        }
        else
        {
            player2HasBall = false;
        }

        if (gameMode == GameMode.Alone)
        {
            player1Life--;
            playerHealth.DisplayHealth(player1Life, 0);

            if (player1Life <= 0)
            {
                GameOver();
            }
            else
            {
                player1.InitializePlayer(0, -4);
            }
        }
        else if (gameMode == GameMode.Duo_community)
        {
            if (player1HasBall == false && player2HasBall == false)
            {
                player1Life--;
                playerHealth.DisplayHealth(player1Life, 0);

                if (player1Life <= 0)
                {
                    GameOver();
                }
                else
                {
                    player1.InitializePlayer(-1.5f, -4);
                    player2.InitializePlayer(1.5f, -4);
                }
            }
        }
    }

    protected void GameClear()
    {
        isPlaying = false;

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
        player1.OnLaunchEvent -= PlayerOnLaunch;
        player2.OnLaunchEvent -= PlayerOnLaunch;
        player1.OnLifeUpEvent -= PlayerOnLifeUp;
        player2.OnLifeUpEvent -= PlayerOnLifeUp;
        player1.OnDeathEvent -= PlayerOnDeath;
        player2.OnDeathEvent -= PlayerOnDeath;
        blockContainer.OnAllBlockDestroyed -= GameClear;

        ExecuteStage();
    }
    public void ExitStage()
    {
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
        public InGameManager.GameMode gameMode;
    }

    public class StageResult
    {
        public int stageIndex;
        public bool isClear;
    }
}

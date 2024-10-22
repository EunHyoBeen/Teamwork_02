using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSceneManager : InGameManager
{
    [SerializeField] private BlockContainerForBoss blockContainerForBoss;
    [SerializeField] private BossController boss;

    protected override void Start()
    {
        InGameManager.StageParameter stageParameter = GameManager.Instance.stageParameter;
        gameMode = stageParameter.gameMode;
        stageIndex = stageParameter.stageIndex;
        // stage
        stageIndex = 10; //Debug
        ExecuteStage();
    }
    protected override void ExecuteStage()
    {
        isPlaying = true;
        isRally = false;

        player1.PlayerID = 1;
        player2.PlayerID = 2;
        // TODO : 플레이어 런치 이벤트
        player1.OnLaunchEvent += PlayerOnLaunch;
        player2.OnLaunchEvent += PlayerOnLaunch;
        player1.OnLifeUpEvent += PlayerOnLifeUp;
        player2.OnLifeUpEvent += PlayerOnLifeUp;
        player1.OnDeathEvent += PlayerOnDeath;
        player2.OnDeathEvent += PlayerOnDeath;

        if (gameMode == GameMode.Alone)
        {
            player1Life = 3;
            player1HasBall = true;
            player1.InitializePlayer(0, -4, true);
            playerHealth.DisplayHealth(3, 0);
        }
        else if (gameMode == GameMode.Duo_community)
        {
            player1Life = 3;
            player2Life = 0;
            player1HasBall = true;
            player1.InitializePlayer(-1.5f, -4, true);
            player2.InitializePlayer(1.5f, -4, false);
        }
        else
        {
            player1Life = 3;
            player2Life = 3;
            player1HasBall = true;
            player2HasBall = true;
            player1.InitializePlayer(-1.5f, -4, true);
            player2.InitializePlayer(1.5f, -4, true);
        }

        //blockContainer.OnAllBlockDestroyed += GameClear; // 보스 처리로 변경
        boss.OnBossBreak += GameClear;
        blockContainerForBoss.SetStage(stageIndex);
        switch (stageIndex)
        {
            case 10:
                boss.InitializeBoss(stageIndex);
                blockContainerForBoss.InitializedSpawn(stageIndex);
                break;
        }

        Time.timeScale = 1;
    }

    protected override void PlayerOnLaunch()
    {
        if (isRally == false)
        {
            isRally = true;

            boss.ActionStop(false);
            blockContainerForBoss.SpawnStop(false);
            // TODO : 첫 공 발사 후 있어야 할 일들. 타이머를 작동시킨다던지 등
        }
    }

    public override void RestartStage()
    {
        inGameMenu.gameObject.SetActive(false);
        clearMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);

        // 이벤트 및 오브젝트 해제
        blockContainerForBoss.Clear();
        player1.OnLaunchEvent -= PlayerOnLaunch;
        player2.OnLaunchEvent -= PlayerOnLaunch;
        player1.OnLifeUpEvent -= PlayerOnLifeUp;
        player2.OnLifeUpEvent -= PlayerOnLifeUp;
        player1.OnDeathEvent -= PlayerOnDeath;
        player2.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear; // 보스 처리로 변경
        boss.OnBossBreak -= GameClear;

        ExecuteStage();
        boss.InitializeBoss(stageIndex);
    }
}

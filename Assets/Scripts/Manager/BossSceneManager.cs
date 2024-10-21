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
        // TODO : �÷��̾� ��ġ �̺�Ʈ
        player1.OnLaunchEvent += PlayerOnLaunch;
        player2.OnLaunchEvent += PlayerOnLaunch;
        player1.OnDeathEvent += PlayerOnDeath;
        player2.OnDeathEvent += PlayerOnDeath;

        if (gameMode == GameMode.Alone)
        {
            player1Life = 3;
            player1HasBall = true;
            player1.InitializePlayer(0, -4);
            playerHealth.DisplayHealth(3, 0);
        }
        else
        {
            player1Life = 3;
            player1HasBall = true;
            player2HasBall = true;
            player1.InitializePlayer(-1.5f, -4);
            player2.InitializePlayer(1.5f, -4);
            playerHealth.DisplayHealth(3, 0);
        }

        //blockContainer.OnAllBlockDestroyed += GameClear; // ���� ó���� ����
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
            // TODO : ù �� �߻� �� �־�� �� �ϵ�. Ÿ�̸Ӹ� �۵���Ų�ٴ��� ��
        }
    }

    public override void RestartStage()
    {
        inGameMenu.gameObject.SetActive(false);
        clearMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);

        // �̺�Ʈ �� ������Ʈ ����
        blockContainerForBoss.Clear();
        player1.OnLaunchEvent -= PlayerOnLaunch;
        player2.OnLaunchEvent -= PlayerOnLaunch;
        player1.OnDeathEvent -= PlayerOnDeath;
        player2.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear; // ���� ó���� ����
        boss.OnBossBreak -= GameClear;

        ExecuteStage();
        boss.InitializeBoss(stageIndex);
    }
}

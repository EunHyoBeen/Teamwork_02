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
        stageIndex = stageParameter.stageIndex;
        // stage
        stageIndex = 10; //Debug
        ExecuteStage();
    }

    protected override void ExecuteStage()
    {
        isPlaying = true;

        player1Alive = true;
        player2Alive = true;
        // TODO : �÷��̾� ��ġ �̺�Ʈ
        //player1.OnLaunch += PlayerOnLaunch;
        //player2.OnLaunch += PlayerOnLaunch;
        player1.OnDeathEvent += PlayerOnDeath;
        player2.OnDeathEvent += PlayerOnDeath;

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
        PlayerOnLaunch();//Debug
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
        //player1.OnLaunch -= PlayerOnLaunch;
        //player2.OnLaunch -= PlayerOnLaunch;
        player1.OnDeathEvent -= PlayerOnDeath;
        player2.OnDeathEvent -= PlayerOnDeath;
        //blockContainer.OnAllBlockDestroyed -= GameClear; // ���� ó���� ����
        boss.OnBossBreak -= GameClear;

        ExecuteStage();
    }
}

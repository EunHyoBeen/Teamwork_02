using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockContainerForBoss : BlockContainer
{
    private List<Block> floatingBlocks = new List<Block>();

    private int stageIndex;
    private float spawnStartTime;
    private float spawnIntervalTime;
    private float currentSpawTime;
    private int spawnCycle;

    private bool spawnStopped;

    private Action spawnBlock;

    public void InitializedSpawn(int _stageIndex)
    {
        stageIndex = _stageIndex;

        spawnStopped = true; // 플레이어가 공을 발사하고 카운팅 시작

        switch (stageIndex)
        {
            case 10:
                spawnStartTime = 10f;
                spawnIntervalTime = 2f;
                spawnBlock = SpawnBlockStage10;
                break;
        }
        currentSpawTime = spawnStartTime;
        spawnCycle = 0;
    }
    public void SpawnStop(bool _spawnStopped)
    {
        spawnStopped = _spawnStopped;
    }

    private void Update()
    {
        if (spawnStopped) return;

        currentSpawTime -= Time.deltaTime;

        if (currentSpawTime < 0)
        {
            spawnBlock?.Invoke();
            spawnCycle++;

            currentSpawTime = spawnIntervalTime;
        }
    }

    private void SpawnBlockStage10()
    {
        float originX, originY, radius;
        Item.Type dropItem;
        Vector2 speed;

        if (spawnCycle % 2 == 0)
        {
            originX = -5f;
            dropItem = Item.Type.PaddleSizeUp;
            speed = new Vector2(1, 0);
        }
        else
        {
            originX = 5f;
            dropItem = Item.Type.BallPowerUp;
            speed = new Vector2(-1, 0);
        }
        originY = 1.5f;
        radius = 1f;

        // 랜덤한 극좌표 설정
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float distance = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius;

        // 극좌표를 직교 좌표로 변환
        float x = Mathf.Cos(angle) * distance + originX;
        float y = Mathf.Sin(angle) * distance + originY;

        InstantiateFloatingBlock(blockCircle, x, y, 1, dropItem, speed);
    }

    private void InstantiateFloatingBlock(GameObject blockPrefab, float x, float y, int health, Item.Type dropItem, Vector2 speed)
    {
        GameObject blockInstance = Instantiate(blockPrefab);
        blockInstance.transform.SetParent(transform);
        Block block = blockInstance.GetComponent<Block>();
        block.OnBreak += BlockBreakEvent;
        block.InitializeBlock(x, y, health, dropItem, speed);
    }

    private void InstantiateFloatingInvincibleBlock(float x, float y, float width, float height, Vector2 speed)
    {
        GameObject blockInstance = Instantiate(blockInvincible);
        blockInstance.transform.SetParent(transform);
        Block block = blockInstance.GetComponent<Block>();
        block.InitializeInvincibleBlock(x, y, width, height, speed);
    }
}

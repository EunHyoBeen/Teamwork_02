using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockContainer : MonoBehaviour
{
    [SerializeField] protected GameObject blockRectangle;
    [SerializeField] protected GameObject blockCircle;
    [SerializeField] protected GameObject blockSquare;
    [SerializeField] protected GameObject blockInvincible;

    [SerializeField] private ItemContainer itemContainer;

    private int blockRemains;

    public event Action OnAllBlockDestroyed;
    
    /// <summary>
    /// stageIndex : 스테이지 번호. 현재 1만 받음
    /// </summary>
    /// <param name="stageIndex"></param>
    public void SetStage(int stageIndex)
    {
        itemContainer.ResetItemTypeWeight();


        #region Stage Block Deployment(Detailed adjustment of item appearance probability)

        blockRemains = 0;

        int[,] BlockMap_R = null;
        float xCenter_R = 0f;
        float yCenter_R = 2f;
        float xInterval_R = 0.5f;
        float yInterval_R = 0.25f;

        int[,] BlockMap_C = null;
        float xCenter_C = 0f;
        float yCenter_C = 2f;
        float xInterval_C = 0.5f;
        float yInterval_C = 0.5f;

        int[,] BlockMap_S = null;
        float xCenter_S = 0f;
        float yCenter_S = 2f;
        float xInterval_S = 0.25f;
        float yInterval_S = 0.25f;

        switch (stageIndex)
        {
            case 1:
                xInterval_R = 0.64f;
                yInterval_R = 0.4f;
                BlockMap_R = new int[8, 6] { { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 } };
                break;
            case 2:
                yInterval_R = 0.4f;
                BlockMap_R = new int[8, 8] { { 2, 2, 2, 2, 2, 2, 2, 2 },
                                             { 1, 1, 1, 1, 1, 1, 1, 1 },
                                             { 2, 2, 2, 2, 2, 2, 2, 2 },
                                             { 1, 1, 1, 1, 1, 1, 1, 1 },
                                             { 2, 2, 2, 2, 2, 2, 2, 2 },
                                             { 1, 1, 1, 1, 1, 1, 1, 1 },
                                             { 2, 2, 2, 2, 2, 2, 2, 2 },
                                             { 1, 1, 1, 1, 1, 1, 1, 1 } };
                break;
            case 3:
                BlockMap_C = new int[6, 6] { { 2, 2, 2, 2, 2, 2, },
                                             { 2, 2, 2, 2, 2, 2, },
                                             { 2, 2, 2, 2, 2, 2, },
                                             { 2, 2, 2, 2, 2, 2, },
                                             { 2, 2, 2, 2, 2, 2, },
                                             { 2, 2, 2, 2, 2, 2, } };
                break;
            case 4:
                yCenter_R = 0.5f;
                BlockMap_R = new int[2, 10] { { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                                              { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
                itemContainer.SetItemTypeWeight(Item.Type.PaddleSizeUp, 50f);
                break;
            case 5:
                BlockMap_R = new int[10, 7] { {10,10,10,10,10,10,10 },
                                              { 9, 9, 9, 9, 9, 9, 9 },
                                              { 8, 8, 8, 8, 8, 8, 8 },
                                              { 7, 7, 7, 7, 7, 7, 7 },
                                              { 6, 6, 6, 6, 6, 6, 6 },
                                              { 5, 5, 5, 5, 5, 5, 5 },
                                              { 4, 4, 4, 4, 4, 4, 4 },
                                              { 3, 3, 3, 3, 3, 3, 3 },
                                              { 2, 2, 2, 2, 2, 2, 2 },
                                              { 1, 1, 1, 1, 1, 1, 1 } };

                BlockMap_C = new int[7, 9] { {10,10,10,10,10,10,10,10,10 },
                                             {10, 0, 0, 0, 0, 0, 0, 0,10 },
                                             {10, 0, 0, 0, 0, 0, 0, 0,10 },
                                             {10, 0, 0, 0, 0, 0, 0, 0,10 },
                                             {10, 0, 0, 0, 0, 0, 0, 0,10 },
                                             {10, 0, 0, 0, 0, 0, 0, 0,10 },
                                             {10,10,10,10, 0,10,10,10,10 } };
                break;
            case 6:
                InstantiateInvincibleBlock(-2.2f, 1, 2, 1);
                InstantiateInvincibleBlock(0, 1, 2, 1);
                InstantiateInvincibleBlock(2.2f, 1, 2, 1);
                yCenter_R = 2.75f;
                BlockMap_R = new int[8, 8] { { 0, 0, 0, 2, 2, 0, 0, 0 },
                                             { 0, 0, 0, 2, 2, 0, 0, 0 },
                                             { 0, 0, 2, 2, 2, 2, 0, 0 },
                                             { 0, 0, 2, 2, 2, 2, 0, 0 },
                                             { 0, 2, 2, 2, 2, 2, 2, 0 },
                                             { 0, 2, 2, 2, 2, 2, 2, 0 },
                                             { 2, 2, 2, 2, 2, 2, 2, 2 },
                                             { 2, 2, 2, 2, 2, 2, 2, 2 } };
                itemContainer.SetItemTypeWeight(Item.Type.PaddleSpeedDown, 50f);
                break;
            case 7:
                InstantiateInvincibleBlock(-1.739f, 0.475f, 3.818f, 1);
                InstantiateInvincibleBlock(1.739f, 0.475f, 3.818f, 1);
                InstantiateInvincibleBlock(-0.918f, 1.984f, 0.5f, 11.212f);
                InstantiateInvincibleBlock(0.918f, 1.984f, 0.5f, 11.212f);
                xInterval_R = 0.53f;
                BlockMap_R = new int[11, 10] { { 1, 1, 1, 0, 0, 0, 0, 1, 1, 1 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 },
                                               { 6, 6, 6, 0, 0, 0, 0, 6, 6, 6 } };
                itemContainer.SetItemTypeWeight(Item.Type.BallTriple, 100f);
                break;
            case 8:
                yCenter_S = 1f;
                BlockMap_S = new int[16, 14] { { 0, 0, 0, 0, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 0, 0 },
                                               { 0, 0, 0,10,10,10, 5, 5,10, 5, 0, 0, 0, 0 },
                                               { 0, 0,10, 5,10, 5, 5, 5,10, 5, 5, 5, 0, 0 },
                                               { 0, 0,10, 5,10,10, 5, 5, 5,10, 5, 5, 5, 0 },
                                               { 0, 0,10,10, 5, 5, 5, 5,10,10,10,10, 0, 0 },
                                               { 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0 },
                                               { 0, 0,10,10,10, 7,10,10,10, 0, 0, 0, 0, 0 },
                                               { 0,10,10,10,10, 7,10,10, 7,10,10,10,10, 0 },
                                               {10,10,10,10,10, 7, 7, 7, 7,10,10,10,10,10 },
                                               { 5, 5, 5,10, 7, 5, 7, 7, 5, 7,10, 5, 5, 5 },
                                               { 5, 5, 5, 5, 7, 7, 7, 7, 7, 7, 5, 5, 5, 5 },
                                               { 5, 5, 5, 7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 5 },
                                               { 0, 0, 0, 7, 7, 7, 0, 0, 7, 7, 7, 0, 0, 0 },
                                               { 0, 0,10,10,10, 0, 0, 0, 0,10,10,10, 0, 0 },
                                               { 0,10,10,10,10, 0, 0, 0, 0,10,10,10,10, 0 } };
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        float xPos = x * 0.54f - 2.43f;
                        float yPos = -y * 0.3f + 4.6f;
                        InstantiateBlockDetail(blockRectangle, xPos, yPos, (y == 0 ? 10 : 1), Item.Type.BallTriple, Vector2.zero);
                    }
                }
                break;



            case 10:   // Boss Stage
                xInterval_R = 0.75f;
                yInterval_R = 0.5f;
                BlockMap_R = new int[3, 6] { { 1, 1, 0, 0, 1, 1 },
                                             { 1, 1, 0, 0, 1, 1 },
                                             { 1, 1, 0, 0, 1, 1 } };
                itemContainer.SetItemTypeWeight(Item.Type.PaddleStopDebuff, 0f);
                itemContainer.SetItemTypeWeight(Item.Type.PaddleSizeDown, 0f);
                itemContainer.SetItemTypeWeight(Item.Type._NONE, 5f);
                break;


            default:
                xInterval_R = 0.64f;
                yInterval_R = 0.4f;
                BlockMap_R = new int[8, 6] { { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 },
                                             { 1, 1, 1, 1, 1, 1 } };
                break;
        }

        if (BlockMap_R != null) DrawBlockMap(blockRectangle, BlockMap_R, xCenter_R, yCenter_R, xInterval_R, yInterval_R);
        if (BlockMap_C != null) DrawBlockMap(blockCircle, BlockMap_C, xCenter_C, yCenter_C, xInterval_C, yInterval_C);
        if (BlockMap_S != null) DrawBlockMap(blockSquare, BlockMap_S, xCenter_S, yCenter_S, xInterval_S, yInterval_S);

        #endregion
    }

    private void DrawBlockMap(GameObject blockPrefab, int[,] blockMap, float xCenter, float yCenter, float xInterval, float yInterval)
    {
        int xLen = blockMap.GetLength(1);
        int yLen = blockMap.GetLength(0);
        float xOffset = xCenter - (xLen - 1) * xInterval / 2f;
        float yOffset = yCenter + (yLen - 1) * yInterval / 2f;

        for (int x = 0; x < xLen; x++)
        {
            for(int y = 0; y < yLen; y++)
            {
                if (blockMap[y,x] == 0) continue;
                float xPos = x * xInterval + xOffset;
                float yPos = -y * yInterval + yOffset;
                InstantiateBlock(blockPrefab, xPos, yPos, blockMap[y, x]);
            }
        }
    }

    private void InstantiateBlock(GameObject blockPrefab, float x, float y, int health)
    {
        GameObject blockInstance = Instantiate(blockPrefab);
        blockInstance.transform.SetParent(transform);
        Block block = blockInstance.GetComponent<Block>();
        block.OnBreak += BlockBreakEvent;
        block.InitializeBlock(x, y, health, Item.Type._MAX, Vector2.zero);
        blockRemains++;
    }

    private void InstantiateBlockDetail(GameObject blockPrefab, float x, float y, int health, Item.Type itemType, Vector2 speed)
    {
        GameObject blockInstance = Instantiate(blockPrefab);
        blockInstance.transform.SetParent(transform);
        Block block = blockInstance.GetComponent<Block>();
        block.OnBreak += BlockBreakEvent;
        block.InitializeBlock(x, y, health, itemType, speed);
        blockRemains++;
    }

    private void InstantiateInvincibleBlock(float x, float y, float width, float height)
    {
        GameObject blockInstance = Instantiate(blockInvincible);
        blockInstance.transform.SetParent(transform);
        Block block = blockInstance.GetComponent<Block>();
        block.InitializeInvincibleBlock(x, y, width, height, Vector2.zero);
    }

    protected void BlockBreakEvent(float x, float y, Item.Type dropItem)
    {
        blockRemains--;

        if (dropItem == Item.Type._MAX) // 무작위 아이템 생성
        {
            itemContainer.RandomItemCreation(x, y);
        }
        else if (dropItem != Item.Type._NONE) // 특정 아이템 생성
        {
            itemContainer.ItemCreation(x, y, dropItem);
        }

        // 게임 클리어
        if (blockRemains <= 0)
        {
            OnAllBlockDestroyed?.Invoke();
        }
    }

    public void Clear()
    {
        itemContainer.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

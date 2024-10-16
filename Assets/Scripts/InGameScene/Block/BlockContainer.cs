using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    [SerializeField] private GameObject blockRectangle;
    [SerializeField] private GameObject blockCircle;

    [SerializeField] private ItemContainer itemContainer;

    [SerializeField] private int StageIndex;

    private int blockRemains;

    private void Start()
    {
        StageIndex = Mathf.Clamp(StageIndex, 1, 4);
        DeployBlock(StageIndex);
    }

    /// <summary>
    /// stageIndex : 스테이지 번호. 현재 1만 받음
    /// </summary>
    /// <param name="stageIndex"></param>
    public void DeployBlock(int stageIndex)
    {
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
                BlockMap_R = new int[12, 8] { { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 3, 1, 1, 1, 1, 1, 1, 3 },
                                              { 5, 5, 5, 5, 5, 5, 5, 5 } };
                break;

            default:
                break;
        }

        if (BlockMap_R != null) DrawBlockMap(blockRectangle, BlockMap_R, xCenter_R, yCenter_R, xInterval_R, yInterval_R);
        if (BlockMap_C != null) DrawBlockMap(blockCircle, BlockMap_C, xCenter_C, yCenter_C, xInterval_C, yInterval_C);
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
        block.OnBreak += BreakBlock;
        block.InitializeBlock(x, y, health);
        blockRemains++;
    }

    public void BreakBlock(float x, float y)
    {
        blockRemains--;
        itemContainer.RandomItemCreation(x, y);
        if (blockRemains <= 0)
        {
            // TODO : 게임 클리어 호출
        }
    }
}

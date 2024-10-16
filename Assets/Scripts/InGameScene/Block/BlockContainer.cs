using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    [SerializeField] private GameObject blockRectangle;
    [SerializeField] private GameObject blockCircle;

    [SerializeField] private int StageIndex;

    private int blockRemains;

    private void Start()
    {
        StageIndex = Mathf.Clamp(StageIndex, 1, 3);
        DeployBlock(StageIndex);
    }

    /// <summary>
    /// stageIndex : 스테이지 번호. 현재 1만 받음
    /// </summary>
    /// <param name="stageIndex"></param>
    public void DeployBlock(int stageIndex)
    {
        blockRemains = 0;

        int[,] BlockRMap = null;
        int[,] BlockCMap = null;

        switch (stageIndex)
        {
            case 1:
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        InstantiateBlock(blockRectangle, x * 0.5f - 1.25f, y * 0.25f + 0.5f, 1);
                    }
                }
                break;
            case 2:
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if (y % 2 == 0) InstantiateBlock(blockRectangle, x * 0.5f - 1.25f, y * 0.25f + 0.5f, 1);
                        else InstantiateBlock(blockRectangle, x * 0.5f - 1.25f, y * 0.25f + 0.5f, 2);
                    }
                }
                break;
            case 3:
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        InstantiateBlock(blockCircle, x * 0.5f - 1.25f, y * 0.5f + 0.5f, 2);
                    }
                }
                break;
            case 4:
                
                break;

            default:
                break;
        }

        
    }
    private void InstantiateBlock(GameObject gameObject, float x, float y, int health)
    {
        GameObject blockInstance = Instantiate(gameObject);
        blockInstance.transform.parent = transform;
        Block block = blockInstance.GetComponent<Block>();
        block.OnBreak += BreakBlock;
        block.InitializeBlock(x, y, health);
        blockRemains++;
    }

    public void BreakBlock()
    {
        blockRemains--;
        if (blockRemains <= 0)
        {
            // TODO : 게임 클리어 호출
        }
    }
}

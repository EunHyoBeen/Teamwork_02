using System.Collections.Generic;
using UnityEngine;

public class BallContainer : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    private int activeBalls = 0;
    private int maxBallNumber = 20;
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                //obj.GetComponent<BallController>().OnDeath += OnDeath();
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        activeBalls++;
        return obj;
    }

    public void PowerChange(int power)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<BallController>().PowerChange(power);
        }
    }

    public void SpeedChange(int speed)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<BallController>().SpeedChange(speed);
        }
    }

    public void MultiplyBalls(int multiplier)                   // 공의 갯수 multiplier 배로 늘림, 공 개수의 최대치는 maxBallNumber
    {
        List<Transform> activechild = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                activechild.Add(child);
            }
        }
        foreach (Transform transform in activechild)
        {
            for (int i = 0; i < multiplier; i++)
            {
                if (activeBalls >= maxBallNumber) return;
                GameObject ball = SpawnFromPool("Ball");
                ball.transform.position = transform.transform.position;
            }
        }
    }
    //void OnDeath()
    //{
    //    if (AllBallsInactive)
    //    {
            
    //    }
    //}

    //private bool AllBallsInactive()
    //{
    //    foreach (Transform child in this.transform)
    //    {
    //        if (child.gameObject.activeSelf) 
    //        { 
    //            return false; 
    //        }
    //    }
    //    return true;
}
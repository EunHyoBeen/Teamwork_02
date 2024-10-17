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

    public void TripleBalls()
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                GameObject Ball1 = SpawnFromPool("Ball");
                Ball1.transform.position = child.transform.position;
                GameObject Ball2 = SpawnFromPool("Ball");
                Ball2.transform.position = child.transform.position;
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
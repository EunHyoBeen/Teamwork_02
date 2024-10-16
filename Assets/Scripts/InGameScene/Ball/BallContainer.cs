using System.Collections.Generic;
using UnityEngine;

public class BallContainer : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public LayerMask layer;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<LayerMask, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<LayerMask, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.layer, objectPool);
        }
    }
    public GameObject SpawnFromPool(LayerMask layer)
    {
        if (!PoolDictionary.ContainsKey(layer))
            return null;

        GameObject obj = PoolDictionary[layer].Dequeue();
        PoolDictionary[layer].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }
}
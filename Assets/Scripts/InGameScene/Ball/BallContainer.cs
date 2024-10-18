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
    [SerializeField][Range(0f, 20f)] private float initialSpeed = 5f;

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
                obj.GetComponent<BallController>().SetInitialSpeed(initialSpeed);
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

    public void InitialSpeedChange(float speed)
    {
        initialSpeed = speed;
    }

    public void PowerChange(int power)                          // 컨테이너 안의 모든 공의 공격력을 power로 맞춤
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<BallItemHandler>().PowerUpItem(power);
        }
    }

    public void SpeedChange(float speed)                          // 컨테이너 안의 모든 공에 스피드에 speed 만큼 더함
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<BallItemHandler>().SpeedUpItem(speed);
        }
    }

    public void MultiplyBalls(int multiplier)                   // 공의 갯수 multiplier 배로 늘림, 공 개수의 최대치는 maxBallNumber
    {
        List<Transform> activechild = new List<Transform>();
        foreach (Transform child in transform)                  // 현재 활성화 된 공을 리스트에 추가
        {
            if (child.gameObject.activeSelf)
            {
                activechild.Add(child);
            }
        }
        foreach (Transform transform in activechild)            // 활성화된 공에 공 개수를 몇배로 함
        {
            for (int i = 0; i < multiplier; i++)
            {
                if (activeBalls >= maxBallNumber) return;
                GameObject ball = SpawnFromPool("Ball");
                ball.transform.position = transform.position;
            }
        }
    }

    public void ResetBalls()
    {
        foreach (Transform child in transform)                  // 현재 활성화 된 공을 리스트에 추가
        {
            child.gameObject.SetActive(false);
        }
        activeBalls = 0;
    }
    //public void OnDeath()
    //{
    //    if (AllBallsInactive)
    //    {

    //    }
    //}

    public bool AllBallsFall()
    {
        return activeBalls == 0;
    }
}

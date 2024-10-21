using System;
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
    public event Action OnDeath;

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
                BallController ballcontroller = obj.GetComponent<BallController>();
                ballcontroller.SetInitialSpeed(initialSpeed);       //각각의 공에 초기 속도 따로 지정
                ballcontroller.OnTouchFloor += TouchFloor;
                obj.SetActive(false);
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

    public void InitialSpeedChange(float speed)                 // 스테이지 시작시 모든 공의 초기속도 지정할 때 쓰일 함수
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
        foreach (Transform transform in activechild)            // 활성화된 공에 공 개수를 'multiplier'배로 만듬
        {
            for (int i = 1; i < multiplier; i++)
            {
                if (activeBalls >= maxBallNumber) return;
                GameObject ball = SpawnFromPool("Ball");
                ball.transform.position = transform.position;
            }
        }
    }

    public void ResetBalls()                    // 모든 공 비활성화
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        activeBalls = 0;
    }

    private void TouchFloor()
    {
        activeBalls--;
        if (activeBalls <= 0) CallDeath();
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }

    public bool AllBallsFall()      // 모든 공이 떨어져서 비활성화 됐는지
    {
        return activeBalls == 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public int level;

    [System.Serializable]
    public class SpawnData
    {
        public int spriteType;
        public float spawnTime;
        public int health;
        public float speed;

    }

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

    }

    void Start()
    {
        for (int index = 0; index < spawnPoint.Length; ++index)
        {
            switch (spawnPoint[index].tag)
            {
                case "1":
                    level = 0;
                    Spawn(index);
                    break;
                case "2":
                    level = 1;
                    Spawn(index);
                    break;
                case "3":
                    level = 2;
                    Spawn(index);
                    break;
                case "4":
                    level = 3;
                    Spawn(index);
                    break;
            }
        }
    }

    void Spawn(int index)
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[index].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}
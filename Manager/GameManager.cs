using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Game Control

    // Player Info
    public float health;
    public float maxHealth;
    public bool isLive;

    // 아이템 + 무기
    public int[] inventory = new int[10];

    //Game Object
    public PoolManager pool;
    public Player player;

    public float speed;
    public float power;

    // update
    public bool temp;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pool = FindObjectOfType<PoolManager>();
    }
    public void GameStart()
    {
        health = maxHealth;
        isLive = true;
    }
    public void GameOver()
    {
        for (int index = 0; index < pool.transform.childCount; index++)
        {
            Transform child = pool.transform.GetChild(index);

            // 해당 자식 게임 오브젝트의 Enemy 컴포넌트 가져오기
            Enemy enemy = child.GetComponent<Enemy>();
            Scanner scanner = child.GetComponent<Scanner>();

            if (enemy != null)
            {
                enemy.target = null;
                enemy.targetPlayer = null;
            }

            if (scanner != null)
            {
                scanner.nearestTarget = null;
            }

            // 자식 게임 오브젝트 비활성화
            child.gameObject.SetActive(false);

        }
    }
}


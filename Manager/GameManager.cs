using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
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
        } else
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
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        GotoVillage();
    }
    public void GotoVillage()
    {
        //reset?
        SceneManager.LoadScene("Town");
    }    
}

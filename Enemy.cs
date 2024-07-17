using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Spawner;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    private float localSpeed;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive;
    int level;
    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    public Scanner scanner;
    public Transform targetPlayer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        if (isLive)
        {
            searchTarget();
        }
    }
    void FixedUpdate()
    {

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
    }

    void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;

    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        localSpeed = data.speed;
        maxHealth = data.health;
        health = data.health;
        level = data.spriteType;
    }

    //몬스터가 죽었을 때 실행할 로직
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Tag가 Bullet과 몬스터가 살아있지 않다면 실행하지 않음
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }

        //Bullet의 damage를  몬스터의 health에서 감소
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(PauseEnemy());        //코루틴 함수를 부를땐 이렇게 불러야 한다.
        collision.gameObject.SetActive(false);
        //몬스터의 체력이 0이상이라면 몬스터 애니메이션의 Hit를 작동시킴
        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            //죽었을 때 비활성화
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            //죽었을 경우 플레이어를 쫒아가는 걸 멈추고 애니메이션 실행
            anim.SetBool("Dead", true);
            rigid.velocity = Vector2.zero;
            //몬스터가 죽었을 때 리워드 생성
            int random = Random.Range(1, 100);
            if (random < 100)
            {
                GameObject reward = GameManager.instance.pool.Get(2);
                reward.GetComponent<Item>().Init(level);
                reward.transform.position = GetComponent<Enemy>().transform.position;
            }
        }

    }

    IEnumerator PauseEnemy()
    {
        speed = 0;
        yield return new WaitForSeconds(1f);
        speed = localSpeed;
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    void searchTarget()
    {
        // 플레이어를 처음 발견하면 타겟을 설정
        if (targetPlayer == null && scanner.nearestTarget != null && scanner.nearestTarget.CompareTag("Player"))
        {
            targetPlayer = scanner.nearestTarget;
        }

        // 타겟이 있으면 해당 방향으로 이동
        if (targetPlayer != null)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
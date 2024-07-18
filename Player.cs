using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    static Player instance;
    // 이미지
    private SpriteRenderer spriter;
    private Animator anim;

    // 대화
    public TextManager textManager;
    public RewardManager rewardManager;

    // 이동
    private Rigidbody2D rigid;
    private Vector2 inputVec;
    private Vector2 nextVec;
    private Vector2 lastVec;
    public float speed;
    public float localSpeed;

    // 공격
    public Weapon weapon;
    private float timer;

    // 스캔
    private GameObject scanObj;
    private RaycastHit2D hitted;

    //퀘스트
    public List<int> questList = new List<int>();
    private Tuple<int, int> proceedingQuest = new Tuple<int, int>(-1, -1);  // questId, nextNpcId

    private QuestData questData;
    private string[] dialogue = null;
    private int nextNpc = -1;
    private int endNpc = -1;

    private int curQuestId = -1;
    private int scriptCnt = 1;
    private bool inQuest;
    private bool nextStep;
    public bool onDialogue;
    private bool getReward;
    //update
    private bool clear;

    private int[] questItems;
    //private int[] inventory;

    private string[] questScroll;

    //맵이동
    public string curMapName;
    public string dir;

    private void Awake()
    {
        // 초기화
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            anim = GetComponent<Animator>();
            textManager = FindObjectOfType<TextManager>();
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            instance.textManager = FindObjectOfType<TextManager>();
        }
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //inventory = new int[4];
        weapon = FindObjectOfType<Weapon>();
        localSpeed = speed;
    }
    private void Update()
    {
        // 대화 && 공격 && 스캔 업데이트
        if (!onDialogue && !getReward)
        {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            if (inputVec.magnitude > 0)
            {
                lastVec = inputVec;
            }
        }
        else
        {
            inputVec = lastVec;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (curMapName != "Village")
            {
                weapon.dir = lastVec;

                timer += Time.deltaTime;
                if (timer > weapon.speed)
                {
                    timer = 0f;
                    weapon.Fire();
                }
            }

            if (curMapName == "Village" && !GameManager.instance.temp)
            {
                Scan();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!onDialogue)
        {
            //update
            int addSpeed = 0;
            if (inputVec.x != 0) addSpeed = 1;            
            nextVec = inputVec.normalized * (speed + addSpeed) * Time.deltaTime;
            Debug.Log("nextVec " + nextVec.magnitude);
            rigid.MovePosition(rigid.position + nextVec);
        }
    }
    private void LateUpdate()
    {
        if (inputVec.magnitude > 0)
        {
            anim.SetFloat("x", lastVec.x);
            anim.SetFloat("y", lastVec.y);
            anim.SetBool("moving", true);
        }

        if (inputVec.magnitude <= 0 || onDialogue)
        {
            anim.SetBool("moving", false);
        }
        UpdateWeaponDirection();
    }
    void UpdateWeaponDirection()
    {
        if (lastVec.x > 0)
        {
            weapon.SetDirection(Vector2.right);
        }
        else if (lastVec.x < 0)
        {
            weapon.SetDirection(Vector2.left);
        }
    }
    public void Scan()
    {
        hitted = Physics2D.Raycast(this.transform.position, inputVec, 50.0f, LayerMask.GetMask("Object"));
        if (hitted.collider != null && Input.GetKeyDown(KeyCode.Space))
        {
            lastVec = inputVec;
            scanObj = hitted.collider.gameObject;

            NpcData npcData = scanObj.GetComponent<NpcData>();
            questData = scanObj.GetComponent<QuestData>();
            nextStep = true;
            // 퀘스트 유무 확인
            if (questData != null && questList.Count != 0)
            {
                if (npcData.haveQuest && questList[0] == questData.questId && proceedingQuest.Item1 != questData.questId)
                {
                    EnrollQuest();
                }
                // 퀘스트 진행
                if (npcData.id == nextNpc || getReward)
                {
                    if (questData.haveItems)
                    {
                        FindInventory();
                    }
                    else
                    {
                        dialogue = questData.FindSubScript(curQuestId, scriptCnt++).scripts;
                    }

                    if (nextStep)
                    {
                        NextStep();
                    }
                    inQuest = true;
                }
            }
            // 일반적인 대화
            if (!inQuest && dialogue == null)
            {
                dialogue = npcData.normalScript;
            }

            onDialogue = textManager.Action(scanObj, dialogue, getReward);
            if (getReward)
            {
                getReward = onDialogue;
            }
            CheckQuestEnd(npcData);
            ResetDialogue();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            Item obj = collision.GetComponent<Item>();
            GameManager.instance.inventory[obj.no]++;
            obj.gameObject.SetActive(false);
        }
        if (collision.CompareTag("TrickBtn 1"))
        {
            GameObject.FindWithTag("Trick 1 Box").SetActive(false);
            GameObject.FindWithTag("Trick1").GetComponent<TilemapRenderer>().sortingOrder = 2;
            GameObject.FindWithTag("TrickBtn 1").SetActive(false);
        }
        if (collision.CompareTag("TrickBtn 2"))
        {
            GameObject.FindWithTag("Trick 2 Box").SetActive(false);
            GameObject.FindWithTag("Trick2").GetComponent<TilemapRenderer>().sortingOrder = 2;
            GameObject.FindWithTag("TrickBtn 2").SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
            GameManager.instance.health -= Time.deltaTime * 5;

        if (other.CompareTag("River"))
        {
            speed = localSpeed - 1f;
        }
        else
        {
            speed = localSpeed;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
    private void EnrollQuest()
    {
        proceedingQuest = new Tuple<int, int>(questList[0], questData.nextNpcId[questData.questId]);
        curQuestId = proceedingQuest.Item1;
        if (questData.haveItems)
        {
            questItems = questData.questItems;
        }
        dialogue = questData.mainScript;
        questScroll = dialogue;
        nextNpc = questData.nextNpcId[curQuestId];
    }
    private void FindInventory()
    {
        clear = true;
        for (int i = 0; i < questItems.Length; ++i)
        {
            if (GameManager.instance.inventory[i] < questItems[i])
            {
                clear = false;
                break;
            }
        }

        if (clear)
        {            
            for (int i = 0; i < questItems.Length; ++i)
            {
                GameManager.instance.inventory[i] -= questItems[i];
            }
            if (questData.hasReward)
            {
                getReward = true;                
                dialogue = questData.rewardScripts[curQuestId].scripts;
            }
            else
            {
                dialogue = questData.FindSubScript(curQuestId, scriptCnt++).scripts;
            }
        }
        else
        {
            //update
            nextStep = false;
            clear = false;
            dialogue = questData.tellOffScripts[curQuestId].scripts;
        }
    }
    private void NextStep()
    {
        if (questData.nextNpcId.Length > 0 && questData.nextNpcId.Length > curQuestId)
        {
            nextNpc = questData.nextNpcId[curQuestId];
        }
        else
        {
            nextNpc = -1;
        }
    }
    private void CheckQuestEnd(NpcData npcData)
    {
        //update
        if (questData != null && questData.EndIdList != null)
        {
            int temp = questData.FindEndId(curQuestId);
            if (temp != -1)
            {
                endNpc = temp;
            }
        }
        // 퀘스트 삭제 및 초기화
        //update
        if (endNpc == npcData.id && inQuest && !onDialogue && clear)
        {
            questList.RemoveAt(0);
            scriptCnt = 1;
            endNpc = -1;
            nextNpc = -1;
        }
    }
    private void ResetDialogue()
    {
        if (!onDialogue)
        {
            dialogue = null;
            inQuest = false;
        }
    }
}

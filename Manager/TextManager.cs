using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    private static TextManager instance;
    public GameObject panel;
    public GameObject rewardButtons;
    public GameObject menuSet;

    public TextMeshProUGUI text;
    public TextMeshProUGUI rewardText;
    public TalkManager talkBox;

    public int npcId;
    public string[] dialogue;
    public GameObject scanObj;
    public Image image;
    public bool onDialogue;
    private int talkIndex;
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
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }
    public bool Action(GameObject obj, string[] script, bool reward)
    {
        scanObj = obj;
        NpcData objData = scanObj.GetComponent<NpcData>();       
        onDialogue = Talk(objData.id, objData.isNpc, script, reward);
        
        if (reward && talkIndex == script.Length)
        {
            GameManager.instance.temp = true;
            rewardButtons.SetActive(onDialogue);
        }
        panel.SetActive(onDialogue);
        return onDialogue;
    }
    bool Talk(int objId, bool isNpc, string[] script, bool reward)
    {
        string ment = "";
        onDialogue = true;
        ment = talkBox.GetTalk(objId, talkIndex, script,reward);
        
        if (ment == null)
        {
            onDialogue = false;
            talkIndex = 0;
            return onDialogue;
        }
        text.text = ment;
        talkIndex++;
        return onDialogue;
    }

}

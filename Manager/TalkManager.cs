using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private static TalkManager instance;
    Dictionary<int, Sprite> npcSprite;
    Dictionary<int, Sprite> objSprite;
    public Sprite[] npcImages;
    public Sprite[] objImages;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        instance = this;
    }
    public string GetTalk(int objId, int talkIndex, string[] script, bool reward)
    {



        if (talkIndex >= script.Length)
        {
            return null;
        }
        else
        {
            return script[talkIndex];
        }
    }
    public Sprite GetImage(int objId, int imageIdx, bool isNpc)
    {
        if (isNpc)
        {
            return npcSprite[objId + imageIdx];
        }
        else
        {
            return objSprite[objId + imageIdx];
        }
    }
}

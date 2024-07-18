using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    [Serializable]
    public class ScriptArray
    {
        public string[] scripts;
    }
    [Serializable]
    public class KeyAndScriptArray
    {
        public int scriptCnt;
        public ScriptArray scriptArray;
    }

    [Serializable]
    public class KeyAndScriptArrayList
    {
        public List<KeyAndScriptArray> list;
    }
    [Serializable]
    public class QuestIdAndEndId
    {
        public int questId;
        public int endNpcId;
    }

    [Serializable]
    public class QuestIdAndEndIdList
    {
        public List<QuestIdAndEndId> list;
    }


    public int questId;
    public bool haveItems;
    public int[] questItems;
    public bool hasReward;

    public int[] nextNpcId;
    public QuestIdAndEndIdList EndIdList;

    public string[] mainScript;
    public KeyAndScriptArrayList subScripts;
    public List<ScriptArray> tellOffScripts;
    public List<ScriptArray> rewardScripts;
    public ScriptArray FindSubScript(int questId, int scriptCnt)
    {
        if (subScripts != null && subScripts.list != null &&
            questId >= 0 && questId <= subScripts.list.Count)
        {
            KeyAndScriptArray kvp = subScripts.list[questId];
            if (kvp.scriptCnt == scriptCnt)
            {
                return kvp.scriptArray;
            }
            else
            {
                Debug.Log("kvp.value != scriptCnt");
            }

        }
        return null;
    }
    public int FindEndId(int questId)
    {
        if (EndIdList != null && EndIdList.list != null && questId > 0)
        {
            List<QuestIdAndEndId> endValuePairs = EndIdList.list;
            foreach (QuestIdAndEndId kvp in endValuePairs)
            {
                if (kvp.questId == questId)
                {
                    return kvp.endNpcId;
                }
            }
        }
        return -1;
    }
}

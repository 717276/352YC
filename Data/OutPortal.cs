using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPortal : MonoBehaviour
{
    public string from;
    public string fromDir;
    void Start()
    {
        Player player = FindObjectOfType<Player>();        
        if (from == player.curMapName && fromDir == player.dir)
        {               
            player.transform.position = this.transform.position;           
        }
    }
}

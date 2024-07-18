using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPortal : MonoBehaviour
{
    public string from;
    public string fromDir;
    public BoxCollider2D bound;
    private CameraManager cm;
    void Start()
    {
        Player player = FindObjectOfType<Player>();
        cm = FindObjectOfType<CameraManager>();

        if (from == player.curMapName && fromDir == player.dir)
        {
            cm.SetBound(bound);
            player.transform.position = this.transform.position;           
        }
    }
}

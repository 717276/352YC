using UnityEngine;

public class Item : MonoBehaviour
{
    public int no;
    public Sprite[] sprites;
    SpriteRenderer spriter;
    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }
    public void Init(int level)
    {
        no = level;
        spriter.sprite = sprites[level];
    }
}
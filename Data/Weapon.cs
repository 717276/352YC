using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Vector2 dir;
    private Vector3 localPosition;
    private SpriteRenderer spriter;
    public int prefabId;

    public float speed;
    public float damage;
    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        localPosition = transform.position;
    }

    void LateUpdate()
    {
        if (dir.x > 0)
        {
            transform.localPosition = new Vector3(Mathf.Abs(localPosition.x), localPosition.y, localPosition.z);
            spriter.flipX = false;
        }
        else if (dir.x < 0)
        {
            transform.localPosition = new Vector3(-Mathf.Abs(localPosition.x), localPosition.y, localPosition.z);
            spriter.flipX = true;
        }
    }
    public void SetDirection(Vector2 direction)
    {
        dir = direction;
    }
    public void Fire()
    {
        // instance, pool        
        Transform bullet = GameManager.instance.pool.Get(1).transform;

        bullet.SetPositionAndRotation(GameManager.instance.player.transform.position, Quaternion.FromToRotation(Vector3.up, dir));
        bullet.GetComponent<Bullet>().Init(damage, speed, dir.normalized);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string to;
    public string toDir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();

            player.curMapName = to;
            player.dir = toDir;
            SceneManager.LoadScene(to);
        }
    }
}

using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private static RewardManager instance;

    public GameObject rewardButtons;
    private void Check()
    {
        rewardButtons.SetActive(false);
        GameManager.instance.temp = false;
    }
    public void UpgradeSpeed()
    {
        Debug.Log(GameManager.instance.player.speed + "1");
        GameManager.instance.player.speed += 1;
        Debug.Log(GameManager.instance.player.speed + "2");
        Check();
    }
    public void UpgradePower()
    {

        Weapon w = FindFirstObjectByType<Weapon>();
        Debug.Log(w.damage);
        w.damage += 5;
        Debug.Log(w.damage);
        Check();
    }
    public void UpgradeWeaponSpeed()
    {
        Weapon w = FindFirstObjectByType<Weapon>();
        w.speed -= 0.03f;
        Check();
    }
}

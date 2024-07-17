using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        GameManager.instance.speed += 1;
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

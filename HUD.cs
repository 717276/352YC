using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //다루게 될 데이터를 미리 열겨형타입 enum으로 선언
    public enum InfoType { Health, Bronze, Silver, Gold, Ruby, Power, Speed }
    //사용하기 위한 변수선언
    public InfoType type;
    //using UnityEngine.UI 를 사용
    Text myText;
    Slider mySlider;


    //초기화 작업
    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.Bronze:
                myText.text = string.Format("{0:F0}", GameManager.instance.inventory[0]);
                break;
            case InfoType.Silver:
                myText.text = string.Format("{0:F0}", GameManager.instance.inventory[1]);
                break;
            case InfoType.Gold:
                myText.text = string.Format("{0:F0}", GameManager.instance.inventory[2]);
                break;
            case InfoType.Ruby:
                myText.text = string.Format("{0:F0}", GameManager.instance.inventory[3]);
                break;
            case InfoType.Power:
                if (FindObjectOfType<Weapon>() != null)
                {
                    float power = FindAnyObjectByType<Weapon>().damage;
                    myText.text = string.Format("{0:F1}", power);
                }
                break;
            case InfoType.Speed:
                myText.text = string.Format("{0:F1}", GameManager.instance.player.speed);
                break;
        }
    }
}

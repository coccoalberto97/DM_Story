using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    public GameObject bossWalls;
    public GameObject boss;

    private void Start()
    {
        GameEvents.instance.OnPlayerEntersBoosArea += OnPlayerEntersBoosArea;
        GameEvents.instance.OnBossDeath += OnBossDeath;

    }


    private void OnPlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        Boss b = boss.GetComponent<Boss>();
        if (b != null && b.bossTag.Equals(bossTag))
        {
            if (bossWalls != null)
            {
                bossWalls.SetActive(true);
            }

            if (boss != null)
            {
                boss.SetActive(true);
            }
        }
    }


    private void OnBossDeath(string bossTag)
    {
        Boss b = boss.GetComponent<Boss>();
        if (b != null && b.bossTag.Equals(bossTag))
        {
            if (bossWalls != null)
            {
                bossWalls.SetActive(false);
            }
        }
    }
}

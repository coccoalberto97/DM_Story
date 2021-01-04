using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{

    public static GameEvents instance;
    public event Action<string, int> OnPlayerEntersBoosArea;
    public event Action<string> OnBossDeath;
    public event Action<int> OnBossHit;
    public event Action OnPlayerModHealth;

    private void Awake()
    {
        instance = this;
    }

    public void PlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        OnPlayerEntersBoosArea?.Invoke(bossTag, maxHealth);
    }

    public void BossDeath(string bossTag)
    {
        OnBossDeath?.Invoke(bossTag);
    }

    public void BossHit(int boosHealth)
    {
        OnBossHit?.Invoke(boosHealth);
    }

    public void PlayerModHealth()
    {
        OnPlayerModHealth?.Invoke();
    }
}

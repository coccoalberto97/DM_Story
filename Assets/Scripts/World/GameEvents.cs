using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{

    public static GameEvents instance;
    public event Action<string, int> OnPlayerEntersBoosArea;
    public event Action<string> OnBossDeath;
    public event Action<int> OnBossHit;
    public event Action OnPlayerModHealth;
    public event Action<AudioClip> OnPlaySFXAudioClip;
    public event Action OnStopSFXAudioClip;
    public event Action<AudioClip> OnPlayOSTAudioClip;
    public event Action OnStopOSTAudioClip;
    public event Action OnStopAudio;

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

    /// <summary>
    /// Manda all' Audio manager il segnale di riprodurre l' sfx
    /// </summary>
    /// <param name="audioClip">Sfx da riprodurre</param>
    public void PlaySFXAudioClip(AudioClip audioClip)
    {
        OnPlaySFXAudioClip?.Invoke(audioClip);
    }

    /// <summary>
    /// Manda all' Audio manager il segnale di terminare tutti i suoni sfx (ma non le ost)
    /// </summary>
    public void StopSFXAudioClip()
    {
        OnStopSFXAudioClip?.Invoke();
    }

    /// <summary>
    /// Manda all' Audio manager il segnale di riprodurre la ost
    /// </summary>
    /// <param name="audioClip">Ost da riprodurre</param>
    public void PlayOSTAudioClip(AudioClip audioClip)
    {
        OnPlayOSTAudioClip?.Invoke(audioClip);
    }

    /// <summary>
    /// Manda all' Audio manager il segnale di terminare la ost in riproduzione (ma non i suoni sfx)
    /// </summary>
    public void StopOSTAudioClip()
    {
        OnStopOSTAudioClip?.Invoke();
    }
    /// <summary>
    /// Manda all' Audio manager il segnale di terminare tutti i suoni e le ost in riproduzione
    /// </summary>
    public void StopAudio()
    {
        OnStopAudio?.Invoke();
    }
}

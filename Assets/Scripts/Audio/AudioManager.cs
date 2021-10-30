using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource sfxSource;
    public AudioSource ostSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameEvents.instance.OnPlaySFXAudioClip += PlaySFXAudioClip;
        GameEvents.instance.OnStopSFXAudioClip += StopSFXAudioClip;
        GameEvents.instance.OnPlayOSTAudioClip += PlayOSTAudioClip;
        GameEvents.instance.OnStopOSTAudioClip += StopOSTAudioClip;
        GameEvents.instance.OnStopAudio += StopAudio;
    }

    private void PlaySFXAudioClip(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    private void StopSFXAudioClip()
    {
        sfxSource.Stop();
    }

    private void PlayOSTAudioClip(AudioClip clip)
    {
        ostSource.clip = clip;
        ostSource.Play();
    }

    private void StopOSTAudioClip()
    {
        ostSource.Stop();
    }

    private void StopAudio()
    {
        ostSource.Stop();
        sfxSource.Stop();
    }
}

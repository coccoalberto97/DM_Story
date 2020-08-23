﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableParticleSystem : MonoBehaviour
{
    ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particles.Play();
        Invoke("DisableObject", particles.main.duration + 1f);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

}

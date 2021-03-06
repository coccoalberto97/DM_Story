﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public Transform target;
    public float dampingTime = 1f;
    public float ppu = 16f;

    private Vector3 velocity;
    private Vector3 proxyPosition = new Vector3();



    void LateUpdate()
    {

        proxyPosition = Vector3.SmoothDamp(proxyPosition, target.position, ref velocity, dampingTime);

        transform.position = new Vector3(Mathf.Round(proxyPosition.x * ppu) / ppu, Mathf.Round(proxyPosition.y * ppu) / ppu, -10f);
    }

}

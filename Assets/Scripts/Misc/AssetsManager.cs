using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    public static AssetsManager instance;
    public Transform damageIndicator;

    private void Awake()
    {
        instance = this;
    }
}

using System;
using UnityEngine;

public class TileMapEventHandler : MonoBehaviour
{
    private void Start()
    {
        GameEvents.instance.OnPlayerEntersBoosArea += OnPlayerEntersBoosArea;
    }


    private void OnPlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        Debug.Log("player entered boss area " + bossTag);
    }
}

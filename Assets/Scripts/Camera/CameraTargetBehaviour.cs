using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetBehaviour : MonoBehaviour
{
    public float distance = 4f;
    private Player player;

    void Start()
    {
        player = Player.instance;
    }

    private void Update()
    {
        Vector3 localPos = new Vector3(player.IsFacingRight() ? 1f : -1f, General.DirectionToVector(player.GetDirection()).y, 0f);

        transform.localPosition = localPos * distance;
    }
}

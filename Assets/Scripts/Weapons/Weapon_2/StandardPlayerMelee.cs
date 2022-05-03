using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardPlayerMelee : MeleeDamageSource
{

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = General.DirectionToVector(player.GetDirection());
        transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(-direction.y, -direction.x);
        time = maxTime;
    }
}
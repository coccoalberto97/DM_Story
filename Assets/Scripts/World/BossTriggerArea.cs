using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerArea : Trigger
{
    public GameObject boss;

    protected override void TriggerAction()
    {
        base.TriggerAction();
        Boss b = boss.GetComponent<Boss>();
        if (b != null)
        {
            GameEvents.instance.PlayerEntersBoosArea(b.bossTag, b.maxHealth);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Trigger
{
    public Dialogue dialogue;

    protected override void TriggerAction()
    {
        base.TriggerAction();
        Hud.instance.StartDialogue(dialogue);
    }
}

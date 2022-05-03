using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableHeart : Droppable
{

    public int restoreValue = 2;

    public override void Effect()
    {
        Player.instance.AddHealth(restoreValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class HealthStation : Interactable
{
    //private SpriteRenderer renderer;

    private void Start()
    {
        //enderer = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        Player.instance.restoreFullHealth();
    }
}

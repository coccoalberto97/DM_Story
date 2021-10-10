using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : Interactable
{
    //private SpriteRenderer renderer;

    private void Start()
    {
        //enderer = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        //todo popup
        SaveSystem.SaveData();

    }

    public void tmp() {
        Player.instance.LoadData(SaveSystem.LoadData());
    }
}

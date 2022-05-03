using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "item/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name = "Name";
    [TextArea(3, 5)]
    public string description = "DESCRIPTION";

    public Sprite sprite;
    public int handleOffsetX = -11;
    public int handleOffestY = -1;

    public List<WeaponLevelDescriptor> shootables;

    public WeaponLevelDescriptor currentDescriptor;


    public void SetShootable(int currentExp)
    {
        currentDescriptor = null;
        if (shootables != null && shootables.Count > 0)
        {
            foreach (WeaponLevelDescriptor weaponLevelDescriptor in shootables)
            {
                if (weaponLevelDescriptor.requiredExp <= currentExp)
                {
                    currentDescriptor = weaponLevelDescriptor;
                }
                else
                {
                    break;
                }
            }
        }
    }
}

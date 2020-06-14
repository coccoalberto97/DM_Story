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

    public GameObject shootablePrefab;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public Sprite charImage;
    [TextArea(3, 9)]
    public string text;
}

using UnityEngine;
using System;
[Serializable]
public class Sentence
{
    [TextArea(3, 9)]
    public string text;
    public CharNameEnum charName = CharNameEnum.EMPTY;
    public AudioClip ost;
    public AudioClip voice;
    public int charStop = -1;
}

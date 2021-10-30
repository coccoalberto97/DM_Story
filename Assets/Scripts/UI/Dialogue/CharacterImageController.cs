using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageController : MonoBehaviour
{
    public GameObject charPrefab;


    private DialogueCharacter[] chars;


    public void initChars(DialogueCharacter[] characters)
    {
        chars = characters;

        foreach (DialogueCharacter c in chars)
        {
            Instantiate(charPrefab, transform);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageController : MonoBehaviour
{
    public CharPrefab charPrefab;


    private List<CharPrefab> chars = new List<CharPrefab>();

    public void InitChars(DialogueCharacter[] characters)
    {
        gameObject.SetActive(true);
        foreach (CharPrefab c in chars)
        {
            Destroy(c);
        }

        chars = new List<CharPrefab>();

        foreach (DialogueCharacter c in characters)
        {
            CharPrefab prefab = Instantiate(charPrefab, transform);
            prefab.image.texture = c.charImage;
            prefab.nameLabel.text = c.charName.ToString();
            prefab.charName = c.charName;
            prefab.SetActive(CharNameEnum.EMPTY);
            chars.Add(prefab);
        }
    }

    public void EnableChar(CharNameEnum charname)
    {
        foreach (CharPrefab c in chars)
        {
            c.SetActive(charname);
        }
    }

    public void HideChars()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharPrefab : MonoBehaviour
{
    public RawImage image;
    public TextMeshProUGUI nameLabel;
    public CharNameEnum charName;

    public void SetActive(CharNameEnum charName)
    {
        if (this.charName == charName)
        {
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.2f);
        }
    }
}

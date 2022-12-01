using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshPro))]
public class DamageIndicator : MonoBehaviour
{
    private TextMeshPro textLabel;
    private Color textColor;
    private float disappearTimer;
    private float verticalSpeed;
    public static int sortingOrder = 0;

    private void Awake()
    {
        textLabel = GetComponent<TextMeshPro>();
        textColor = textLabel.color;
        disappearTimer = 0.6f;
        verticalSpeed = 0.002f;
    }

    private void Update()
    {
        transform.position += new Vector3(0, verticalSpeed, 0);
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a = textColor.a -= 2f * Time.deltaTime;
            textLabel.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public static DamageIndicator Spawn(int damage, Vector3 position, ElementEffectiveness elementEffectiveness)
    {
        //TODO aggiungere gli sprite come rich text
        int spriteIndex = 0;
        float fontSize = 10;
        string sprite = "";
        Transform indicatorTransform = Instantiate(AssetsManager.instance.damageIndicator, position, Quaternion.identity);
        DamageIndicator indicator = indicatorTransform.GetComponent<DamageIndicator>();
        if (elementEffectiveness.mFactor == 0)
        {
            spriteIndex = 9;
            fontSize = 6;
        }
        else if (elementEffectiveness.mFactor > 1)
        {
            spriteIndex = 2;
            fontSize = 12;
        }
        else if (elementEffectiveness.mFactor < 1)
        {
            spriteIndex = 10;
            fontSize = 8;
        }

        if(spriteIndex != 0)
        {
            sprite = $"<sprite index={spriteIndex} tint=1 color={elementEffectiveness.attackerColor} >";
        }
        indicator.textLabel.fontSize = fontSize;
        indicator.textLabel.SetText($"<color={elementEffectiveness.attackerColor}>{damage.ToString()}</color> {sprite}");
        indicator.textLabel.sortingOrder = sortingOrder;
        sortingOrder++;
        return indicator;

    }
}

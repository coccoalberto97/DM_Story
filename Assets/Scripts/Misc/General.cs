using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    UP, DOWN, LEFT, RIGHT, NONE
}


public class ElementInfo
{
    public DamageTypeEnum[] weaknesses;
    public DamageTypeEnum[] strengths;
    public DamageTypeEnum[] immunes;
    public string color;
}

public class ElementEffectiveness
{
    public float mFactor;
    public string attackerColor;
    public string receiverColor;
}

public class General
{

    public static Vector2 DirectionToVector(Direction d)
    {
        switch (d)
        {
            case Direction.UP:
                return Vector2.up;
            case Direction.DOWN:
                return Vector2.down;
            case Direction.LEFT:
                return Vector2.left;
            case Direction.RIGHT:
                return Vector2.right;
            default:
                return Vector2.zero;

        }
    }

    public static Dictionary<DamageTypeEnum, ElementInfo> elementChart = new Dictionary<DamageTypeEnum, ElementInfo>()
    {
        { DamageTypeEnum.FIRE , new ElementInfo {weaknesses = new DamageTypeEnum[] { DamageTypeEnum.ICE }, strengths = new DamageTypeEnum[] { DamageTypeEnum.MEME}, immunes = new DamageTypeEnum[]{ }, color = "#DB5461" } },
        { DamageTypeEnum.ICE , new ElementInfo {weaknesses = new DamageTypeEnum[] { DamageTypeEnum.MEME}, strengths = new DamageTypeEnum[] { DamageTypeEnum.FIRE},  immunes = new DamageTypeEnum[]{ }, color= "#7F95D1" } },
        { DamageTypeEnum.MEME , new ElementInfo {weaknesses = new DamageTypeEnum[] { DamageTypeEnum.FIRE}, strengths = new DamageTypeEnum[] { DamageTypeEnum.ICE},  immunes = new DamageTypeEnum[]{ }, color= "#3F826D" } },
        { DamageTypeEnum.DENIAL , new ElementInfo {weaknesses =  new DamageTypeEnum[]{ }, strengths =  new DamageTypeEnum[]{ }, immunes = new DamageTypeEnum[]{ DamageTypeEnum.DEFAULT}, color= "#2E2E3A"  } },
        { DamageTypeEnum.DEFAULT , new ElementInfo {weaknesses =  new DamageTypeEnum[]{ }, strengths =  new DamageTypeEnum[]{ },  immunes = new DamageTypeEnum[]{ }, color= "#FFFFFF"  } }
    };

    public static ElementEffectiveness GetElementEffectiveness(DamageTypeEnum attacker, DamageTypeEnum receiver)
    {
        float mFactor = 1;
        ElementInfo receiverInfo = elementChart[receiver];
        ElementInfo attackerInfo = elementChart[attacker];
        if (receiverInfo.weaknesses.Contains(attacker))
        {
            mFactor = 1.25f;
        }

        if (receiverInfo.strengths.Contains(attacker))
        {
            mFactor = 0.75f;
        }

        if (receiverInfo.immunes.Contains(attacker))
        {
            mFactor = 0;
        }
        return new ElementEffectiveness { mFactor = mFactor, attackerColor = attackerInfo.color, receiverColor = receiverInfo.color };
    }

    public static string GetHexColorBasedOnType(DamageTypeEnum type)
    {
        return elementChart[type].color;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHealth = 10;
    private int health;

    public void AddHealth(int m)
    {
        ModHealth(m);
    }

    public void SubtractHealth(int m)
    {
        ModHealth(-m);
    }

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    private void ModHealth(int m)
    {
        health += m;
        CheckHealth();

    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
        else if (health > maxHealth)
        {
            maxHealth = health;
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Die");
    }

    public int getHealth()
    {
        return health;
    }
}

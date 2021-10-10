using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHealth = 10;
    private int health;

    public void IncreaseMaxHealth(int m)
    {
        maxHealth += m;
        ModHealth(m);
    }

    public void AddHealth(int m)
    {
        int v = m;
        if (health + m > maxHealth)
        {
            v = maxHealth - health;
        }

        ModHealth(v);
    }

    public void restoreFullHealth()
    {
        AddHealth(maxHealth);
    }

    public void SubtractHealth(int m)
    {
        ModHealth(-m);
    }

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    protected virtual void ModHealth(int m)
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

    public void setHealth(int health)
    {
        if (health > 0)
        {
            if (health > this.maxHealth)
            {
                this.health = this.maxHealth;
            }
            else
            {
                this.health = health;
            }
        }
    }
}

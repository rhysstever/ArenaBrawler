using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public float currentHealth;
    public float maxHealth;
    public float movement;
    public float defense;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Removes health from the unit
    /// </summary>
    /// <param name="amount">The amount of health the unit is losing</param>
    public virtual void TakeDamage(float amount)
	{
        currentHealth -= amount;
    }

    /// <summary>
    /// Heals the unit's health
    /// </summary>
    /// <param name="amount">The amount the unit is healing</param>
    public virtual void Heal(float amount)
	{
        currentHealth += amount;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
	}
}

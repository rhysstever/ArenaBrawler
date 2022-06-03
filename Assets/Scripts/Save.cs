using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
	public string name;
	public List<ClassType> classLevels;
    public float currentHealth;
    public float maxHealth;
    public float healthRegen;
    public float movement;
    public float defense;
    public float damage;
    public float attackStaminaCost;
    public float currentStamina;
    public float maxStamina;
    public float staminaRegen;
    public int currentXP;
    public int currentGold;
}

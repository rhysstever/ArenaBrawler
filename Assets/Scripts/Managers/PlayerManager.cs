using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static PlayerManager instance = null;

    // Awake is called even before start 
    // (I think its at the very beginning of runtime)
    private void Awake()
    {
        // If the reference for this script is null, assign it this script
        if(instance == null)
            instance = this;
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        else if(instance != this)
            Destroy(gameObject);
    }
    #endregion
    
    public float health;
    public float healthRegen;
    public float movement;
    public float defense;
    public float damage;
    public float attackSpeed;
    public float stamina;
    public float staminaRegen;

    // Start is called before the first frame update
    void Start()
    {
        movement = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupStats(ClassStats classStats, bool isFirstLevel)
	{
        if(isFirstLevel)
        {
            health = classStats.baseHealth;
            healthRegen = classStats.baseHealthRegen;
            movement = classStats.baseMovement;
            defense = classStats.baseDefense;
            damage = classStats.baseDamage;
            attackSpeed = classStats.baseAttackSpeed;
            stamina = classStats.baseStamina;
            staminaRegen = classStats.baseStaminaRegen;
        }
        else
		{
            health *= 1 + classStats.levelMultiplierHealth;
            healthRegen *= 1 + classStats.levelMultiplierHealthRegen;
            movement *= 1 + classStats.levelMultiplierMovement;
            defense *= 1 + classStats.levelMultiplierDefense;
            damage *= 1 + classStats.levelMultiplierDamage;
            attackSpeed *= 1 + classStats.levelMultiplierAttackSpeed;
            stamina *= 1 + classStats.levelMultiplierStamina;
            staminaRegen *= 1 + classStats.levelMultiplierStamina;
        }
    }
}

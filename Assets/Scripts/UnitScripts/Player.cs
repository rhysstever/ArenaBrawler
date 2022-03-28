using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public float healthRegen;
    public float stamina;
    public float staminaRegen;

    public int currentXP;
    public int currentGold;

    private List<ClassType> levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = new List<ClassType>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Clears all of the stats of the player
    /// </summary>
    public void ClearStats()
	{
        unitName = "";
        health = 0;
        healthRegen = 0;
        movement = 0;
        defense = 0;
        damage = 0;
        attackSpeed = 0;
        stamina = 0;
        staminaRegen = 0;

        levels.Clear();
    }

    /// <summary>
    /// Sets the player's stats based on the leveled class
    /// </summary>
    /// <param name="classStats">The class the stats are being set from</param>
    public void SetStats(ClassStats classStats)
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

    /// <summary>
    /// Increases the player's stats based on the leveled class
    /// </summary>
    /// <param name="classStats">The class the player has leveled</param>
    public void IncreaseStats(ClassStats classStats)
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

    /// <summary>
    /// Gets the levels/classes of the character
    /// </summary>
    /// <returns>The list of classes of the current character</returns>
    public List<ClassType> GetLevels()
    {
        return levels;
    }

    /// <summary>
    /// Adds a level to the player
    /// </summary>
    /// <param name="levelUpClass">The class that the player is leveling into</param>
    public void LevelUp(ClassType levelUpClass)
    {
        // If the player has levels aleady,
        // Add the leveling stats of the class to the player
        // (the player already received base stats from their initial class)
        if(levels.Count > 0)
            IncreaseStats(LevelManager.instance.classStats[levelUpClass]);
        // If the player does not have a level in that class,
        // Add the base stats of the class to the player
        // (it is the first total level of the player)
        else
            SetStats(LevelManager.instance.classStats[levelUpClass]);

        // Adds that class to the player's levels, setting it at level 1
        levels.Add(levelUpClass);
    }

    /// <summary>
    /// Changes to game over menu when if the player loses all health
    /// </summary>
    public override void TakeDamage(float amount)
	{
        base.TakeDamage(amount);

        if(health <= 0.0f)
            GameManager.instance.ChangeMenuState(MenuState.GameOver);
	}

    /// <summary>
    /// Adds xp and gold to the player
    /// </summary>
    /// <param name="xpCollected">The amount of collected XP</param>
    /// <param name="goldCollected">The amount of collected gold</param>
    public void CollectResources(int xpCollected, int goldCollected)
	{
        currentXP += xpCollected;
        currentGold += goldCollected;
    }
}

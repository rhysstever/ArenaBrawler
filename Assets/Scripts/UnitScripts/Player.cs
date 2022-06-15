using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public float healthRegen;
    public float currentStamina;
    public float maxStamina;
    public float staminaRegen;
    public float attackStaminaCost;

    public int currentXP;
    public int currentGold;

    private List<ClassType> currentLevels;

    // Start is called before the first frame update
    void Start()
    {
        currentLevels = new List<ClassType>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.GetCurrentMenuState() == MenuState.Game
            && Input.GetKeyDown(KeyCode.E))
            CollectResources(50, 0);

        // Check if the player can level up
        // TODO: restrict this check to happen only after a wave is completed
        if(GameManager.instance.GetCurrentMenuState() == MenuState.Game
            && currentLevels.Count > 0
            && CanLevelUp())
            LevelUp(currentLevels[0]);
	}

    /// <summary>
    /// Clears all of the stats of the player
    /// </summary>
    public void ClearStats()
	{
        unitName = "";
        currentHealth = 0;
        maxHealth = 0;
        healthRegen = 0;
        movement = 0;
        defense = 0;
        damage = 0;
        attackStaminaCost = 0;
        currentStamina = 0;
        maxStamina = 0;
        staminaRegen = 0;

        currentXP = 0;
        currentGold = 0;

        currentLevels.Clear();
    }

    /// <summary>
    /// Sets the player's stats based on the leveled class
    /// </summary>
    /// <param name="classStats">The class the stats are being set from</param>
    public void SetStats(ClassStats classStats)
    {
        currentHealth = classStats.health;
        maxHealth = classStats.health;
        healthRegen = classStats.healthRegen;
        movement = classStats.movement;
        defense = classStats.defense;
        damage = classStats.damage;
        attackStaminaCost = classStats.attackStaminaCost;
        currentStamina = classStats.stamina;
        maxStamina = classStats.stamina;
        staminaRegen = classStats.staminaRegen;
    }

    /// <summary>
    /// Increases the player's stats based on the leveled class
    /// </summary>
    /// <param name="classType">The class the player has leveled</param>
    public void IncreaseStats(ClassType classType)
    {
        currentHealth       += LevelManager.instance.GetStatAmountGained("health", classType);
        maxHealth           += LevelManager.instance.GetStatAmountGained("health", classType);
        healthRegen         += LevelManager.instance.GetStatAmountGained("healthRegen", classType);
        movement            += LevelManager.instance.GetStatAmountGained("movement", classType);
        defense             += LevelManager.instance.GetStatAmountGained("defense", classType);
        damage              += LevelManager.instance.GetStatAmountGained("damage", classType);
        attackStaminaCost   += LevelManager.instance.GetStatAmountGained("attackStaminaCost", classType);
        currentStamina      += LevelManager.instance.GetStatAmountGained("stamina", classType);
        maxStamina          += LevelManager.instance.GetStatAmountGained("stamina", classType);
        staminaRegen        += LevelManager.instance.GetStatAmountGained("staminaRegen", classType);
    }

    /// <summary>
    /// Sets player stat values from a Save object
    /// </summary>
    /// <param name="savedStats">The save of the player stats</param>
    public void LoadStats(Save savedStats)
	{
        unitName = savedStats.name;
        currentLevels = savedStats.classLevels;
        currentHealth = savedStats.currentHealth;
        maxHealth = savedStats.maxHealth;
        healthRegen = savedStats.healthRegen;
        movement = savedStats.movement;
        defense = savedStats.defense;
        damage = savedStats.damage;
        attackStaminaCost = savedStats.attackStaminaCost;
        currentStamina = savedStats.currentStamina;
        maxStamina = savedStats.maxStamina;
        staminaRegen = savedStats.staminaRegen;
        currentXP = savedStats.currentXP;
        currentGold = savedStats.currentGold;
	}

    /// <summary>
    /// Gets the levels/classes of the character in a simple list format
    /// </summary>
    /// <returns>The list of the order of classes of the current character</returns>
    public List<ClassType> GetLevelsList() { return currentLevels; }

    /// <summary>
    /// Get the number of levels the player has
    /// </summary>
    /// <returns>The number of levels</returns>
    public int GetsLevelsCount() { return currentLevels.Count; }

    /// <summary>
    /// Gets the levels of the character in a dictionary format, broken down by class
    /// </summary>
    /// <returns>A breakdown of how many levels of each class the character has</returns>
    public Dictionary<ClassType, int> GetLevelsBreakdown()
	{
        Dictionary<ClassType, int> levels = new Dictionary<ClassType, int>();
        foreach(ClassType type in currentLevels)
        {
            if(levels.ContainsKey(type))
                levels[type]++;
            else
                levels.Add(type, 1);
        }
        return levels;
    }

    /// <summary>
    /// Adds a level to the player
    /// </summary>
    /// <param name="levelUpClass">The class that the player is leveling into</param>
    public void LevelUp(ClassType levelUpClass)
    {
        // Removes the amount of XP needed to level
        CollectResources(-LevelManager.instance.XPToLevel(currentLevels.Count), 0);

        // If the player has levels aleady,
        // Add the leveling stats of the class to the player
        // (the player already received base stats from their initial class)
        if(currentLevels.Count > 0)
            IncreaseStats(levelUpClass);
        // If the player does not have a level in that class,
        // Add the base stats of the class to the player
        // (it is the first total level of the player)
        else
            SetStats(LevelManager.instance.classStats[levelUpClass]);

        // Adds that class to the player's levels, setting it at level 1
        currentLevels.Add(levelUpClass);
    }

    /// <summary>
    /// Changes to game over menu when if the player loses all health
    /// </summary>
    public override void TakeDamage(float amount)
	{
        base.TakeDamage(amount);

        if(currentHealth <= 0.0f)
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
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

    /// <summary>
    /// Checks if the player can level up
    /// </summary>
    /// <returns>Whether the player has enough XP to level up, based on their current level</returns>
    private bool CanLevelUp()
    {
        int currentLevel = currentLevels.Count;
        return currentXP >= LevelManager.instance.XPToLevel(currentLevel);
    }
}

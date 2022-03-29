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

    private List<ClassType> currentLevels;

    // Start is called before the first frame update
    void Start()
    {
        currentLevels = new List<ClassType>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            CollectResources(1000, 100);

        if(CanLevelUp())
        {
            if(Input.GetKeyDown(KeyCode.G))
                LevelUp(ClassType.Gladiator);
            else if(Input.GetKeyDown(KeyCode.B))
                LevelUp(ClassType.Brawler);
		}
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

        currentLevels.Clear();
    }

    /// <summary>
    /// Sets the player's stats based on the leveled class
    /// </summary>
    /// <param name="classStats">The class the stats are being set from</param>
    public void SetStats(ClassStats classStats)
    {
        health = classStats.health.Item1;
        healthRegen = classStats.healthRegen.Item1;
        movement = classStats.movement.Item1;
        defense = classStats.defense.Item1;
        damage = classStats.damage.Item1;
        attackSpeed = classStats.attackSpeed.Item1;
        stamina = classStats.stamina.Item1;
        staminaRegen = classStats.staminaRegen.Item1;
    }

    /// <summary>
    /// Increases the player's stats based on the leveled class
    /// </summary>
    /// <param name="classStats">The class the player has leveled</param>
    public void IncreaseStats(ClassStats classStats)
    {
        health *= 1 + classStats.health.Item2;
        healthRegen *= 1 + classStats.healthRegen.Item2;
        movement *= 1 + classStats.movement.Item2;
        defense *= 1 + classStats.defense.Item2;
        damage *= 1 + classStats.damage.Item2;
        attackSpeed *= 1 + classStats.attackSpeed.Item2;
        stamina *= 1 + classStats.stamina.Item2;
        staminaRegen *= 1 + classStats.staminaRegen.Item2;
    }

    /// <summary>
    /// Gets the levels/classes of the character
    /// </summary>
    /// <returns>The list of classes of the current character</returns>
    public List<ClassType> GetLevels()
    {
        return currentLevels;
    }

    /// <summary>
    /// Adds a level to the player
    /// </summary>
    /// <param name="levelUpClass">The class that the player is leveling into</param>
    public void LevelUp(ClassType levelUpClass)
    {
        // Removes the amount of XP needed to level
        // CollectResources(-LevelManager.instance.XPToLevel(currentLevels.Count + 1), 0); // TODO: refine XP vs levling (maybe keep track of total xp instead of removing it every levelup)

        // If the player has levels aleady,
        // Add the leveling stats of the class to the player
        // (the player already received base stats from their initial class)
        if(currentLevels.Count > 0)
            IncreaseStats(LevelManager.instance.classStats[levelUpClass]);
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

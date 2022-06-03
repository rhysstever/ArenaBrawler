using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum ClassType
{
    Gladiator,
    Brawler
}

public class LevelManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static LevelManager instance = null;

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

    public GameObject player;
    public Dictionary<ClassType, ClassStats> classStats;

    private List<int> xpAmountsToLevel;

    // Start is called before the first frame update
    void Start()
    {
        FillClassStatsDictionary();
        FillXPAmountsToLevelList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates and pairs a stats struct to a levelable class
    /// </summary>
    private void FillClassStatsDictionary()
	{
        classStats = new Dictionary<ClassType, ClassStats>();

        // Create a struct for each levelable rpg class
        // Each stat has is a tuple
        // Item1 = the base value a character receives if they take that class at the character's first level
        // Item2 = the % that value increases when they take a level in that class after the character's first level (% between 0.00 and 1.00)
        ClassStats gladiatorStats = new ClassStats()
        {
            // Gladiator, slow but strong attacker
            // Can take more hits but can't run far due to heavy armor
            health = (25.0f, 0.3f),
            healthRegen = (0.4f, 0.2f),
            movement = (2.0f, 0.1f),
            defense = (5.0f, 0.3f),
            damage = (2.0f, 0.25f),
            attackStaminaCost = (2.0f, 0.1f),
            stamina = (10.0f, 0.15f),
            staminaRegen = (0.5f, 0.1f)
        };

        ClassStats brawlerStats = new ClassStats()
        {
            // Brawler, quick but light hitter
            // Bobs and weaves in and out of range
            // Can run away from enemies easily but dies quickly if caught
            health = (15.0f, 0.1f),
            healthRegen = (0.4f, 0.1f),
            movement = (2.5f, 0.3f),
            defense = (2.0f, 0.1f),
            damage = (1.0f, 0.15f),
            attackStaminaCost = (1.0f, 0.2f),
            stamina = (10.0f, 0.3f),
            staminaRegen = (0.5f, 0.4f)
        };

        // Add each struct to the dictionary
        classStats.Add(ClassType.Gladiator, gladiatorStats);
        classStats.Add(ClassType.Brawler, brawlerStats);
    }

    /// <summary>
    /// Creates a list that holds how much xp a character needs to level each level
    /// </summary>
    private void FillXPAmountsToLevelList()
	{
        xpAmountsToLevel = new List<int>();

        xpAmountsToLevel.Add(0); // Level 1 : no XP required
        xpAmountsToLevel.Add(100); 
        xpAmountsToLevel.Add(150);
        xpAmountsToLevel.Add(200);
        xpAmountsToLevel.Add(250); // Level 5
        xpAmountsToLevel.Add(300);
        xpAmountsToLevel.Add(350);
        xpAmountsToLevel.Add(400);
        xpAmountsToLevel.Add(450);
        xpAmountsToLevel.Add(500); // Level 10 : "max" level (for now)
    }

    /// <summary>
    /// Gets how much XP is needed to get to that level
    /// </summary>
    /// <param name="level">The goal level</param>
    /// <returns>How much XP is needed to get to that level from the previous level</returns>
    public int XPToLevel(int level)
	{
        return xpAmountsToLevel[level]; 
	}

    /// <summary>
    /// Gets the total XP the character needs from their current level to reach the goal level
    /// </summary>
    /// <param name="player">The </param>
    /// <param name="goalLevel">The level the player wants to reach</param>
    /// <returns>The amount of total XP the player needs from their current level to reach the goal level</returns>
    public int XPToLevel(GameObject character, int goalLevel)
	{
        int startingLevel = character.GetComponent<Player>().GetLevelsList().Count;
        // Make sure the player is not already at or past the goal level
        if(startingLevel >= goalLevel)
            return 0;

        // Subtracts the player's current xp from the total amount needed
        int currentXP = character.GetComponent<Player>().currentXP;
        int totalXPNeeded = -currentXP;
        // Add each level's XP amount beyond the starting level until the goal level is reached
        for(int i = startingLevel + 1; i <= goalLevel; i++)
            totalXPNeeded += XPToLevel(i);

        return totalXPNeeded;
	}
}

public struct ClassStats
{
    // For each stat
    // Item1: base value
    // Item2: level modifier value
    public (float, float) health;
    public (float, float) healthRegen;
    public (float, float) movement;
    public (float, float) defense;
    public (float, float) damage;
    public (float, float) attackStaminaCost;
    public (float, float) stamina;
    public (float, float) staminaRegen;
}

using System.Collections;
using System.Collections.Generic;
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

    public string characterName;

    private List<ClassType> levels;
    private Dictionary<ClassType, ClassStats> classStats;

    // Start is called before the first frame update
    void Start()
    {
        levels = new List<ClassType>();
        FillClassStatsDictionary();
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
        ClassStats gladiatorStats = new ClassStats()
        {
            baseHealth = 0,
            baseHealthRegen = 0,
            baseMovement = 0,
            baseDefense = 0,
            baseDamage = 0,
            baseAttackSpeed = 0,
            baseStamina = 0,
            baseStaminaRegen = 0,
            levelMultiplierHealth = 0,
            levelMultiplierHealthRegen = 0,
            levelMultiplierMovement = 0,
            levelMultiplierDefense = 0,
            levelMultiplierDamage = 0,
            levelMultiplierAttackSpeed = 0,
            levelMultiplierStamina = 0,
            levelMultiplierStaminaRegen = 0
        };
        ClassStats brawlerStats = new ClassStats()
        {
            baseHealth = 0,
            baseHealthRegen = 0,
            baseMovement = 0,
            baseDefense = 0,
            baseDamage = 0,
            baseAttackSpeed = 0,
            baseStamina = 0,
            baseStaminaRegen = 0,
            levelMultiplierHealth = 0,
            levelMultiplierHealthRegen = 0,
            levelMultiplierMovement = 0,
            levelMultiplierDefense = 0,
            levelMultiplierDamage = 0,
            levelMultiplierAttackSpeed = 0,
            levelMultiplierStamina = 0,
            levelMultiplierStaminaRegen = 0
        };

        // Add each struct to the dictionary
        classStats.Add(ClassType.Gladiator, gladiatorStats);
        classStats.Add(ClassType.Brawler, brawlerStats);
    }

    /// <summary>
    /// Adds a level to the player
    /// </summary>
    /// <param name="levelUpClass">The class that the player is leveling into</param>
    public void LevelUp(ClassType levelUpClass)
	{
        // If the player has levels aleady
		if(levels.Count > 0)
		{
            // Add the leveling stats of the class to the player
            // (the player already received base stats from their initial class)

            // TODO: Get player script and add each leveling modifier stat
            // Ex: player.statName *= 1 + classStats[levelUpClass].levelMultiplierStatName; <-- do this for each stat (replacing "statName")
        }
        // If the player does not have a level in that class
        else
		{
            // Add the base stats of the class to the player
            // (it is the first total level of the player)

            // TODO: Get player script and add each base stat
            // Ex: player.statName = classStats[levelUpClass].baseStatName; <-- do this for each stat (replacing "statName")
		}
        
        // Adds that class to the player's levels, setting it at level 1
        levels.Add(levelUpClass);
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
    /// Clears the levels list
    /// </summary>
    public void ResetLevels()
	{
        levels.Clear();
    }
}

public struct ClassStats
{
    public int baseHealth;
    public int baseHealthRegen;
    public int baseMovement;
    public int baseDefense;
    public int baseDamage;
    public int baseAttackSpeed;
    public int baseStamina;
    public int baseStaminaRegen;
    public float levelMultiplierHealth;
    public float levelMultiplierHealthRegen;
    public float levelMultiplierMovement;
    public float levelMultiplierDefense;
    public float levelMultiplierDamage;
    public float levelMultiplierAttackSpeed;
    public float levelMultiplierStamina;
    public float levelMultiplierStaminaRegen;
}

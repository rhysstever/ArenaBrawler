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

    public GameObject player;
    public Dictionary<ClassType, ClassStats> classStats;

    // Start is called before the first frame update
    void Start()
    {
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
            baseHealth = 20,
            baseHealthRegen = 1,
            baseMovement = 5,
            baseDefense = 5,
            baseDamage = 2,
            baseAttackSpeed = 3,
            baseStamina = 10,
            baseStaminaRegen = 1,
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
            baseHealth = 10,
            baseHealthRegen = 2,
            baseMovement = 7,
            baseDefense = 2,
            baseDamage = 1,
            baseAttackSpeed = 6,
            baseStamina = 20,
            baseStaminaRegen = 2,
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

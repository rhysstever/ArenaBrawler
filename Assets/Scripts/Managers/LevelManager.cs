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
            health = (20.0f, 0),
            healthRegen = (1.0f, 0),
            movement = (2.5f, 0),
            defense = (5.0f, 0),
            damage = (2.0f, 0),
            attackSpeed = (3.0f, 0),
            stamina = (10.0f, 0),
            staminaRegen = (1.0f, 0)
        };
        ClassStats brawlerStats = new ClassStats()
        {
            health = (10.0f, 0),
            healthRegen = (2.0f, 0),
            movement = (4.0f, 0),
            defense = (2.0f, 0),
            damage = (1.0f, 0),
            attackSpeed = (6.0f, 0),
            stamina = (20.0f, 0),
            staminaRegen = (2.0f, 0)
        };

        // Add each struct to the dictionary
        classStats.Add(ClassType.Gladiator, gladiatorStats);
        classStats.Add(ClassType.Brawler, brawlerStats);
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
    public (float, float) attackSpeed;
    public (float, float) stamina;
    public (float, float) staminaRegen;
}

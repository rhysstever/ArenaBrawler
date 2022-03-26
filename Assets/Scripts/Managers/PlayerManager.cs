using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassType
{
    Gladiator, 
    Brawler
}

public class PlayerManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static PlayerManager instance = null;

    // Awake is called even before start 
    // (I think its at the very beginning of runtime)
    private void Awake()
    {
        // If the reference for this script is null, assign it this script.
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }
    #endregion

    public GameObject player;
    public Dictionary<ClassType, CharacterClass> classBaseStats;

    // Start is called before the first frame update
    void Start()
    {
        SetBaseClassStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Prep the base stats of each class
    /// </summary>
    private void SetBaseClassStats()
	{
        classBaseStats = new Dictionary<ClassType, CharacterClass>();

        // Test values
        CharacterClass gladiator = new CharacterClass(10, 1, 2, 3, 4, 5, 6, 7);
        CharacterClass brawler = new CharacterClass(10, 1, 2, 3, 4, 5, 6, 7);

        classBaseStats.Add(ClassType.Gladiator, gladiator);
        classBaseStats.Add(ClassType.Brawler, brawler);
    }

    /// <summary>
    /// Sets the class of the player
    /// </summary>
    /// <param name="classType">The new class of the player</param>
    public void SetClass(ClassType classType)
	{
        player.GetComponent<PlayerInfo>().characterClass = classBaseStats[classType];
	}
}

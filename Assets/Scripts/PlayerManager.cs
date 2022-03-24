using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassType
{
    Swordsperson, 
    Brawler
}

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public Dictionary<ClassType, CharacterClass> characterClasses;

    // Start is called before the first frame update
    void Start()
    {
        SetClassDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetClassDictionary()
	{
        characterClasses = new Dictionary<ClassType, CharacterClass>();

        // Test values
        CharacterClass swordsperson = new CharacterClass(10, 1, 2, 3, 4, 5, 6, 7);
        CharacterClass brawler = new CharacterClass(10, 1, 2, 3, 4, 5, 6, 7);

        characterClasses.Add(ClassType.Swordsperson, swordsperson);
        characterClasses.Add(ClassType.Brawler, brawler);
    }

    /// <summary>
    /// Sets the class of the player
    /// </summary>
    /// <param name="classType">The new class of the player</param>
    public void SetClass(ClassType classType)
	{
        player.GetComponent<PlayerInfo>().characterClass = characterClasses[classType];
	}
}

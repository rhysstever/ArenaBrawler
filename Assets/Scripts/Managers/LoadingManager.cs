using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static LoadingManager instance = null;

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

    private Save newSave;
    private string localSavePath;

    // Start is called before the first frame update
    void Start()
    {
        localSavePath = "Assets/Resources/SavedCharacters/";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a save file for the current character
    /// </summary>
    public void CreateSave()
	{
        // Create Save object
        newSave = new Save();

        // Get the player class 
        Player player = LevelManager.instance.player.GetComponent<Player>();
        // Set values of data that will be saved
        newSave.name = player.unitName;
        newSave.classLevels = player.GetLevelsList();
        newSave.currentHealth = player.currentHealth;
        newSave.maxHealth = player.maxHealth;
        newSave.healthRegen = player.healthRegen;
        newSave.movement = player.movement;
        newSave.defense = player.defense;
        newSave.damage = player.damage;
        newSave.attackStaminaCost = player.attackStaminaCost;
        newSave.currentStamina = player.currentStamina;
        newSave.maxStamina = player.maxStamina;
        newSave.staminaRegen = player.staminaRegen;
        newSave.currentXP = player.currentXP;
        newSave.currentGold = player.currentGold;

        // Turn the Save object into a json string and save it
        string savedDataStr = JsonConvert.SerializeObject(newSave);
        System.IO.File.WriteAllText(localSavePath + player.unitName + ".json", savedDataStr);
    }

    /// <summary>
    /// Gets and sets a saved character's data
    /// </summary>
    /// <param name="characterName">The name of the saved character</param>
    public void LoadSavedCharacter(string characterName)
	{
        // Check that a save file exists for the character
        if(!System.IO.File.Exists(localSavePath + characterName + ".json"))
            return;    // End early if there is no save file

        // Find and read the saved json file 
        string loadedDataStr = System.IO.File.ReadAllText(localSavePath + characterName + ".json");

        // Convert the json string into a Save object 
        Save loadedSave = JsonConvert.DeserializeObject<Save>(loadedDataStr);

        // Pass the save to the player
        LevelManager.instance.player.GetComponent<Player>().LoadStats(loadedSave);
    }

    /// <summary>
    /// A getter helper method for the local save path
    /// </summary>
    /// <returns>The local save path</returns>
    public string GetLocalSavePath() { return localSavePath; }
}

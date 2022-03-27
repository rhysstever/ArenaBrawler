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

        // Set values of data that will be saved
        newSave.name = LevelManager.instance.characterName;
        newSave.classLevels = LevelManager.instance.GetLevels();

        // Turn the Save object into a json string and save it
        string savedDataStr = JsonConvert.SerializeObject(newSave);
        System.IO.File.WriteAllText(localSavePath + LevelManager.instance.characterName + ".json", savedDataStr);
    }

    /// <summary>
    /// Gets and sets a saved character's data
    /// </summary>
    /// <param name="characterName">The name of the saved character</param>
    public void LoadSavedCharacter(string characterName)
	{
        SetLoadedData(LoadSave(characterName));
	}

    /// <summary>
    /// Loads a character's saved data
    /// </summary>
    /// <param name="characterName">The name of the character</param>
    /// <returns>The save data of that character</returns>
	private Save LoadSave(string characterName)
	{
        // Check that a save file exists for the character
        if(!System.IO.File.Exists(localSavePath + characterName + ".json"))
            return null;    // End early if there is no save file

        // Find and read the saved json file 
        string loadedDataStr = System.IO.File.ReadAllText(localSavePath + characterName + ".json");

        // Convert the json string into a Save object 
        return JsonConvert.DeserializeObject<Save>(loadedDataStr);
    }

    /// <summary>
    /// Converts the Save object to game values
    /// </summary>
    /// <param name="loadedSave">The Save object of the loaded save file</param>
    private void SetLoadedData(Save loadedSave)
	{
        // Set saved data as game values
        LevelManager.instance.characterName = loadedSave.name;
        // Reset the current character
        LevelManager.instance.ResetLevels();
        // Level the character in order of the saved classes
        foreach(ClassType level in loadedSave.classLevels)
            LevelManager.instance.LevelUp(level);
    }

    /// <summary>
    /// A getter helper method for the local save path
    /// </summary>
    /// <returns>The local save path</returns>
    public string GetLocalSavePath()
	{
        return localSavePath;
	}
}

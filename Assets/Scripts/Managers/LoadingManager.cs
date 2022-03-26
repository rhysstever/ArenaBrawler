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
    private string localSaveFolderPath;

    // Start is called before the first frame update
    void Start()
    {
        localSaveFolderPath = "Assets/Resources/SaveData/";
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

        List<ClassType> levels = LevelManager.instance.GetLevels();

        string levelsStr = "";
        foreach (ClassType level in levels)
            levelsStr += level + ", ";

        newSave.levels = levelsStr;

        // Turn the Save object into a json string and save it
        string savedDataStr = JsonUtility.ToJson(newSave);
        System.IO.File.WriteAllText(localSaveFolderPath + LevelManager.instance.characterName + ".json", savedDataStr);
    }

    /// <summary>
    /// Loads a character's saved data
    /// </summary>
    /// <param name="characterName">The name of the character</param>
    /// <returns>The save data of that character</returns>
	public Save LoadSave(string characterName)
	{
        // Check that a save file exists for the character
        if(!System.IO.File.Exists(localSaveFolderPath + characterName + ".json"))
            return null;    // End early if there is no save file

        // Find and read the saved json file 
        string loadedDataStr = System.IO.File.ReadAllText(localSaveFolderPath + characterName + ".json");

        // Convert the json string into a Save object 
        return JsonUtility.FromJson<Save>(loadedDataStr);
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
        string[] levelBreakdown = loadedSave.levels.Split(',');
        foreach(string level in levelBreakdown)
		{
            if(Enum.TryParse(level, out ClassType classLevel))
                LevelManager.instance.LevelUp(classLevel);
		}
    }
}

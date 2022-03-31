using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static UIManager instance = null;

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

    [SerializeField]
    private Canvas canvas;

    [SerializeField]    // Empty parent gameObjects
    private GameObject mainMenuParent, selectParent, createCharacterParent, gameParent, pauseParent, gameOverParent;

    [SerializeField]    // Main Menu Buttons
    private GameObject playButton, quitButton;

    [SerializeField]    // Select Buttons
    private GameObject savedCharacterButtonPrefab, selectCharacterButton, newCharacterButton, deleteSaveButton, selectToMainMenuButton;

    [SerializeField]    // Select empty parent gameObjects
    private GameObject savedCharacterButtonsParent;

    [SerializeField]    // Create Character Buttons
    private GameObject createCharacterButton, createCharacterToSelectButton;

    [SerializeField]    // Create Character Inputs
    private GameObject characterNameTextInput, characterClassTogglesParent;

    [SerializeField]    // Game Text
    private GameObject characterNameText, classText;

    [SerializeField]    // Pause Buttons
    private GameObject resumeButton, saveButton, pauseToMainMenuButton;

    [SerializeField]    // Game Over Buttons
    private GameObject gameOverToMainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        SetupUpUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Setups up button onClicks
    /// </summary>
    private void SetupUpUI()
	{
        // Main Menu buttons
        playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Select));
        quitButton.GetComponent<Button>().onClick.AddListener(() => Application.Quit());
        // Select buttons        
        newCharacterButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.CharacterCreate));
        selectCharacterButton.GetComponent<Button>().onClick.AddListener(() => CheckForSelectedCharacter());
        deleteSaveButton.GetComponent<Button>().onClick.AddListener(() => DeleteSelectedSave());
        selectToMainMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
        // Character Create toggles
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            toggleTransform.GetComponent<Toggle>().onValueChanged.AddListener((bool value) => ToggleValueChange(toggleTransform.gameObject, value));
        // Character Create buttons
        createCharacterButton.GetComponent<Button>().onClick.AddListener(() => CreateNewCharacterButtonClicked());
        createCharacterToSelectButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Select));
        // Pause buttons
        resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        saveButton.GetComponent<Button>().onClick.AddListener(() => LoadingManager.instance.CreateSave());
        pauseToMainMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
        // Game Over buttons
        gameOverToMainMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
    }

    /// <summary>
    /// Runs constant logic based on the current menu state 
    /// </summary>
    private void UpdateUI()
	{
        switch(GameManager.instance.GetCurrentMenuState())
        {
            case MenuState.MainMenu:
                break;
            case MenuState.Select:
                break;
            case MenuState.CharacterCreate:
                break;
            case MenuState.Game:
                // When in the game, key controls: 
                // Esc:     Opens pause menu
                // Tab:     Ends the game
                if(Input.GetKeyDown(KeyCode.Escape))
                    GameManager.instance.ChangeMenuState(MenuState.Pause);
                else if(Input.GetKeyDown(KeyCode.Tab))
                    GameManager.instance.ChangeMenuState(MenuState.GameOver);
                break;
            case MenuState.Pause:
                // When game is paused, key controls:
                // Esc:     Resumes the game
                if(Input.GetKeyDown(KeyCode.Escape))
                    GameManager.instance.ChangeMenuState(MenuState.Game);
                break;
            case MenuState.GameOver:
                break;
        }
    }

    /// <summary>
    /// Updates the menu state UI
    /// </summary>
    /// <param name="menuState">The new menu state</param>
    public void UpdateMenuStateUI(MenuState menuState)
    {
        // Deactivate all empty gameObject parents
        foreach(Transform childTrans in canvas.transform)
            childTrans.gameObject.SetActive(false);

        // Activate the right empty parent gameObject
        switch(menuState)
        {
            case MenuState.MainMenu:
                mainMenuParent.SetActive(true);
                break;
            case MenuState.Select:
                selectParent.SetActive(true);
                CreateSavedCharacterButtons();
                break;
            case MenuState.CharacterCreate:
                createCharacterParent.SetActive(true);
                // Clear player stats and input fields for creating a new character
                LevelManager.instance.player.GetComponent<Player>().ClearStats();
                ClearCreateCharacterInput();
                break;
            case MenuState.Game:
                gameParent.SetActive(true);
                // Update the text of the character
                characterNameText.GetComponent<TMP_Text>().text = "Name: " + LevelManager.instance.player.GetComponent<Player>().unitName;
                UpdatePlayerLevelText();
                break;
            case MenuState.Pause:
                pauseParent.SetActive(true);
                break;
            case MenuState.GameOver:
                gameOverParent.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Creates a button for each saved character
    /// </summary>
    private void CreateSavedCharacterButtons()
	{
        ClearSavedCharacterButtons();

        // Get an array of saved character files
        DirectoryInfo dataFolder = new DirectoryInfo(LoadingManager.instance.GetLocalSavePath());
        FileInfo[] dataFiles = dataFolder.GetFiles("*.json");

        // Initial and the change in y-value for each button created
        float yPos = 50.0f;
        float deltaY = -50.0f;

        GameObject firstSave = null;

        // Loop through each saved file
        foreach(FileInfo file in dataFiles)
		{
            // Create the actual button
            GameObject savedCharacterButton = Instantiate(savedCharacterButtonPrefab, savedCharacterButtonsParent.transform);

            // Set the first save if it is null
            if(firstSave == null) firstSave = savedCharacterButton;

            yPos += deltaY;
            savedCharacterButton.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
            // Get the name of the character
            string trimmedName = Path.ChangeExtension(file.Name, null);
            savedCharacterButton.name = trimmedName;
            // Set the text of the button
            savedCharacterButton.transform.GetChild(0).GetComponent<TMP_Text>().text = trimmedName;   
            // Set the onClick
            savedCharacterButton.transform.GetComponent<Button>().onClick.AddListener(
                () => LoadSavedCharacterButton(savedCharacterButton));
        }

		// Select the first button (if there is one)
		if(firstSave != null)
            firstSave.GetComponent<Button>().onClick.Invoke();
	} 

    /// <summary>
    /// Loads a character's stats when it is selected
    /// </summary>
    /// <param name="selectedCharacterButton">The name of the character being selected</param>
    private void LoadSavedCharacterButton(GameObject selectedCharacterButton)
	{
        // Make the button background green
        selectedCharacterButton.transform.GetComponent<Image>().color = new Color(0.0f, 200.0f, 0.0f);
        // Make every other button background white
        foreach(Transform savedCharacterButtonParentTransform in savedCharacterButtonsParent.transform)
		{
            if(savedCharacterButtonParentTransform.gameObject != selectedCharacterButton)
                savedCharacterButtonParentTransform.GetComponent<Image>().color = Color.white;
        }

        LoadingManager.instance.LoadSavedCharacter(selectedCharacterButton.name);
    }

    /// <summary>
    /// Clears all load character buttons
    /// </summary>
    private void ClearSavedCharacterButtons()
	{
        List<GameObject> characterButtonsToBeDestroyed = new List<GameObject>();

        // Delete any children buttons in the parent
        foreach(Transform savedCharacterButtonTrans in savedCharacterButtonsParent.transform)
            characterButtonsToBeDestroyed.Add(savedCharacterButtonTrans.gameObject);

        // Destroy() is too slow (the new buttons are being created immediately after
        for(int i = characterButtonsToBeDestroyed.Count - 1; i >= 0; i--)
            DestroyImmediate(characterButtonsToBeDestroyed[i]);

        // Clear player stats
        LevelManager.instance.player.GetComponent<Player>().ClearStats();
    }

    /// <summary>
    /// Performs logic for when a toggle is clicked (for either toggle on or off)
    /// </summary>
    /// <param name="toggleClicked">The toggle game object that was toggled</param>
    /// <param name="toggleValue">The value the toggle is currently</param>
    private void ToggleValueChange(GameObject toggleClicked, bool toggleValue)
    {
        // Set the toggle based on the newValue
        toggleClicked.GetComponent<Toggle>().isOn = toggleValue;

        // If the toggle is now on, it loops through all other toggles and detoggles them
        if(toggleValue)
            foreach(Transform toggleTransform in characterClassTogglesParent.transform)
                if(toggleTransform.gameObject != toggleClicked)
                    toggleTransform.GetComponent<Toggle>().isOn = false;
    }

    /// <summary>
    /// Gets the inputted information to created a new character
    /// </summary>
    private void CreateNewCharacterButtonClicked()
	{
        if(!HasEnteredEnoughInput())
		{
            Debug.Log("You must enter a character name and select a class!");
            return;
		}

        // Set the character name
        LevelManager.instance.player.GetComponent<Player>().unitName = characterNameTextInput.GetComponent<TMP_InputField>().text;

        // Level up the character based on the class they selected
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            if(toggleTransform.GetComponent<Toggle>().isOn)
                if(Enum.TryParse(toggleTransform.gameObject.name.Substring(11), out ClassType classLevel))
                    LevelManager.instance.player.GetComponent<Player>().LevelUp(classLevel);

        // Set the menu state to game
        GameManager.instance.ChangeMenuState(MenuState.Game);
    }

    /// <summary>
    /// Checks if the player has entered enough input to create a character
    /// </summary>
    /// <returns>Whether there is enough input entered</returns>
    private bool HasEnteredEnoughInput()
	{
        // If there is no name entered, return false
        if(characterNameTextInput.GetComponent<TMP_InputField>().text == "")
            return false;

        // If a class is selected, return true
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            if(toggleTransform.GetComponent<Toggle>().isOn)
                return true;

        // If no class is selected, return false
        return false;
    }

    /// <summary>
    /// Deletes a saved character
    /// </summary>
    private void DeleteSelectedSave()
	{
        string filePath = LoadingManager.instance.GetLocalSavePath() + LevelManager.instance.player.GetComponent<Player>().unitName + ".json";
        // Delete the file of the character linked to that button
        File.Delete(filePath);
        // Delete the .meta file too
        File.Delete(filePath + ".meta");

        // Recreate the list of saved character buttons
        CreateSavedCharacterButtons();
    }

    /// <summary>
    /// Checks to make sure a character is selected before loading into the game
    /// </summary>
    private void CheckForSelectedCharacter()
	{
        if(LevelManager.instance.player.GetComponent<Player>().unitName == "")
            Debug.Log("You need to select a character");
        else 
            GameManager.instance.ChangeMenuState(MenuState.Game);
    }

    /// <summary>
    /// Clears all inputs in the character creation menu
    /// </summary>
    private void ClearCreateCharacterInput()
	{
        // Clear text input
        characterNameTextInput.GetComponent<TMP_InputField>().text = "";
        // Turn off all class toggles
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            toggleTransform.GetComponent<Toggle>().isOn = false;
    }

    /// <summary>
    /// Displays the class breakdown of the player
    /// </summary>
    private void UpdatePlayerLevelText()
	{
        Dictionary<ClassType, int> classes = LevelManager.instance.player.GetComponent<Player>().GetLevelsBreakdown();
        string classStr = "Level " + LevelManager.instance.player.GetComponent<Player>().GetLevelsList().Count + ": ";
        foreach(ClassType type in classes.Keys)
            classStr += type + " " + classes[type] + " ";

        classText.GetComponent<TMP_Text>().text = classStr;
	}
}

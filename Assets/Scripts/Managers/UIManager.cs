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
    private GameObject savedCharacterButtonPrefab, selectCharacterButton, newCharacterButton;

    [SerializeField]    // Select empty parent gameObjects
    private GameObject savedCharacterButtonsParent;

    [SerializeField]    // Create Character Buttons
    private GameObject createCharacterButton;

    [SerializeField]    // Create Character Inputs
    private GameObject characterNameTextInput, characterClassTogglesParent;

    [SerializeField]    // Game Text
    private GameObject characterName;

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
        if(GameManager.instance.GetCurrentMenuState() == MenuState.Game)
		{
            // When in the game, controls: 
            // Esc:     Opens pause menu
            // Tab:     Ends the game
            if(Input.GetKeyDown(KeyCode.Escape))
                GameManager.instance.ChangeMenuState(MenuState.Pause);
            else if(Input.GetKeyDown(KeyCode.Tab))
                GameManager.instance.ChangeMenuState(MenuState.GameOver);
        }
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
        selectCharacterButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        newCharacterButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.CharacterCreate));
        // Character Create toggles
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            toggleTransform.GetComponent<Toggle>().onValueChanged.AddListener((bool value) => ToggleValueChange(toggleTransform.gameObject, value));
        // Character Create buttons
        createCharacterButton.GetComponent<Button>().onClick.AddListener(() => CreateNewCharacterButtonClicked());
        // Pause buttons
        resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        saveButton.GetComponent<Button>().onClick.AddListener(() => LoadingManager.instance.CreateSave());
        pauseToMainMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
        // Game Over buttons
        gameOverToMainMenuButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
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
                CreateSavedCharacterButtons();
                selectParent.SetActive(true);
                break;
            case MenuState.CharacterCreate:
                createCharacterParent.SetActive(true);
                break;
            case MenuState.Game:
                characterName.GetComponent<TMP_Text>().text = "Name: " + LevelManager.instance.characterName;
                gameParent.SetActive(true);
                break;
            case MenuState.Pause:
                pauseParent.SetActive(true);
                break;
            case MenuState.GameOver:
                gameOverParent.SetActive(true);
                break;
        }
    }

    private void CreateSavedCharacterButtons()
	{
        ClearSavedCharacterButtons();

        // Get an array of saved character files
        DirectoryInfo dataFolder = new DirectoryInfo(LoadingManager.instance.GetLocalSavePath());
        FileInfo[] dataFiles = dataFolder.GetFiles("*.json");

        float yPos = 50.0f;
        float deltaY = -50.0f;

        foreach(FileInfo file in dataFiles)
		{
            GameObject savedCharacterButton = Instantiate(savedCharacterButtonPrefab, savedCharacterButtonsParent.transform);

            yPos += deltaY;
            savedCharacterButton.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
            // Get the name of the character
            string trimmedName = Path.ChangeExtension(file.Name, null);
            savedCharacterButton.name = trimmedName;
            // Set the text of the button
            savedCharacterButton.transform.GetChild(0).GetComponent<TMP_Text>().text = trimmedName;   
            // Set the onClick
            savedCharacterButton.GetComponent<Button>().onClick.AddListener(() => LoadingManager.instance.LoadSavedCharacter(trimmedName));
        }
    }

    private void ClearSavedCharacterButtons()
	{

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
        LevelManager.instance.characterName = characterNameTextInput.GetComponent<TMP_InputField>().text;

        // Level up the character based on the class they selected
        foreach(Transform toggleTransform in characterClassTogglesParent.transform)
            if(toggleTransform.GetComponent<Toggle>().isOn)
                if(Enum.TryParse(toggleTransform.gameObject.name.Substring(11), out ClassType classLevel))
                    LevelManager.instance.LevelUp(classLevel);

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
}

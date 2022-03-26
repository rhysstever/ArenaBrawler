using System.Collections;
using System.Collections.Generic;
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
    private GameObject mainMenuParent, selectParent, gameParent, pauseParent, gameOverParent;

    [SerializeField]    // Main Menu Buttons
    private GameObject playButton, quitButton;

    [SerializeField]    // Select Buttons
    private GameObject selectCharacterButton;

    [SerializeField]    // Pause Buttons
    private GameObject resumeButton, saveButton, pauseToMainMenuButton;

    [SerializeField]    // Game Over Buttons
    private GameObject gameOverToMainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        SetupOnClicks();
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
    private void SetupOnClicks()
	{
        // Main Menu buttons
        playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Select));
        quitButton.GetComponent<Button>().onClick.AddListener(() => Application.Quit());
        // Select buttons        
        selectCharacterButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
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
                selectParent.SetActive(true);
                break;
            case MenuState.Game:
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
}

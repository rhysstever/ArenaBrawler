using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    MainMenu, 
    CharacterSelect,
    Game,
    Pause,
    GameOver
}

public class GameManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static GameManager instance = null;

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

    private MenuState currentMenuState;

	// Start is called before the first frame update
	void Start()
    {
        // Set the starting menuState
        ChangeMenuState(MenuState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Performs initial logic when the menuState changes
    /// </summary>
    /// <param name="newMenuState">The new menuState</param>
    public void ChangeMenuState(MenuState newMenuState)
	{
        currentMenuState = newMenuState;

        switch(newMenuState)
        {
            case MenuState.MainMenu:
                break;
            case MenuState.CharacterSelect:
                break;
            case MenuState.Game:
                break;
            case MenuState.Pause:
                break;
            case MenuState.GameOver:
                break;
        }
	}
}

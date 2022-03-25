using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static UIManager instance = null;

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

    [SerializeField]
    private Canvas canvas;

    [SerializeField]    // Empty parent gameObjects
    private GameObject mainMenuParent, selectParent, gameParent, pauseParent, gameOverParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary way to advance menu states
        if(Input.GetKeyDown(KeyCode.Return))
		{
            int currentMenuStateNum = (int)GameManager.instance.currentMenuState;
            currentMenuStateNum++;
            GameManager.instance.ChangeMenuState((MenuState)currentMenuStateNum);
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

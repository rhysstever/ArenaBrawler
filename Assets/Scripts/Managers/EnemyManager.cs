using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int health;
    public int movement;
    public int defense;
    public int damage;
    public int attackSpeed;

    #region Singleton Code
    // A public reference to this script
    public static EnemyManager instance = null;

    // Awake is called even before start 
    // (I think its at the very beginning of runtime)
    private void Awake()
    {
        // If the reference for this script is null, assign it this script
        if (instance == null)
            instance = this;
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movement = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

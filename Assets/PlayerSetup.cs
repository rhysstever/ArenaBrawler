using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{ 
    public int health;
    public int healthRegen;
    public int movement;
    public int defense;
    public int damage;
    public int attackSpeed;
    public int stamina;
    public int staminaRegen;

    // Start is called before the first frame update
    void Start()
    {
        movement = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int playerBaseMovement;
    private int sprintValue;
    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        playerBaseMovement = GetComponent<PlayerInfo>().characterClass.Movement;
        Debug.Log(playerBaseMovement);
    }

    // Update is called once per frame
    void Update()
    {
        // Sprinting
        if(Input.GetKeyDown(KeyCode.LeftShift))
		{
            // the player should be sprinting 
            playerBaseMovement *= sprintValue;
        }
    }
}

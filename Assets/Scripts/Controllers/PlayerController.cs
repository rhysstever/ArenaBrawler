using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Declaration of variables
    public float collisionOffset = 0.0f;
    public ContactFilter2D movementFilter;
    public float speed;
    Vector2 movementInput;

    //Declaration of references
    Rigidbody2D rigidbody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	private void Update()
	{
        speed = LevelManager.instance.player.GetComponent<Player>().movement;

        // Determine if the player can move
        canMove = GameManager.instance.GetCurrentMenuState() == MenuState.Game;

        //Get horizontal and vertical components of the Vector2 movement
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput .y);
        animator.SetFloat("Speed", speed);
	}

	private void FixedUpdate()
    {
        if (canMove)
        {
            //If player's movement is not 0
            if (movementInput != Vector2.zero)
            {
                //Try to move in any direction
                bool success = TryMove(movementInput);

                //If unable to move in initial direction, try to move in only the x direction
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    //If unable to move in x, try to move in y direction
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            //Check for potential collisions
            int count = rigidbody.Cast(direction, movementFilter, castCollisions, GetComponent<Player>().movement * Time.fixedDeltaTime + collisionOffset);

            //If no potential collisions are detected, move the character in the specified direction
            if (count == 0)
            {
                rigidbody.MovePosition(rigidbody.position + direction * GetComponent<Player>().movement * Time.fixedDeltaTime);
                return true;
            }
            return false;
        }
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}

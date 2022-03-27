using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Declaration of variables
    public float collisionOffset = 0.0f;
    public ContactFilter2D movementFilter;

    //Declaration of references
    Vector2 movementInput;
    new Rigidbody2D rigidbody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    //Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

                //animator.SetBool("isWalking", success);
            }
            else
            {
                //animator.SetBool("isWalking", false);
            }

            //Set direction of sprite to movement direction
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            //Check for potential collisions
            int count = rigidbody.Cast(direction, movementFilter, castCollisions, PlayerManager.instance.movement * Time.fixedDeltaTime + collisionOffset);

            //If no potential collisiosn are detected, move the character in the specified direction
            if (count == 0)
            {
                rigidbody.MovePosition(rigidbody.position + direction * PlayerManager.instance.movement * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}

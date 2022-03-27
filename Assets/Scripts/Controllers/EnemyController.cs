using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float enemyViewDistance = 4f;
    public ContactFilter2D movementFilter;
    private float distance;

    new Rigidbody2D rigidbody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    //Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 enemyMovementDirection = player.transform.position - transform.position;
        enemyMovementDirection.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (canMove)
        {
            //If the enemy's direction vector is not 0
            if (enemyMovementDirection != Vector2.zero)
            {
                //Try to move in the direction of the player
                bool success = TryMove(enemyMovementDirection);

                //If unable to move in the player's direction, don't move
                if (!success)
                {
                    return;
                }
            }
        }
    }

    private bool TryMove(Vector2 enemyMovementDirection)
    {
        if (enemyMovementDirection != Vector2.zero)
        {
            //Check for potential collisions
            int count = rigidbody.Cast(enemyMovementDirection, movementFilter, castCollisions, EnemyManager.instance.movement * Time.fixedDeltaTime);

            //If no potential collsions are detected, move the character in the specified direction
            if (count == 0)
            {
                //Check to see if the enemy can see the player
                if (distance < enemyViewDistance)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, EnemyManager.instance.movement * Time.deltaTime);
                    //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
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
        else
        {
            return false;
        }
    }
}

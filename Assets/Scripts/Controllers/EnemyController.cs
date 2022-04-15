using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float enemyViewDistance;
    public float enemyAttackDistance;
    public float enemySpeed;
    public ContactFilter2D movementFilter;
    public LayerMask whatIsPlayer;

    private Transform target;
    private Vector2 enemyMovement;
    private Vector3 enemyDirection;
    private float distance;

    new Rigidbody2D rigidbody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;

    bool canMove = true;
    bool isInChaseRange;
    bool isInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

	private void Update()
	{
        //Determine if the enemy can move
        canMove = GameManager.instance.GetCurrentMenuState() == MenuState.Game;
        //Determine if the enemy is moving
        animator.SetBool("isMoving", isInChaseRange);

        //Checks if the enemy is both within chase range and attack range
        isInChaseRange = Physics2D.OverlapCircle(transform.position, enemyViewDistance, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, enemyAttackDistance, whatIsPlayer);

        //distance = Vector2.Distance(transform.position, player.transform.position);
        enemyDirection = target.position - transform.position;
        float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
        enemyDirection.Normalize();
        enemyMovement = enemyDirection;

    }

    private void FixedUpdate()
    {
        //If the enemy is within chase range but not within attack range
        if(isInChaseRange && !isInAttackRange)
        {
            //If the enemy can move
            if(canMove)
            {
                //If the enemy's direction vector is not 0
                if(enemyDirection != Vector3.zero)
                {
                    //Try to move in the direction of the player, and return if it was successful
                    bool success = TryMove(enemyDirection);

                    //If unable to move in the player's direction, don't move
                    if(!success)
                    {
                        return;
                    }
                }
            }
        }
        if(isInAttackRange)
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    private bool TryMove(Vector2 enemyMovementDirection)
    {
        if (enemyMovementDirection != Vector2.zero)
        {
            //Check for potential collisions
            int count = rigidbody.Cast(enemyMovementDirection, movementFilter, castCollisions, GetComponent<Enemy>().movement * Time.fixedDeltaTime);

            //If no potential collsions are detected, move the character in the specified direction
            if (count == 0)
            {
                //Check to see if the enemy can see the player
                if (distance < enemyViewDistance)
                {
                    //Move towards the player in the new vector
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, GetComponent<Enemy>().movement * Time.deltaTime);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variable declaration
    public float enemyViewDistance;
    public float enemyAttackDistance;
    public float enemySpeed;
    public ContactFilter2D movementFilter;
    public LayerMask whatIsPlayer;

    private Transform target;
    private Vector3 enemyDirection;
    private float distance;

    new Rigidbody2D rigidbody;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;

    bool canMove;
    bool isInChaseRange;
    bool isInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = LevelManager.instance.player.transform;
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

    }

    private void FixedUpdate()
    {
        //If:
        //Enemy is within chase range
        //Enemy is not within attack range
        //Enemy can move
        //Enemy direction vector is not zero
        if(isInChaseRange && !isInAttackRange && canMove && enemyDirection != Vector3.zero)
        {
            //Try to move in the direction of the player, and return if it was successful
            bool success = TryMove(enemyDirection);
        }
        else if(isInAttackRange)
        {
            rigidbody.velocity = Vector2.zero;

            //Attack the player
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
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, GetComponent<Enemy>().movement * Time.deltaTime);
                    return true;
                }
            }
        }
        return false;
    }
}

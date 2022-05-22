using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private int xpAmount;
    private int goldAmount;
    private float enemyViewDistance;
    private float enemyAttackDistance;
    private float enemyMaxHealth = 100f;
    private float enemyCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        movement = 2;
        enemyViewDistance = 3f;
        enemyAttackDistance = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the amount of XP and gold that the enemy drops when killed
    /// </summary>
    /// <param name="xpAmount">The amount of XP the enemy drops</param>
    /// <param name="goldAmount">The amount of gold the enemy drops</param>
    public void SetLoot(int xpAmount, int goldAmount)
	{
		this.xpAmount = xpAmount;
        this.goldAmount = goldAmount;
	}

	/// <summary>
	/// Get the XP and gold amount for the enemy
	/// </summary>
	/// <returns>The XP and gold amount</returns>
	public (int, int) GetLoot()
	{
        return (xpAmount, goldAmount);
	}

    /// <summary>
    /// Get the enemyViewDistance and the enemyAttackDistance
    /// </summary>
    /// <returns></returns>
    public (float, float) GetViewAndAttackRadii()
    {
        return (enemyViewDistance, enemyAttackDistance);
    }

    /// <summary>
    /// Removes health from the enemy; checks for death
    /// </summary>
    /// <param name="amount">The amount damage the enemy takes</param>
	public override void TakeDamage(float amount)
	{
		base.TakeDamage(amount);

        // Check for death
        if(enemyCurrentHealth <= 0.0f)
        {
            LevelManager.instance.player.GetComponent<Player>().CollectResources(xpAmount, goldAmount);
            Die();
        }
	}

    public void Die()
    {
        Debug.Log("Enemy died!");
    }
}

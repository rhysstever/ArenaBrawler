using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private int xpAmount;
    private int goldAmount;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
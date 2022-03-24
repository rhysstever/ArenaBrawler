using System.Collections;
using System.Collections.Generic;

public class CharacterClass
{
	#region Fields
	private int health;
	private int healthRegen;
	private int movement;
	private int defense;
	private int damage;
	private int attackSpeed;
	private int stamina;
	private int staminaRegen;
	private bool isSprinting;
	#endregion

	#region Properties
	public int Health { get { return health; } }
	public int HealthRegen { get { return healthRegen; } }
	public int Movement { get { return movement; } }
	public int Defense { get { return defense; } }
	public int Damage { get { return damage; } }
	public int AttackSpeed { get { return attackSpeed; } }
	public int Stamina { get { return stamina; } }
	public int StaminaRegen { get { return staminaRegen; } }
	#endregion

	#region Constructor
	public CharacterClass(int health, int healthRegen, int movement, int defense, int damage, int attackSpeed, int stamina, int staminaRegen)
	{
		this.health = health;
		this.healthRegen = healthRegen;
		this.movement = movement;
		this.defense = defense;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		this.stamina = stamina;
		this.staminaRegen = staminaRegen;
	}
	#endregion

	#region Methods
	public void AddHealth(int amount)
	{
		health += amount;
	}
	#endregion
}

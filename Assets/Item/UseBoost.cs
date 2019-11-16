using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBoost {
	public float armor;
	public float healing;
	public float health;
	public float evasion;
	public float physical_dmg;
	public float magical_dmg;
	public float range;
	public float walk_speed;
	public float health_regen;
	public UseBoost(float armor, float healing, float health_regen, float health, float evasion, 
					float physical_dmg, float magical_dmg, float range, float walk_speed)
	{
		this.armor = armor;
		this.healing = healing;
		this.health = health;
		this.evasion = evasion;
		this.physical_dmg = physical_dmg;
		this.magical_dmg = magical_dmg;
		this.range = range;
		this.walk_speed = walk_speed;
		this.health_regen = health_regen;
	}

	static public UseBoost zero
	{
		get
		{
			return new UseBoost(0,0,0,0,0,0,0,0,0);
		}
	}

	public void AddBoost(UseBoost b)
	{
		armor += b.armor;
		healing += b.healing;
		health += b.health;
		evasion += b.evasion;
		physical_dmg += b.physical_dmg;
		magical_dmg += b.magical_dmg;
		range += b.range;
	}
	public void decBoost(UseBoost b)
	{
		armor -= b.armor;
		healing -= b.healing;
		health -= b.health;
		evasion -= b.evasion;
		physical_dmg -= b.physical_dmg;
		magical_dmg -= b.magical_dmg;
		range -= b.range;
	}
	public UseBoost Copy()
	{
		return new UseBoost(armor, healing, health_regen, health, evasion, physical_dmg, magical_dmg, range, walk_speed);
	}
}

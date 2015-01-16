using UnityEngine;
using System.Collections;

public class Item{
	//public string name;
	public string description;
	public string classRestriction;  // "none" = no restriction
	public string name;
	public Sprite picture;

	//stat bonuses default to zero
	public int constitutionBonus;
	public int HPModBonus;
	public int healthBonus;
	public int strengthBonus;
	
	//use items
	public int hpRestore;
	public bool scurvyReset;
	public bool isEquip;
	public int weaponAttack;

	public void start(){
		isEquip = false;
		constitutionBonus = 0;
		weaponAttack = 0;
		HPModBonus = 0;
		healthBonus = 0;
		strengthBonus = 0;
		classRestriction = "none";

		hpRestore = 0;
		scurvyReset = false;
	}

	public override string ToString(){
		if (weaponAttack == 0)
			return name + "\n" + description + "\n" + "Constitution: " + constitutionBonus + "\n" + 
						"HP Mod: " + HPModBonus + "\n" + "Health Bonus:" + healthBonus + "\n" + "Strength: " + strengthBonus + "\n" +
						"HP Restore: " + hpRestore;
		else
			return name + "\n" + description + "\n" + "Weapon Attack:" + weaponAttack;
		}
}

public class Apple : Item{
	public Apple(){
	hpRestore = 25;
	scurvyReset = true;
		description = "Prevents scurvy and tastes delicious!  Restores 25 HP; prevents scurvy.";
		}
}

public class Rum : Item{
	public Rum(){
		hpRestore = -1;
		description = "Prevents unhappiness and tastes terrible! Subtracts 1 hp, and adds 25 pillage power.";
	}
}
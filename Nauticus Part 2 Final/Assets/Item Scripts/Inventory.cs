using UnityEngine;
using System.Collections;

/// <summary>
/// Inventory.  Handles equipped items, equipping items, and inventory organization.
/// </summary>
public class Inventory{
	public Weapon equippedWeapon;
	public Armor equippedArmor;
	PirateHero _Piro;

	// Use this for initialization
	void Start () {
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		//numItems = 0;
		//invSize = 300;
		//invTable = new Item[30];
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void equipItem(Item item){
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		if(item is Weapon && _Piro){
			if(equippedWeapon != null)
				unapplyStats(equippedWeapon);
			equippedWeapon = (Weapon)item;
			applyStats (item);
		}
		if(item is Armor){
			if(equippedArmor != null)
				unapplyStats(equippedArmor);
			equippedArmor = (Armor)item;
			applyStats (item);
		}
	}

	void applyStats(Item item){
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		if (item is Weapon) {
			_Piro.weaponAttack = ((Weapon)item).weaponAttack;
		}
		if (item is Armor) {
			_Piro.constitution += item.constitutionBonus;
			_Piro.HPMod += item.HPModBonus;
			_Piro.strength += item.strengthBonus;
			//_Piro.health = _Piro.calculateHealth ();
			_Piro.health += item.healthBonus;
			_Piro.armor = ((Armor)item).armor;
		}
	}

	void unapplyStats(Item item){
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		_Piro.constitution -= item.constitutionBonus;
		_Piro.HPMod -= item.HPModBonus;
		_Piro.strength -= item.strengthBonus;
		_Piro.health -= item.healthBonus;
		//_Piro.health = _Piro.calculateHealth ();
	}


}
using UnityEngine;
using System.Collections;

public class Boss : Actor {
	PirateHero _Piro;
	
	// Use this for initialization
	void Start () {
		//Zombie will tell the pirate when he's next to him
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		charClass = "Boss";
		name = "Boss";
		level = 90;
		equipItem (new Zombie_Arm ());
		setStats ();
		calculateAndSetHealth ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	/// <summary>
	/// Sets the stats.
	/// </summary>
	void setStats(){
		constitution = 3;
		HPMod = 6;
		strength = 4;
	}
	
	/// <summary>
	/// Sets the start position.
	/// </summary>
	public void setStartPosition(int x, int y, int tileSize){
		x_pos = x;
		y_pos = y;
		transform.rotation = new Quaternion (0, 0.9f, -0.4f, 0);
		transform.position = new Vector3(x_pos * tileSize + 3, 3, y_pos * tileSize + 5);
	}
	
	public void equipItem(Item item){
		if (item is Weapon) {
			equippedWeapon = (Weapon)item;
		}
		if (item is Armor) {
			equippedArmor = (Armor)item;
		}
		applyStats (item);
	}
	
	void applyStats(Item item){
		constitution += item.constitutionBonus;
		HPMod += item.HPModBonus;
		strength += item.strengthBonus;
		calculateAndSetHealth ();
		health += item.healthBonus;
		if (item is Weapon) {
			weaponAttack = ((Weapon)item).weaponAttack;
		}
		if (item is Armor) {
			armor = ((Armor)item).armor;
		}
	}
	
	public override void Act(){
		ChooseAction ();
	}

	public override void Die(){
		Application.LoadLevel ("win");
	}
	
	void ChooseAction(){
		//check if he should attack piro
		//check up, down, left, right for dat pirate
		if ((x_pos == _Piro.x_pos && y_pos + 1 == _Piro.y_pos) || 
		    (x_pos == _Piro.x_pos && y_pos - 1 == _Piro.y_pos) || 
		    (x_pos - 1 == _Piro.x_pos && y_pos == _Piro.y_pos) || 
		    (x_pos + 1 == _Piro.x_pos && y_pos == _Piro.y_pos)) {
			Attack (_Piro.gameObject);
		}
	}
}
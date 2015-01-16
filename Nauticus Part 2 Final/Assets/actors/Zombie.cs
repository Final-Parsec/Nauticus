using UnityEngine;
using System.Collections;

public class Zombie : Actor {
	TileMap _tileMap;
	PirateHero _Piro;
	int speed = 50;
	Vector3? targetPosition;

	// Use this for initialization
	void Start () {
		_tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
		//Zombie will tell the pirate when he's next to him
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		charClass = "Zombie";
		name = "Zombie";
		level = 1;
		equipItem (new Zombie_Arm ());
		hasDrops = true;
		setStats ();
		calculateAndSetHealth ();
		}
	
	// Update is called once per frame
	void Update () {
		if (targetPosition.HasValue) {
			MoveTowardTargetPosition (targetPosition, speed);
			///check if destination is nigh
			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.Value.x, transform.position.y, targetPosition.Value.z)) < .9) {
				transform.position = targetPosition.Value;
				targetPosition = null;
			} 
		}
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


	void ChooseAction(){
		//check if he should attack piro
		//check up, down, left, right for dat zombie
		if ((x_pos == _Piro.x_pos && y_pos + 1 == _Piro.y_pos) || 
				(x_pos == _Piro.x_pos && y_pos - 1 == _Piro.y_pos) || 
				(x_pos - 1 == _Piro.x_pos && y_pos == _Piro.y_pos) || 
				(x_pos + 1 == _Piro.x_pos && y_pos == _Piro.y_pos)) {
				Attack (_Piro.gameObject);
		}
		//check to move
		//left and right
		else if((Mathf.Abs(x_pos - _Piro.x_pos) < 5) && (Mathf.Abs(y_pos - _Piro.y_pos) < 5)){

			//right
			if (x_pos - _Piro.x_pos < 0 && _tileMap.canMove(x_pos+1, y_pos)){
				targetPosition = new Vector3(transform.position.x+5, transform.position.y, transform.position.z);
				MoveTowardTargetPosition(targetPosition, speed);
				x_pos++;
			}

			//left
			if (x_pos - _Piro.x_pos > 0 && _tileMap.canMove(x_pos-1, y_pos)){
				targetPosition = new Vector3(transform.position.x-5, transform.position.y, transform.position.z);
				MoveTowardTargetPosition(targetPosition, speed);
				x_pos--;
			}
			//up and down
			if (y_pos - _Piro.y_pos < 0 && _tileMap.canMove(x_pos, y_pos+1)){
				targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+5);
				MoveTowardTargetPosition(targetPosition, speed);
				y_pos++;
			}
			if (y_pos - _Piro.y_pos > 0 && _tileMap.canMove(x_pos, y_pos-1)){
				targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-5);
				MoveTowardTargetPosition(targetPosition, speed);
				y_pos--;
			}
		}
	}
}
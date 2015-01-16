using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public int x_pos;
	public int y_pos;

	//game mechanic variables
	public string charClass;
	public int level;
	public int HPMod;
	public int health;  //class level * class health modifier + con
	public int constitution;
	public int strength;
	public int weaponAttack;
	public int armor;

	public bool hasDrops = false;
	// damage = weapon attack + strength - enemy armor
	
	public Weapon equippedWeapon;
	public Armor equippedArmor;

	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// Moves the instance toward target position.
	/// </summary>
	public void MoveTowardTargetPosition(Vector3? targetPosition, float speed) {
		Vector3 moveVector = new Vector3(transform.position.x - targetPosition.Value.x,
		                                 0,
		                                 transform.position.z - targetPosition.Value.z).normalized;
		
		// update the position
		transform.position = new Vector3(transform.position.x - moveVector.x * speed * Time.deltaTime,
		                                 transform.position.y,
		                                 transform.position.z - moveVector.z * speed * Time.deltaTime);
		
	}

	/// <summary>
	/// Calculates the health.
	/// </summary>
	/// <returns>The health.</returns>
	public int calculateHealth(){
		return level * (HPMod + constitution);
	}
	
	/// <summary>
	/// Calculates the health.
	/// </summary>
	/// <returns>The health.</returns>
	public void calculateAndSetHealth(){
		health = level * (HPMod + constitution);
	}
	
	/// <summary>
	/// Calculates the and set damage.
	/// </summary>
	public int CalculateDamage(int enemyArmor){
		return weaponAttack + strength;  //subtract enemy armor
	}

	/// <summary>
	/// called on death of this instance.
	/// </summary>
	public virtual void Die(){
	}

	public virtual void Attack(GameObject enemy){
		Actor enemyActor = enemy.GetComponent<Actor> ();
		enemyActor.health -= CalculateDamage (enemyActor.armor); //damage = weapon attack + strength - enemy armor
	}

	public void spawnCrate(){
		if(Random.Range (1,5) == 1)  // 1/5 chance for drops
			GameObject.Find("TileMap").GetComponent<TileMap>().spawnCrate (transform.position, x_pos, y_pos);
	}
	
	public virtual void Act(){}

	public void killyoself(){
		Die ();
		if (hasDrops)
			spawnCrate ();
		Destroy (gameObject);
	}

	public override string ToString(){
		return "Class: " + charClass + "\n" + 
				"level: " + level + "\n" + 
				"HPMod: " + HPMod + "\n" + 
				"Max Health: " + health + "\n" +
				"Constitution: " + constitution + "\n" + 
				"Strength: " + strength + "\n" +
				"WeaponAttack: " + weaponAttack + "\n" + 
				"Armor: " + armor;
	}
}
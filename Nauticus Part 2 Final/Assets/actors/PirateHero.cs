using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/// <summary>
/// Pirate hero.  Legend of Destiny and Charisma.
/// </summary>
public class PirateHero : Actor {
	TileMap _tileMap;
	PlayerLight _playerLight;

	private Vector3? targetPosition;
	public float speed;

	public Toolbox toolbox;

	EnemyMaster Enemast;

	int buccCount;
	public Item crateItem;
	public bool onCrate;
	ItemCrate curCrate;

	public int scurvy;
	public int maxScurvy;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		_tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
		_playerLight = GameObject.Find("Player Light").GetComponent<PlayerLight>();

		scurvy = 0;
		maxScurvy = 150;

		Vector2 pos_vec = _tileMap.getSpawnPos();
		x_pos = (int)pos_vec.x;
		y_pos = (int)pos_vec.y;

		//set some backend attributes
		speed = 50;
		Enemast = new EnemyMaster ();

		//get character class set from last scene
		toolbox = Toolbox.Instance;
		charClass = toolbox.charClass;
		if (Application.loadedLevelName == "Game") {
			toolbox.inv = new Inventory ();	
			toolbox.inv.equipItem (new Bilgewater_Rags ());
			toolbox.inv.equipItem (new Zombie_Arm ());
		}
		setStartingStats ();
		if (charClass == "Rapscallion")
			health += 200;
		if (charClass == "Buccaneer")
			buccCount = 0;
		setStartPosition ();
		//Debug.Log (pos_vec.ToString());
		//transform.position = new Vector3(x_pos, transform.position.y, y_pos);
	}

	/// <summary>
	/// Initializes a new instance of the "PirateHero" class.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public PirateHero(int x, int y){
		    x_pos = x;
			y_pos = y;
		}
	
	// Update is called once per frame
	void Update () {
		if (targetPosition.HasValue) {
			MoveTowardTargetPosition ();
			///check if destination is nigh
			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.Value.x, transform.position.y, targetPosition.Value.z)) < .9) {
				transform.position = targetPosition.Value;
				targetPosition = null;
			} 
		}
		else
			HandleInputForMovement ();

		if (health < 0)
			killyoself ();
	}
	/// <summary>
	/// Handles the input for movement.
	/// </summary>
	public void HandleInputForMovement(){
		if (Input.GetKeyUp (KeyCode.W)) {
			GameObject tempEnemy;
			if(_tileMap.canMove(x_pos, y_pos+1)){
				tempEnemy = Enemast.CheckTileForEnemy(x_pos, y_pos+1);
				if (tempEnemy != null){
					classAttack(tempEnemy);
					Enemast.CheckForDeadEnemy (tempEnemy);
					enemyTurn();
				}
				else{ // move into free space if there is no enemy
					y_pos += 1;
					targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z+5);
					move();
					enemyTurn();
				}
			}
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			GameObject tempEnemy;
			if(_tileMap.canMove (x_pos+1, y_pos)){
				tempEnemy = Enemast.CheckTileForEnemy(x_pos+1, y_pos);
				if(tempEnemy != null){
					classAttack (tempEnemy);
					Enemast.CheckForDeadEnemy (tempEnemy);
					enemyTurn();
				}
				else{
					x_pos += 1;
					targetPosition = new Vector3(transform.position.x+5, transform.position.y, transform.position.z);
					move();
					enemyTurn();
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.A)) {
			GameObject tempEnemy;
			if(_tileMap.canMove (x_pos-1, y_pos)){
				tempEnemy = Enemast.CheckTileForEnemy(x_pos-1, y_pos);
				if(tempEnemy != null){
					classAttack (tempEnemy);
					Enemast.CheckForDeadEnemy (tempEnemy);
					enemyTurn();
				}
				else{
					x_pos -= 1;
					targetPosition = new Vector3(transform.position.x-5, transform.position.y, transform.position.z);
					move();
					enemyTurn();
				}
			}
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			GameObject tempEnemy;
			if(_tileMap.canMove("down", x_pos, y_pos)){
				tempEnemy = Enemast.CheckTileForEnemy(x_pos, y_pos-1);
				if(tempEnemy != null){
					classAttack (tempEnemy);
					Enemast.CheckForDeadEnemy (tempEnemy);
					enemyTurn();
				}
				else{
					y_pos -= 1;
					targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-5);
					move();
					enemyTurn();
				}
			}
		}
		if (Input.GetKeyUp (KeyCode.Space) && onCrate) {
			onCrate = false;
			if(crateItem.isEquip)
				toolbox.inv.equipItem (crateItem);
			else
				applyConsumable(crateItem);
			curCrate.killyoself();  //despawn crate. THAAAAAAT's why we needed curCrate
			enemyTurn();  //picking up an item counts as a turn!
		}
		if (Input.GetKeyUp (KeyCode.Space) && (_tileMap.Staircase.GetComponent<StairsDown>().x_pos == x_pos && _tileMap.Staircase.GetComponent<StairsDown>().y_pos == y_pos))
				if (Application.loadedLevelName == "Game")
						Application.LoadLevel ("Game2");
				else if (Application.loadedLevelName == "Game2")
						Application.LoadLevel ("Game3");
	}

	/// <summary>
	/// Moves the instance toward target position.
	/// </summary>
	void MoveTowardTargetPosition() {
		Vector3 moveVector = new Vector3(transform.position.x - targetPosition.Value.x,
		                                 0,
		                                 transform.position.z - targetPosition.Value.z).normalized;
		
		// update the position
		transform.position = new Vector3(transform.position.x - moveVector.x * speed * Time.deltaTime,
		                                 transform.position.y,
		                                 transform.position.z - moveVector.z * speed * Time.deltaTime);

	}

	/// <summary>
	/// Runs once when the player gets a move input and it succeeds in moving the player
	/// </summary>
	public void move(){
		_playerLight.setPosition (transform.position);
		onCrate = false;

		//regen for buccaneer
		if (charClass == "Buccaneer"){
			buccCount++;
			if(buccCount % 4 == 0)
				health++;
		}

		//set scurvy
		if ( Random.Range (0,2) == 1 )
			scurvy++;
		if (scurvy >= maxScurvy)
			killyoself();
		}

	/// <summary>
	/// Sets the start position.
	/// </summary>
	public void setStartPosition(){
		transform.rotation = new Quaternion (0, 0.9f, -0.4f, 0);
		transform.position = new Vector3(x_pos * _tileMap.tileSize + 3, 3, y_pos * _tileMap.tileSize + 5);
		_playerLight.setPosition (transform.position);
	}

	/// <summary>
	/// Sets the starting stats for Piro, including Health and Level(level = 1).
	/// </summary>
	private void setStartingStats(){
		switch (charClass) {
			case "Swashbuckler":
					constitution = 15;
					HPMod = 50;
					strength = 15;
					break;
			case "Buccaneer":
					constitution = 15;
					HPMod = 60;
					strength = 20;
					break;
			case "Picaroon":
					constitution = 10;
					HPMod = 40;
					strength = 7;
					break;
			case "Rapscallion":
					constitution = 10;
					HPMod = 40;
					strength = 10;
					break;
			default:
					Debug.Log ("class selection error");
					charClass = "ADMIN";
					constitution = 100;
					HPMod = 400;
					strength = 1000;
			break;
		}

		level = 1;
		health = calculateHealth();
	}

	public void classAttack(GameObject tempEnemy){
		//crits
		if(charClass == "Swashbuckler")
			if(Random.Range (0, 10) == 1){
				weaponAttack *= 2;
				Attack (tempEnemy);
				weaponAttack /= 2;
				return;
			}
		if (charClass == "Picaroon")
			if (Random.Range (0, 4) == 1) {
				weaponAttack *= 4;
				Attack (tempEnemy);
				weaponAttack /= 4;
				return;
			}
		if (charClass == "Rapscallion")
			if (Random.Range (0,5) == 1) {
				weaponAttack *= 2;
				Attack(tempEnemy);
				weaponAttack /= 2;
				return;
			}
		Attack (tempEnemy);
	}


	/// <summary>
	/// Is on dat crate.
	/// </summary>
	/// <param name="it">It.</param>
	/// <param name="itCrate">It crate.</param>
	public void isOnCrate(Item it, ItemCrate itCrate){
		curCrate = itCrate;
		crateItem = it;
		onCrate = true;
	}

	void applyConsumable(Item food){
		health += food.hpRestore;
		if (food.scurvyReset)
			scurvy = 0;
	}

	/// <summary>
	/// Enemies the turn.
	/// </summary>
	public void enemyTurn(){
		Enemast.CheckForDeadEnemies ();
		Enemast.StartEnemyTurn ();
	}
}
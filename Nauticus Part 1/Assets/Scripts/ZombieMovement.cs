using UnityEngine;
using System.Collections;

public class ZombieMovement : MonoBehaviour {

	private TileMap _tileMap;
	private PlayerControl piroThePirateHero;
	private bool isMoving = false;
	private bool isSwimming = false;
	private Vector3? targetPosition;
	//private Animator animator;
	private BloodyOverlay bloodyOverlay;
	
	public float speed = 13;
	private float _speed;
	
	private IEnumerator FlashBloodyOverlay(float severity)
	{
		bloodyOverlay.Fade(0, severity, .2);
		yield return new WaitForSeconds(.25f);
		bloodyOverlay.Fade(severity, 0, .2);
	}
	
	void OnTriggerEnter(Collider col)
	{
		int damage = Random.Range (10, 20);
		piroThePirateHero.Health -= damage;
		piroThePirateHero.audio.Play();		
		
		if (bloodyOverlay)
		{
			StartCoroutine(FlashBloodyOverlay(((float)(damage - 10)/(float)10)));
		}
		else 
		{
			bloodyOverlay = GetComponent<BloodyOverlay>();
		}
	}

	void ChangeElevationForSwimming() {
		int currentTileType = _tileMap.GetTileAt(transform.position.x, transform.position.z);
		
		if (currentTileType == 3 || currentTileType == 4) {
			if (!isSwimming) {
				transform.position = new Vector3(transform.position.x, transform.position.y-2.4f, transform.position.z);
				isSwimming = true;
			} 
		} else {
			if (isSwimming) {
				transform.position = new Vector3(transform.position.x, transform.position.y+2.4f, transform.position.z);
				isSwimming = false;
			}
		}
	}
	
	void PickNewLocation() {
		// todo: eventually make him chase after piro or more intelligently pick a path.
	
		int numberOfTilesToMove = Random.Range (1, 5);
	
		switch (Random.Range (1, 5)) {
			case 1:
				targetPosition = new Vector3(transform.position.x + (_tileMap.tileSize * numberOfTilesToMove), transform.position.y, transform.position.z);
				transform.rotation = Quaternion.Euler(new Vector3(-45, 0, 0));
				break;
			case 2:
			targetPosition = new Vector3(transform.position.x - (_tileMap.tileSize * numberOfTilesToMove), transform.position.y, transform.position.z);
				transform.rotation = Quaternion.Euler(new Vector3(45, 180, 0));
				break;
			case 3:
			targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - (_tileMap.tileSize * numberOfTilesToMove));
				break;
			case 4:
			targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + (_tileMap.tileSize * numberOfTilesToMove));
				break;
		}
		
		if (targetPosition.HasValue && _tileMap.GetTileAt(targetPosition.Value.x, targetPosition.Value.z) == -1) {
			targetPosition = null;
		}
	}
	
	void MoveTowardTargetPosition() {
		Vector3 moveVector = new Vector3(transform.position.x - targetPosition.Value.x,
		                                 0,
		                                 transform.position.z - targetPosition.Value.z).normalized;
		
		// update the position
		transform.position = new Vector3(transform.position.x - moveVector.x * _speed * Time.deltaTime,
		                                 transform.position.y,
		                                 transform.position.z - moveVector.z * _speed * Time.deltaTime);
		
	}

	// Use this for initialization
	void Start () {
		_tileMap = GameObject.Find ("TileMap").GetComponent<TileMap> ();		
		piroThePirateHero = GameObject.Find ("pirate_hero").GetComponent<PlayerControl> ();
		bloodyOverlay = GameObject.Find ("bloody_overlay").GetComponent<BloodyOverlay>();
		
		//animator = GetComponent<Animator> ();
		
		_speed = speed;
		
		ChangeElevationForSwimming();
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPosition.HasValue) {
			if (!isMoving) {
				isMoving = true;
				//animator.SetBool ("isWalking", true);
			}
			
			MoveTowardTargetPosition ();
			ChangeElevationForSwimming ();
			
			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.Value.x, transform.position.y, targetPosition.Value.z)) < .5) {
				targetPosition = null;
				isMoving = false;
				//animator.SetBool ("isWalking", false);			
			}
		} else {
			PickNewLocation();
		}
	}
}

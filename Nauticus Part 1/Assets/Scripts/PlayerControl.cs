using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour {

	private TileMap _tileMap;
	private bool isMoving = false;
	public float yDeltaForSwimming = 6.1f;
	private bool isSwimming = false;
	private Vector3? targetPosition;
	private Animator animator;	

	public float speed = 10;
	private float _speed;
	public float runInputDelay = 2;
	private float _runInputDelay;
	private int health = 100;
	private int rumBottles = 0;
	private MenuBackgroundOverlay menuBackgroundOverlay;
	private PlunderedOverlay plunderedOverlay;
	
	public bool IsInGame { get; set; }
	
	public int Health
	{
		get 
		{
			return health;
		}
		set 
		{
			if (value < 0)
			{
				health = 0;
				StartCoroutine(TurnOnPlunderedOverlay());
				InvokeRepeating("DeathAnimation", 0f, .1f);
				GameObject.Find("plundered_text").GetComponent<Text>().enabled = true;
				GetComponent<AudioSource>().mute = true;
			}
			else if (value > 100) {
				health = 100;
			}
			else
			{
			 health = value;
			}
		}
	}
	
	public int RumBottles 	
	{
		get
		{
			return rumBottles;
		}
		set
		{
			rumBottles = value;
		}
	}
	
	private int x = 0, y = 0, z = 0;
	private int xTarget = 0, yTarget = 0, zTarget = 0;
	private void DeathAnimation()
	{
		if (x < xTarget)
			x ++;
		else if (x > xTarget)
			x --;
		else 
			xTarget = Random.Range(1, 360);
			
		if (y < yTarget)
			y ++;
		else if (y > yTarget)
			y --;
		else 
			yTarget = Random.Range(1, 360);
			
		if (z < zTarget)
			z ++;
		else if (z > zTarget)
			z --;
		else 
			zTarget = Random.Range(1, 360);			
			
		transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
		
	}
	
	private IEnumerator TurnOnMenuBackgroundOverlay() {
		menuBackgroundOverlay.Fade(0, 1, 1);
		yield return new WaitForSeconds(.25f);
	}
		
	private IEnumerator TurnOnPlunderedOverlay()
	{
		plunderedOverlay.Fade(0, .39, .2);
		yield return new WaitForSeconds(.25f);
	}
	
	void CheckForWinCondition() {
		int currentTileType = _tileMap.GetTileAt(transform.position.x, transform.position.z);
		
		// Piro has stepped on the final tile and wins the game.
		if (currentTileType == 0) {
			StartCoroutine(TurnOnMenuBackgroundOverlay());
			// GameObject.Find ("victory_text").guiText.enabled = true;
			IsInGame = false;
		}
	}
	
	void ChangeElevationForSwimming() {
		int currentTileType = _tileMap.GetTileAt(transform.position.x, transform.position.z);
		
		if (currentTileType == 3 || currentTileType == 4) {
			if (!isSwimming) {
				transform.position = new Vector3(transform.position.x, transform.position.y-yDeltaForSwimming, transform.position.z);
				isSwimming = true;
			} 
		} else {
			if (isSwimming) {
				transform.position = new Vector3(transform.position.x, transform.position.y+yDeltaForSwimming, transform.position.z);
				isSwimming = false;
			}
		}
	}

	void HandleInputForMovement() {
		if (Input.GetKey (KeyCode.A)) {
			targetPosition = new Vector3(transform.position.x + _tileMap.tileSize, transform.position.y, transform.position.z);
			transform.rotation = Quaternion.Euler(new Vector3(-45, 0, 0));
		}
		
		if (Input.GetKey (KeyCode.D)) {
			targetPosition = new Vector3(transform.position.x - _tileMap.tileSize, transform.position.y, transform.position.z);
			transform.rotation = Quaternion.Euler(new Vector3(45, 180, 0));
		}
		
		if (Input.GetKey (KeyCode.W)) {
			targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - _tileMap.tileSize);
		}
		
		if (Input.GetKey (KeyCode.S)) {
			targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + _tileMap.tileSize);
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
		animator = GetComponent<Animator> ();		
		
		_runInputDelay = runInputDelay;
		_speed = speed;
		
		ChangeElevationForSwimming();
		
		IsInGame = true;
		
		menuBackgroundOverlay = GameObject.Find ("menu_background_overlay").GetComponent<MenuBackgroundOverlay>();
		plunderedOverlay = GameObject.Find ("plundered_overlay").GetComponent<PlunderedOverlay>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Health == 0)
		{
			// Piro doesn't do shit if he's dead.
			return;
		}
	
		if (Input.GetKey  (KeyCode.A) || Input.GetKey(KeyCode.D)) {
			_runInputDelay -= Time.deltaTime;
			if (_runInputDelay < 0 && _speed == speed) {
				_speed *= 1.2f;
				animator.speed *= 1.2f;
				_runInputDelay = runInputDelay;
			}
		} else {
			_speed = speed;
			animator.speed = 1.0f;
			_runInputDelay = runInputDelay;
		}
		
	
		if (targetPosition.HasValue) {
			if (!isMoving) {
				isMoving = true;
				animator.SetBool ("isWalking", true);
			}

			MoveTowardTargetPosition ();
			ChangeElevationForSwimming ();
			CheckForWinCondition();

			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.Value.x, transform.position.y, targetPosition.Value.z)) < .5) {
				targetPosition = null;
				isMoving = false;
				animator.SetBool ("isWalking", false);			
			}
		} else {
			HandleInputForMovement();
		}
		
		if (!menuBackgroundOverlay){
			menuBackgroundOverlay = GameObject.Find ("menu_background_overlay").GetComponent<MenuBackgroundOverlay>();
		}
		
		if (!plunderedOverlay){
			plunderedOverlay = GameObject.Find ("plundered_overlay").GetComponent<PlunderedOverlay>();
		}
	}
}

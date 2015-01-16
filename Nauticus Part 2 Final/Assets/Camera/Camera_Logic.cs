using UnityEngine;
using System.Collections;

public class Camera_Logic : MonoBehaviour {
	Camera _Camera;
	PirateHero _Piro;
	HUD _HUD;

	private Vector3? targetPosition;
	public float speed;

	[ExecuteInEditMode]
	// Use this for initialization
	void Start () {
		_Camera = GameObject.Find("Camera").GetComponent<Camera>();
		//TODO: put this script on camera?
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		speed = 40;
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
			move();
	}

	/// <summary>
	/// Moves the toward target position.
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
	/// Move the camera if the player is out of it's 10 tile rectangle.  
	/// 0-10 player coordinates = -50 camera
	/// 11-20 player coordinates == -20 camera
	/// </summary>
	void move(){
		//check if pirate is too far
		if (_Piro.transform.position.z - 70 > _Camera.transform.position.z) {
			targetPosition = new Vector3(_Camera.transform.position.x, _Camera.transform.position.y, _Camera.transform.position.z + 30);
		} 
		//check if pirate is too close
		else if (_Piro.transform.position.z - 40 <= _Camera.transform.position.z) {
			targetPosition = new Vector3(_Camera.transform.position.x, _Camera.transform.position.y, _Camera.transform.position.z - 30);
		}
		//check if pirate is too far left
		if (_Piro.transform.position.x - 15 > _Camera.transform.position.x) {
			targetPosition = new Vector3 (_Camera.transform.position.x + 15, _Camera.transform.position.y, _Camera.transform.position.z);
		}
		//check if pirate is too far right
		else if (_Piro.transform.position.x + 15 <= _Camera.transform.position.x) {
			targetPosition = new Vector3 (_Camera.transform.position.x - 15, _Camera.transform.position.y, _Camera.transform.position.z);
		}

	}

	/// <summary>
	/// Sets the target position.
	/// </summary>
	/// <param name="tp">Tp.</param>
	public void setTargetPosition(Vector3? tp){
		targetPosition = tp;
	}

}
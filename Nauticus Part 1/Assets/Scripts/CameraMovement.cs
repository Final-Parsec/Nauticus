using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private PlayerControl pirateHero;
	private Vector3 targetPosition  = new Vector3(195, 45, -10);  // TODO: base this on pirateHero position in start method. defaults to hard coded position
	private bool runningStartAnimation = true; 
	public float speed = 50;

	// Use this for initialization
	void Start () {
		
	
		pirateHero = GameObject.Find ("pirate_hero").GetComponent<PlayerControl> ();
		targetPosition = new Vector3(pirateHero.transform.position.x, pirateHero.transform.position.y + 20, pirateHero.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (runningStartAnimation) {
			Vector3 moveVector = new Vector3(transform.position.x - targetPosition.x,
			                                 transform.position.y - targetPosition.y,
			                                 transform.position.z - targetPosition.z).normalized;
			
			// update the position
			transform.position = new Vector3(transform.position.x - moveVector.x * speed * Time.deltaTime,
			                                 transform.position.y - moveVector.y * speed * Time.deltaTime,
			                                 transform.position.z - moveVector.z * speed * Time.deltaTime);
			                                 
			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z)) < 75) {
				runningStartAnimation = false;
				transform.rotation = Quaternion.Euler(new Vector3(20, 180, 0));
			}
			
			return;  // don't do any of the normal positioning until we finish the inital pan in
		}
		
		
		transform.position = new Vector3(pirateHero.transform.position.x + 3, pirateHero.transform.position.y + 23, pirateHero.transform.position.z + 37);
	}
}

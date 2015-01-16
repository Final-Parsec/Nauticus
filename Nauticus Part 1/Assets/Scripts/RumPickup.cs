using UnityEngine;
using System.Collections;

public class RumPickup : MonoBehaviour {
	public float yDeltaForFloat = 1.25f;
	public float changePerFrame = .035f;
	
	private PlayerControl piroThePirateHero;
	private float originalY;
	private bool goingDown;
	private bool beenPickedUp = false;
	
	void ChangeElevation() {
		if (goingDown) {
			if ((originalY - transform.position.y) < yDeltaForFloat) {
				transform.position = new Vector3(transform.position.x, transform.position.y-changePerFrame, transform.position.z);
			} else {
				goingDown = false;
			}
		} else {
			if ((transform.position.y - originalY) < yDeltaForFloat) {
				transform.position = new Vector3(transform.position.x, transform.position.y+changePerFrame, transform.position.z);
			} else {
				goingDown = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y-100f, transform.position.z);
	
		piroThePirateHero.RumBottles += 1;
	}

	// Use this for initialization
	void Start () {
		originalY = transform.position.y;
		piroThePirateHero = GameObject.Find ("pirate_hero").GetComponent<PlayerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!beenPickedUp) {
			ChangeElevation();
		}		
		
		if (!piroThePirateHero){
			piroThePirateHero = GameObject.Find ("pirate_hero").GetComponent<PlayerControl>();
		}
	}	
}
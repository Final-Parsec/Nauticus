using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	PlayerControl piroThePirateHero;	

	// Use this for initialization
	void Start () {
		piroThePirateHero = GameObject.Find("pirate_hero").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void OnGUI () {		
		if (piroThePirateHero.IsInGame) {
			string healthText = ("Piro's Health: " + piroThePirateHero.Health);
			GUI.Label (new Rect (20,40,200,20), healthText);
			
			string rumBottlesText = ("Rum: " + piroThePirateHero.RumBottles);
			GUI.Label (new Rect (20,70,200,20), rumBottlesText);
		}
	}
}

/* Using GUIContent to display an image, a string, and a tooltip */

using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public Texture2D icon;
	PirateHero _Piro;
	int health;
	string popupText;
	bool popupVisible;

	void Start(){
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
	}

	void OnGUI () {
		//_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		//Debug.Log (GetComponent<Toolbox> ().inv);
		if (_Piro != null && _Piro.toolbox.inv != null && _Piro.toolbox.inv.equippedWeapon != null) {
			GUI.Button (new Rect (10, 10, 100, 20), new GUIContent ("Health: " + _Piro.health, icon, _Piro.ToString()));
			GUI.Button (new Rect (10, 30, 250, 20), new GUIContent ("Weapon: " + _Piro.toolbox.inv.equippedWeapon.name, icon, _Piro.toolbox.inv.equippedWeapon.ToString()));
			GUI.Button (new Rect (10, 50, 250, 20), new GUIContent ("Armor: " + _Piro.toolbox.inv.equippedArmor.name, icon, _Piro.toolbox.inv.equippedArmor.ToString()));
			GUI.Button (new Rect (150, 10, 100, 20), new GUIContent ("SCURVY: " + _Piro.scurvy, icon, "Don't let it get to " + _Piro.maxScurvy + "!"));
		}
		GUI.Label (new Rect (20, 70, 200, 300), GUI.tooltip);

		if (_Piro && _Piro.crateItem != null) {
			if (_Piro.onCrate)
				GUI.Label (new Rect (50, 250, 250, 200), _Piro.crateItem.ToString());
		}
	}

	public void showPopup(string text){
		popupText = text;
		popupVisible = true;
	}

	public void unshowPupup(){
		popupVisible = false;
	}
}
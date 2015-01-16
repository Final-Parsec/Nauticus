using UnityEngine;
using System.Collections;

public class ItemCrate : MonoBehaviour {
	
	PirateHero _Piro;
	Item contents;
	public int x_pos;
	public int y_pos;
	armorList arrrmor;
	weaponList weapons;

	// Use this for initialization
	void Start () {
		arrrmor = new armorList();
		weapons = new weaponList ();
		_Piro = GameObject.Find("TileMap").GetComponent<TileMap>().Piro.GetComponent<PirateHero>();
		generate_contents ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_Piro && _Piro.x_pos == x_pos && _Piro.y_pos == y_pos)
			_Piro.isOnCrate (contents, this);  //set piro's current crate to this object

	}

	public void generate_contents(){
		if (Random.Range (1, 5) == 1)//20% chance for food/rum
			if(Random.Range (1,2) == 1)
						contents = new Apple ();
				else
						contents = new Rum ();
			else if(Random.Range (1,2) == 1){
				contents = getAppropriateEquip();
		}
	}
	
	public Item getAppropriateEquip(){
		Item tempItem;
		//lvl 1
		if (Application.loadedLevelName == "Game") {
			if (Random.Range (0, 2) == 1){
				tempItem = arrrmor.levelOne [Random.Range (0, arrrmor.levelOne.Count - 1)];
				while(tempItem.classRestriction != _Piro.charClass && tempItem.classRestriction != "None"){
					tempItem = arrrmor.levelOne [Random.Range (0, arrrmor.levelOne.Count - 1)];
				}
			}
			else{
				tempItem = weapons.levelOne [Random.Range (0, weapons.levelOne.Count - 1)];
				while(!(tempItem.classRestriction == _Piro.charClass || tempItem.classRestriction == "None")){
					tempItem = weapons.levelOne [Random.Range (0, weapons.levelOne.Count - 1)];
				}
			}
		}
		//lvl 2
		else if(Application.loadedLevelName == "Game2"){
			if (Random.Range (0, 2) == 1){
				tempItem = arrrmor.levelTwo [Random.Range (0, arrrmor.levelTwo.Count - 1)];
				while(tempItem.classRestriction != _Piro.charClass && tempItem.classRestriction != "None")
					tempItem = arrrmor.levelTwo [Random.Range (0, arrrmor.levelTwo.Count - 1)];
			}
			else{
				tempItem = weapons.levelTwo [Random.Range (0, weapons.levelTwo.Count - 1)];
				while(tempItem.classRestriction != _Piro.charClass && tempItem.classRestriction != "None")
					tempItem = weapons.levelTwo [Random.Range (0, weapons.levelTwo.Count - 1)];
			}
		}
		//else it's lvl 3
		else{
			if (Random.Range (0, 2) == 1){
				tempItem = arrrmor.levelThree [Random.Range (0, arrrmor.levelThree.Count - 1)];
				while(tempItem.classRestriction != _Piro.charClass && tempItem.classRestriction != "None")
					tempItem = arrrmor.levelThree [Random.Range (0, arrrmor.levelThree.Count - 1)];
			}
			else{
				tempItem = weapons.levelThree [Random.Range (0, weapons.levelThree.Count - 1)];
				while(tempItem.classRestriction != _Piro.charClass && tempItem.classRestriction != "None")
					tempItem = weapons.levelThree [Random.Range (0, weapons.levelThree.Count - 1)];
			}
			Debug.Log (tempItem.name);
			return tempItem;
		}
		return tempItem;
	}


	public void killyoself(){
		Destroy (gameObject);
	}
}

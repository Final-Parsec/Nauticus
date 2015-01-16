using UnityEngine;
using System.Collections;

public class Menu_Rapscallion : MonoBehaviour {
	
	Class_Menu_Script _classMenu;
	
	// Use this for initialization
	void Start () {
		_classMenu = GameObject.Find("Nauticus Act 2(1)").GetComponent<Class_Menu_Script>();
	}
	
	/// <summary>
	/// Raises the mouse enter event.
	/// </summary>
	void OnMouseEnter(){
		renderer.material.color = Color.red;
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit(){
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp(){
		_classMenu.rapscallionSelect ();
	}
}
using UnityEngine;
using System.Collections;

public class Menu_Buccaneer : MonoBehaviour {

	Class_Menu_Script _classMenu;

	// Use this for initialization
	void Start () {
		_classMenu = GameObject.Find("Nauticus Act 2(1)").GetComponent<Class_Menu_Script>();
	}

	/// <summary>
	/// Raises the mouse enter event.
	/// </summary>
	void OnMouseEnter(){
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit(){
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseUp(){
		_classMenu.buccaneerSelect ();
		}
}
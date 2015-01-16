using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {
	
	Menu_Camera_Logic _camera;
	Menu_Logic _mlogic;

	int menuNum;

	public bool isQuit = false;

	void OnMouseEnter(){
		renderer.material.color = Color.red;
	}

	// Use this for initialization
	void Start () {
		menuNum = 0;
		_camera = GameObject.Find("Main Camera").GetComponent<Menu_Camera_Logic>();
		_mlogic = GameObject.Find ("Main Camera").GetComponent<Menu_Logic> ();
		_mlogic.engageCameraSlide (menuNum);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			activateNextMenu();
		}
	}

	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit(){
		renderer.material.color = Color.white;
	}

	/// <summary>
	/// Activates the next menu.
	/// </summary>
	void activateNextMenu(){
		_mlogic.engageCameraSlide (++menuNum);
	}

	void OnMouseDown(){
		
	}
}
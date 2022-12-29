using UnityEngine;
using System.Collections;

public class Menu_Logic : MonoBehaviour {
	Menu_Camera_Logic _camera;
	public AudioSource pirateHero;

	// Use this for initialization
	void Start () {
		_camera = GameObject.Find("Main Camera").GetComponent<Menu_Camera_Logic>();
		engageCameraSlide (0);
		pirateHero = GetComponent<AudioSource> ();
		GetComponent<AudioSource>().Play ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	/// <summary>
	/// Engages the camera slide and activates the next menu
	/// </summary>
	/// <param name="menuNum">Menu number.</param>
	public void engageCameraSlide(int menuNum){
		if (_camera) {
			if (menuNum == 0) { 
					_camera.setTargetPosition (new Vector3 (0, 15, -100));
					_camera.setTargetRotation (new Quaternion (0.1f, 0, 0, 1.0f));
			}
			if (menuNum == 1) { 
					_camera.setTargetPosition (new Vector3 (-225, 17, 10));
					_camera.setTargetRotation (new Quaternion (0, 0.7f, 0, -0.7f));
			}
		}
	}
}

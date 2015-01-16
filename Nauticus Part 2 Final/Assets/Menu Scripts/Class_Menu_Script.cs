using UnityEngine;
using System.Collections;

public class Class_Menu_Script : MonoBehaviour {
	
	Toolbox toolbox;

	void awake(){
	}

	// Use this for initialization
	void Start () {
		toolbox = Toolbox.Instance;
//		_buccaneer = GameObject.Find ("Buccaneer");
//		_mlogic = GameObject.Find ("Picaroon");
//		_camera = GameObject.Find ("Rapscallion");
//		_mlogic = GameObject.Find ("Swashbuckler");
//		_mlogic.engageCameraSlide (menuNum);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void buccaneerSelect(){
		toolbox.charClass = "Buccaneer";
		//toolbox.travis.charClass = "buccaneer";
		Application.LoadLevel ("Game");

		//Toolbox.travis.charClass = "Buccaneer";
		//Toolbox.travis.GetOrAddComponent<TravelClass>();
		//Debug.Log (Toolbox.travis.charClass);
	}

	public void picaroonSelect(){
		toolbox.charClass = "Picaroon";
		Application.LoadLevel ("Game");

	}

	public void rapscallionSelect(){
		toolbox.charClass = "Rapscallion";
		Application.LoadLevel ("Game");

	}

	public void swashbucklerSelect(){
		toolbox.charClass = "Swashbuckler";
		Application.LoadLevel ("Game");

	}

}
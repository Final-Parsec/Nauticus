using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/// <summary>
/// Player Light.  Purveyor of player's death.
/// </summary>
public class PlayerLight : MonoBehaviour {
	private int y_offset = 2;
	
	void Start() {
	}
	
	public PlayerLight(){
	}

	// Update is called once per frame
	void Update () {
		flicker ();
	}

	private void flicker(){
		if (Random.value > 0.9){
			GetComponent<Light>().range = (Random.value * 52) + 60;
		}
	}
	
	public void setPosition(Vector3 position){
		transform.position = new Vector3(position.x, position.y + y_offset, position.z);
	}
}
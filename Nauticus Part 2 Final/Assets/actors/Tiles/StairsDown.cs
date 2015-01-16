using UnityEngine;
using System.Collections;

public class StairsDown : MonoBehaviour{

	public int x_pos;
	public int y_pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Sets the start position.
	/// </summary>
	public void setStartPosition(int x, int y, int tileSize){
		x_pos = x;
		y_pos = y;
		transform.rotation = new Quaternion (0, 0.9f, -0.4f, 0);
		transform.position = new Vector3(x_pos * tileSize + 3, 3, y_pos * tileSize + 5);
	}
	
	public void killyoself(){
		Destroy (gameObject);
	}
}

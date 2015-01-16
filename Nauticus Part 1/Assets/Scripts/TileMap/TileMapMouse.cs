using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	
	TileMap _tileMap;
	
	Vector3 currentTileCoord;
	
	public Transform selectionCube;
	
	DateTime preDeterminedTime;
	
	void Start() {
		_tileMap = GetComponent<TileMap>();
		preDeterminedTime = DateTime.Now;	
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		var cube = selectionCube.Find ("Cube").gameObject;
		
		var beenHowLong = DateTime.Now - preDeterminedTime;
		if (beenHowLong.TotalMilliseconds > 200) {
			cube.renderer.material.color = new Color(0f, 255f, 95f, 131f);
		}
		
		if( collider.Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
			int x = Mathf.FloorToInt( hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.FloorToInt( hitInfo.point.z / _tileMap.tileSize);
			//Debug.Log ("Tile: " + x + ", " + z);
			
			currentTileCoord.x = x;
			currentTileCoord.z = z;
			
			selectionCube.transform.position = currentTileCoord*5f;
			
			if(Input.GetMouseButtonDown(0)) {
				Debug.Log(_tileMap.GetTilePosition(hitInfo.point.x, hitInfo.point.z));
				cube.renderer.material.color = new Color(255f, 0f, 0f, 131f);
				preDeterminedTime = DateTime.Now;
			}
		}
		else {
			// Hide selection cube?
		}
		
		
	}
}


using System;
using UnityEngine;
namespace Zombies
{
	public class CameraMovement : MonoBehaviour{

		int sensitivity = 50;
		int scrollSensitivity = 120;
		Vector2 cameraMinDistance;
		Vector2 cameraMaxDistance;
		int scrollMaxDistance = 40;
		int scrollMinDistance = 5;
		int mouseBorder = 20;
	
		void Start() {

			TileMap _tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();

			cameraMinDistance = new Vector2(0, -_tileMap.size_z);
			cameraMaxDistance = new Vector2(_tileMap.size_x, 0);
		}
				
		// Update is called once per frame
		void Update () {
			float theScreenWidth = Screen.width;
			float theScreenHeight = Screen.height;


			float moveRate = sensitivity * Time.deltaTime;
			float scrollRate = scrollSensitivity * Time.deltaTime;

			float deltaX = 0;
			float deltaY = 0;
			float deltaZ = 0;

			// Move the camera with the arrow keys or with the mouse.
			if ( Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y > theScreenHeight - mouseBorder){
				if (transform.position.z + moveRate < cameraMaxDistance.y)
					deltaY = moveRate;
			}
			if ( Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y < 0 + mouseBorder){
				if (transform.position.z - moveRate > cameraMinDistance.y)
					deltaY = -moveRate;
			}
			if ( Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > theScreenWidth - mouseBorder){
				if (transform.position.x + moveRate < cameraMaxDistance.x)
					deltaX = moveRate;
			}
			if ( Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < 0 + mouseBorder){
				if (transform.position.x - moveRate > cameraMinDistance.x)
					deltaX = -moveRate;
			}

			// Zoom in/out with the scroll wheel.
			if (Input.GetAxis("Mouse ScrollWheel") > 0){
				if (camera.orthographicSize - scrollRate > scrollMinDistance)
					camera.orthographicSize = camera.orthographicSize - scrollRate;
				else
					camera.orthographicSize = scrollMinDistance;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0){
				if (camera.orthographicSize + scrollRate < scrollMaxDistance)
					camera.orthographicSize = camera.orthographicSize + scrollRate;
				else
					camera.orthographicSize = scrollMaxDistance;

			}

			moveCamera(deltaX, deltaY, deltaZ);
		}

		private void moveCamera(float x, float y, float z){
			// Different coordinate standards.
			transform.position = new Vector3(transform.position.x + x, transform.position.y + z, transform.position.z + y);
		}

		public void setStartingPosition(Vector3 position){
			transform.position = new Vector3(position.x, transform.position.y, position.z);
		}

		public float GetDistanceRatio(){
			return camera.orthographicSize / scrollMaxDistance;
		}
	
	}
}


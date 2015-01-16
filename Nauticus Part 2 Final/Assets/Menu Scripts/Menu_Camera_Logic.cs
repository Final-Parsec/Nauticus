using UnityEngine;
using System.Collections;

public class Menu_Camera_Logic : MonoBehaviour {
	
	private Vector3? targetPosition;
	private Quaternion? targetRotation;
	public float speed;
	public float angularVelocity;
	
	[ExecuteInEditMode]
	// Use this for initialization
	void Start () {
		speed = 160;
		angularVelocity = .8f;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPosition.HasValue) {
			MoveTowardTargetPosition ();
			///check if destination is nigh
			if (Vector3.Distance (transform.position, new Vector3 (targetPosition.Value.x, transform.position.y, targetPosition.Value.z)) < .9) {
				transform.position = targetPosition.Value;
				targetPosition = null;
			}
		}

		if (targetRotation.HasValue) {
			MoveTowardTargetRotation();
		}

		//Vector3 tempRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
		if (targetRotation.HasValue) {
			MoveTowardTargetRotation();
//			if (Vector3.Distance (tempRotation, new Vector3 (targetRotation.Value.x, transform.rotation.y, targetRotation.Value.z)) < .9) {
//				Quaternion tempQ = new Quaternion(0f, targetRotation.Value.x, targetRotation.Value.y, targetRotation.Value.z);
//				//transform.rotation.x = targetRotation.Value.x;
//				//transform.rotation.y = targetRotation.Value.y;
//				//transform.rotation.z = targetRotation.Value.z;
//				transform.rotation = tempQ;
//				targetRotation = null;
//			}
		}
	}
	
	/// <summary>
	/// Moves the toward target position.
	/// </summary>
	void MoveTowardTargetPosition() {
		Vector3 moveVector = new Vector3(transform.position.x - targetPosition.Value.x,
		                                 transform.position.y - targetPosition.Value.y,
		                                 transform.position.z - targetPosition.Value.z).normalized;
		// update the position
		transform.position = new Vector3(transform.position.x - moveVector.x * speed * Time.deltaTime,
		                                 transform.position.y - moveVector.y * speed * Time.deltaTime,
		                                 transform.position.z - moveVector.z * speed * Time.deltaTime);
	}

	void MoveTowardTargetRotation() {
//		Vector3 rotationVector = new Vector3(transform.rotation.x - targetRotation.Value.x,
//		                                 transform.rotation.y - targetRotation.Value.y,
//		                                 transform.rotation.z - targetRotation.Value.z).normalized;
//		// update the position
//		Debug.Log ("rotate");
//		transform.rotation = new Quaternion(0f,
//										 transform.rotation.x - rotationVector.x * speed * Time.deltaTime,
//		                                 transform.rotation.y - rotationVector.y * speed * Time.deltaTime,
//		                                 transform.rotation.z - rotationVector.z * speed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation.Value, Time.deltaTime * angularVelocity);

	}
	
	/// <summary>
	/// Sets the target position.
	/// </summary>
	/// <param name="tp">Tp.</param>
	public void setTargetPosition(Vector3 tp){
		targetPosition = tp;
		//Debug.Log (targetPosition.Value.y.ToString());
	}

	public void setTargetRotation(Quaternion tr){
		targetRotation = tr;
	}
	
}
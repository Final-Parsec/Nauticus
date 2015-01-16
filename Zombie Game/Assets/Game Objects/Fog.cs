using UnityEngine;
using System.Collections;

namespace Zombies{
	public class Fog : GameObjectBase {
		public GameObjectBase clearingObject = null;

		// Use this for initialization
		void Start () {
			renderer = GetComponent<Renderer>();
		}
		
		// Update is called once per frame
		void Update () {
			renderer.enabled = false;

//			if(onTile.reservedByUnit!=null){
//				renderer.enabled = true;
//			}else{
//				renderer.enabled = false;
//			}

//			if(!onTile.isWalkable){
//				renderer.enabled = true;
//			}else{
//				renderer.enabled = false;
//			}

			if(clearingObject != null){
				if (Vector3.Distance(clearingObject.onTile.position, transform.position) <= clearingObject.sightRange){
					renderer.enabled = false;
				}else{
					clearingObject = null;
					renderer.enabled = true;
				}
			}else{
				renderer.enabled = true;
			}
		}
	}
}

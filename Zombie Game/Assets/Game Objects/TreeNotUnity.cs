using UnityEngine;
using System.Collections;

namespace Zombies{
	public class TreeNotUnity : GameObjectBase {

		public int wood = 15;


		// Use this for initialization
		void Start () {
			animator = GetComponent<Animator>();
			renderer = GetComponent<Renderer>();
			animator.SetInteger("Tree Type", Random.Range(1,5));

			transform.position = new Vector3(transform.position.x,
			                                 -1 * (transform.position.z/_UnitGod._eventHandler._tileMap.size_z),
			                                 transform.position.z);
		}
		
		// Update is called once per frame
		void Update () {
			if(wood <= 0){
				KillSelf();
			}

			if(onTile.gatherer == null || onTile != onTile.gatherer.gatheringFromTile)
				onTile.gatherer = null;
		}

		public int HarvestWood(int amount){
			wood = wood - amount;
			if(wood < 0)
				return amount - wood;
			return amount;
		}

		/// <summary>
		/// Kills the self.
		/// </summary>
		new public void KillSelf(){
			_UnitGod.DeReference(this);
			onTile.description = "Grass";
			Destroy(gameObject);
		}
	}
}

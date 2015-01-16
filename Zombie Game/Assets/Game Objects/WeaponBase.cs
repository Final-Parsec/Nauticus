using UnityEngine;
using System.Collections.Generic;


namespace Zombies
{
	public class WeaponBase : GameObjectBase {

		public UnitBase owner = null;

		// Attack attr
		public int attack = 5;
		public float attackDelay = .5f; // In seconds
		public float lastAttackTime = 0f;
		public int attackRange = 4;
		public float animationCancelDelta = .2f; // In seconds
		//public float animationCancelExperation = 0f;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public int GetAttack(){

			return attack;
		}
	}
}

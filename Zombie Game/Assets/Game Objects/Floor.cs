using UnityEngine;
using System.Collections;

namespace Zombies{
	public class Floor : BuildingParts {
		// Use this for initialization
		void Start () {
			_UnitGod.AddFloor((Floor)this);
			animator = GetComponent<Animator>();

		}

	}
}

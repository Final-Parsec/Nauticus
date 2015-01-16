using UnityEngine;
using System.Collections;

namespace Zombies{
	public class Wall : BuildingParts{

		// Use this for initialization
		void Start () {
			_UnitGod.AddWall((Wall)this);
			animator = GetComponent<Animator>();
			SetUp();
		}

	}
}
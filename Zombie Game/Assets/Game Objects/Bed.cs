using UnityEngine;
using System.Collections;

namespace Zombies{
	public class Bed : BuildingParts {
		// Use this for initialization
		void Start () {
			_UnitGod.AddBed((Bed)this);

		}

		void Awake(){
			_UnitGod = UnitGod.GetInstance();
			setPosition(_UnitGod.GetTileFromLocation(transform.position));
		}
	}
}

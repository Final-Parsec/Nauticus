using UnityEngine;
using System.Collections;

namespace Zombies{
	public class House : BuildingBase {
		public float lastHealTime = 0;
		public float healDelay = 1;
		public int healRate = 3; // rate of health regen in seconds 

		// Use this for initialization
		void Start () {
			_UnitGod.AddHouse((House)this);
			healthBarWidth = 200;

			PopulatePartsOf(this);
			SetUp();
		}

		void Awake(){
			_UnitGod = UnitGod.GetInstance();
			setPosition(_UnitGod.GetTileFromLocation(transform.position));
			width = _UnitGod._eventHandler._hoverHouse.sizeX;
			height = _UnitGod._eventHandler._hoverHouse.sizeZ;
		}
		
	}
}
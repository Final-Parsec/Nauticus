using UnityEngine;
using System.Collections;

namespace Zombies{
	public class Hut : BuildingBase {

		// Use this for initialization
		void Start () {
			_UnitGod.AddHut((Hut)this);
			healthBarWidth = 200;

			PopulatePartsOf(this);
			SetUp();
		}

		void Awake(){
			_UnitGod = UnitGod.GetInstance();
			setPosition(_UnitGod.GetTileFromLocation(transform.position));
			width = _UnitGod._eventHandler._hoverHut.sizeX;
			height = _UnitGod._eventHandler._hoverHut.sizeZ;
		}

	}
}

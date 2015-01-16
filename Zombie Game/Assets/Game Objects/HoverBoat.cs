using UnityEngine;
using System.Collections;

namespace Zombies{
	public class HoverBoat : HoverObject {

		// Use this for initialization
		void Start () {
			_eventHandler = GameObject.Find("TileMap").GetComponent<EventHandler>();
		}
		
		/// <summary>
		/// Determines whether the object can be built can build.
		/// </summary>
		/// <returns><c>true</c> if this instance can build; otherwise, <c>false</c>.</returns>
		public override bool CanBuild(){
			if (_eventHandler._hud.woodResourse < woodCost)
				return false;
			
			int floorX = Mathf.FloorToInt((sizeX) / 2.0f);
			int floorZ = Mathf.FloorToInt((sizeZ) / 2.0f);
			int ceilX = Mathf.CeilToInt(sizeX / 2.0f);
			int ceilZ = Mathf.CeilToInt(sizeZ / 2.0f);
			
			Vector2 vec1 = new Vector2(transform.position.x - floorX - .5f, transform.position.z - floorZ - .5f);
			Vector2 vec2 = new Vector2(transform.position.x + ceilX - .5f, transform.position.z + ceilZ - .5f);
			Rect rect = _eventHandler.getRect(vec1, vec2);
			
			Vector3 vec3 = _eventHandler._tileMap.toTileMapCoordnates(new Vector3(rect.x, 0, rect.y));
			WorldTile currentTile;
			for (int x = (int)vec3.x; x < vec3.x + rect.width; x++){
				for (int z = (int)vec3.z - 1; z >= vec3.z - rect.height; z--){
					currentTile = _eventHandler._tileMap.world[x,z];
					if (!currentTile.hasWater())
						return false;
				}
			}
			
			return true;
			
		}
	}
}

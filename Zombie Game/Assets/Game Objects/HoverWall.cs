using UnityEngine;
using System.Collections;

namespace Zombies{
	public class HoverWall : HoverObject {

		// Use this for initialization
		void Start () {
			_eventHandler = GameObject.Find("TileMap").GetComponent<EventHandler>();
		}

		public override bool CanBuild(){
			if (_eventHandler._hud.woodResourse < woodCost * GetBiggestSide())
				return false;

			Vector3 start = GetStartCoordinate();
			WorldTile currentTile;


			for (int x = (int)start.x; x < start.x + sizeX; x++){
				for (int z = (int)start.z; z > start.z - sizeZ; z--){
					currentTile = _eventHandler._tileMap.world[x,z];
					if (!currentTile.isBuildable)
						return false;
				}
			}

			UseResourse();

			for (int x = (int)start.x; x < start.x + sizeX; x++){
				for (int z = (int)start.z; z > start.z - sizeZ; z--){
					currentTile = _eventHandler._tileMap.world[x, z];
					Instantiate(prefab, currentTile.position, Quaternion.Euler(new Vector3(90, 0, 0)));
				}
			}
			
			return true;
			
		}

		public Vector3 GetStartCoordinate(){
			Rect rect = _eventHandler.getRect(new Vector2(_eventHandler.startWallPosition.x, _eventHandler.startWallPosition.z),
			                                  new Vector2(_eventHandler.tileCoord.x, _eventHandler.tileCoord.z));
			Debug.Log (_eventHandler.startWallPosition+", "+_eventHandler.tileCoord);
			sizeX = (int)rect.width + 1;
			sizeZ = (int)rect.height + 1;
			
			Vector3 start;
			if(sizeX < sizeZ){
				start = new Vector3(transform.position.x - .5f, 0, rect.y);
				sizeX = 1;
			}else{
				start = _eventHandler._tileMap.toTileMapCoordnates(new Vector3(rect.x, 0, transform.position.z - .5f));
				sizeZ = 1;

			}

			return start;
		}

		public override void UseResourse(){
			_eventHandler._hud.woodResourse -= woodCost * GetBiggestSide();

		}

		public int GetBiggestSide(){
			if (sizeZ > sizeX)
				return sizeZ;
			else
				return sizeX;
		}

	}
}

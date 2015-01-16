using UnityEngine;
using System;
using System.Collections.Generic;
namespace Zombies
{
	public class GameObjectBase : MonoBehaviour
	{
		public int sightRange = 0;

		protected UnitGod _UnitGod = null;
		protected new Renderer renderer;
		protected Animator animator = null;
		public WorldTile onTile = null;
		protected SpriteRenderer spriteRenderer;

		// Unit attributes
		public int maxHealth = 100;
		public int health = 100;
		public int healthBarWidth = 40;

		void Awake(){
			_UnitGod = UnitGod.GetInstance();
			setPosition(_UnitGod.GetTileFromLocation(transform.position));

		}

		protected void SetUp(){
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void setPosition(WorldTile location){
			transform.position = new Vector3(location.position.x, transform.position.y, location.position.z);
			onTile = location;
			location.addOccupant((GameObjectBase)this);
		}

		public void TurnOffAnimationInFog(){
			if (onTile.getFog().renderer.enabled){
				animator.enabled = false;
			}else{
				animator.enabled = true;
			}
		}

		public void TurnOffRendererInFog(){
			if (onTile.getFog().renderer.enabled){
				renderer.enabled = false;
			}else{
				renderer.enabled = true;
			}
		}

		/// <summary>
		/// Clears the fog.
		/// </summary>
		public void ClearFog(){

			Vector3 startCorner = new Vector3(Mathf.FloorToInt( transform.position.x - sightRange), 0, Mathf.FloorToInt( transform.position.z + sightRange));
			startCorner = _UnitGod._tileMap.toTileMapCoordnates(startCorner);
			if(startCorner.x < 0)
				startCorner.x = 0;
			if(startCorner.z >= _UnitGod._tileMap.size_z)
				startCorner.z = _UnitGod._tileMap.size_z - 1;

			for(int x=(int)startCorner.x; x<=startCorner.x + sightRange*2+1; x++){
				if(x >= _UnitGod._tileMap.size_x)
					break;
				for (int z=(int)startCorner.z; z>=startCorner.z - sightRange*2-1; z--){
					if(z < 0)
						break;


					WorldTile currentTile = _UnitGod._tileMap.world[x,z];
					Fog fog = currentTile.getFog();
					if (Vector3.Distance(onTile.position, fog.transform.position) <= sightRange)
						fog.clearingObject = this;

				}
			}

		}

		/// <summary>
		/// Damage done to this instance by the attacker.
		/// </summary>
		/// <param name="target">Target.</param>
		public void Damage(UnitBase attacker){
			if(this is BuildingParts && ((BuildingParts)this).partOf != null){
				BuildingParts instance = (BuildingParts)this;
				instance.partOf.health = instance.partOf.health - attacker.GetAttack();
				
				if(!instance.partOf.IsAlive()){
					instance.partOf.KillSelf();
					attacker.target = null;
				}
			}
			else{
				health = health - attacker.GetAttack();

				if(!IsAlive()){
					KillSelf();
					attacker.target = null;
				}
			}
		}
		
		public bool IsAlive(){
			if(health > 0)
				return true;
			return false;
		}

		public void KillSelf(){
			_UnitGod.DeReference(this);
			onTile.description = "Grass";
			Destroy(gameObject);
		}

		public Vector3 GetPosition(){
			return transform.position;
		}

		public virtual Vector2 GetPixelSize(){
			Vector3 start = Camera.main.WorldToScreenPoint(new Vector3(spriteRenderer.bounds.min.x, spriteRenderer.bounds.min.y, spriteRenderer.bounds.min.z));
			Vector3 end = Camera.main.WorldToScreenPoint(new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.max.y, spriteRenderer.bounds.max.z));

			int widthX = (int)(end.x - start.x);
			int widthY = (int)(end.y - start.y);

			return new Vector2(widthX, widthY);
		}

		public virtual void Heal(){
			//override this
		}
	}
}


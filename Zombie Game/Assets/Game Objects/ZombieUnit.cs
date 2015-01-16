using UnityEngine;
using System.Collections.Generic;

namespace Zombies{
	public class ZombieUnit : UnitBase {

		// Use this for initialization
		void Start () {
			_UnitGod.AddZombieUnit((ZombieUnit)this);
			animator = GetComponent<Animator>();
			renderer = GetComponent<Renderer>();
			unitId = unitID.Zombie;
			SetUp();
		}
		
		// Update is called once per frame
		void Update () {

			if(!IsAlive()){
				KillSelf();
				return;
			}

			Wander ();
			//InFog ();
			Move ();
			if(path==null)
				Attack();


		}

		/// <summary>
		/// Attack a player.
		/// </summary>
		private void Attack(){
			int range = attackRange;
			if(weapon != null)
				range = weapon.attackRange;

			if(target && Vector3.Distance(onTile.position, target.onTile.position) <= range){

				Vector3 rotation = Quaternion.LookRotation(transform.position - target.onTile.position).eulerAngles;
				
				if(rotation.y > 135 && rotation.y <= 225)
					animator.SetInteger("attackDirection", (int)attackDirection.up);
				else  if(rotation.y > 45 && rotation.y <= 135 )
					animator.SetInteger("attackDirection", (int)attackDirection.Left);
				else  if(rotation.y > 225 && rotation.y <= 315 )
					animator.SetInteger("attackDirection", (int)attackDirection.Right);
				else 
					animator.SetInteger("attackDirection", (int)attackDirection.down);
				
				SetState((int)state.Attacking);

				// If it is ready to attack
				if(doTheDamage && Time.time > animationCancelExperation){
					target.Damage(this);
					lastAttackTime = Time.time;
					doTheDamage = false;
					return;
				}

				if(Time.time > animationCancelExperation){
					//Do the damage time
					doTheDamage = true;
					animationCancelExperation = Time.time + animationCancelDelta;
					StopMoving();
					return;
				}
			}else{
				SetState((int)state.Walking);
			}
		}

		/// <summary>
		/// Turns off the renderer if the unit is in the fog.
		/// </summary>
		private void InFog(){
			if (onTile.hasFog ()) 
				renderer.enabled = false;
			else
				renderer.enabled = true;
		}

		 /// <summary>
		/// Wander the map.
		/// </summary>
		private void Wander(){
			FindTarget();
			//go towards the target
			if(target != null){
				if (target.onTile.BorderTilesContains(onTile)){
					StopMoving();
					return;
				}

				int closestDistance = int.MaxValue;
				WorldTile closestTile = null;
				foreach(WorldTile tile in onTile.borderTiles){
					if(tile != null){
						int currentCost = _UnitGod._eventHandler.heuristic_cost_estimate(tile, target.onTile);
						if(currentCost < closestDistance){
							closestDistance = currentCost;
							closestTile = tile;
						}
					}
				}

				Wall wall = closestTile.GetWall();
				if (wall != null){
					target = wall;
				}else{
					List<WorldTile> pathTemp = new List<WorldTile>();
					pathTemp.Add(closestTile);
					StopMoving();
					SetPath(pathTemp);
				}

			//go in a random direction
			}else if(path == null){ 

				List<WorldTile> validTiles = new List<WorldTile>();

				foreach(WorldTile tile in onTile.borderTiles){
					if (tile != null && tile.isWalkable)
						validTiles.Add(tile);
				}
				//List<WorldTile> newpath = _UnitGod._eventHandler.Astar (onTile, validTiles[Random.Range(0, validTiles.Count)]);
				if (validTiles.Count!=0)
					_UnitGod._eventHandler.moveTheUnit(validTiles[Random.Range(0, validTiles.Count)], this);
				
			}
		}

		/// <summary>
		/// Determins if the Zed found a target.
		/// </summary>
		private void FindTarget(){
			GameObjectBase newTarget = null;
			float closestDistance = float.MaxValue;
			foreach(PlayerUnit playerUnit in _UnitGod.playerUnits){
				float thisDistance = Vector3.Distance(onTile.position, playerUnit.onTile.position);
				if (thisDistance <= sightRange && thisDistance < closestDistance){
					newTarget = playerUnit;
					closestDistance = thisDistance;
				}
			}

			closestDistance = float.MaxValue;
			// if null, find a wall to hit
			if(newTarget == null){
				foreach(Wall wall in _UnitGod.walls){
					float thisDistance = Vector3.Distance(onTile.position, wall.onTile.position);
					if (thisDistance <= sightRange && thisDistance < closestDistance){
						newTarget = wall;
						closestDistance = thisDistance;
					}
				}
			}

			target = newTarget;
		}

		new public void Damage(UnitBase attacker){
			this.target = attacker;
			health = health - attacker.GetAttack();

			if(!IsAlive()){
				KillSelf();
				attacker.target = null;
			}
		}
	}
}

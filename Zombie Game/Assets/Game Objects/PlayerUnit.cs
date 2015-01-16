using UnityEngine;
using System.Collections.Generic;

namespace Zombies{
	public class PlayerUnit : UnitBase {


		private LineRenderer lineRenderer;
	
		// Gathering variables
		public gatheringTypes gatheringState = gatheringTypes.NotGathering;
		public float woodCuttingSpeed = 5; //time in seconds
		public int woodStorageSize = 6;
		public bool hasWood = false;
		public WorldTile gatheringFromTile = null;
		private float gatheringStartTime = float.MaxValue;
		private float gatheringSoundTime = 0;

		public bool isSelected = false;

//		private GameObject healthBar;
		public Texture healthTexture;

		// Use this for initialization
		void Start () {
			SetUp();

			_UnitGod.AddPlayerUnit((PlayerUnit)this);
			lineRenderer = GetComponent<LineRenderer>();
			unitId = unitID.Player;
		}
		
		// Update is called once per frame
		void Update () {
			if(!IsAlive()){
				KillSelf();
				return;
			}
			if(gatheringState == gatheringTypes.NotGathering && path == null)
				Attack();
			Move();
			//ClearFog();
			if(IsGathering())
				if(GatherDeleyHasPassed())
					GatherResourse();
			if(gatheringState == gatheringTypes.DoneGathering)
				ReturnResourse();
			Heal();
		}

		/// <summary>
		/// Attack a target.
		/// </summary>
		private void Attack(){
			FindTarget();

			if(target != null){
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
			}else{
				SetState((int)state.Walking);
				return;
			}

			// If it is ready to attack
			if(doTheDamage && target != null && Time.time > animationCancelExperation){
				_UnitGod.PlayAttackSound(GetUnitID(), audioSource);
				target.Damage(this);
				lastAttackTime = Time.time;
				doTheDamage = false;
				return;
			}

			if(Time.time > animationCancelExperation){
				int range = attackRange;
				if(weapon != null)
					range = weapon.attackRange;
				
				if(target && Vector3.Distance(onTile.position, target.onTile.position) <= range){
					//Do the damage time
					doTheDamage = true;
					animationCancelExperation = Time.time + animationCancelDelta;
					StopMoving();
					return;
				}
			}


			
		}

		/// <summary>
		/// Determins if the unit found a target.
		/// </summary>
		/// <returns>The target if found.</returns>
		private void FindTarget(){
			if(target != null && Vector3.Distance(onTile.position, target.onTile.position) <= sightRange)
				return;
			target = null;

			foreach(ZombieUnit zed in _UnitGod.zombieUnits){
				if (Vector3.Distance(onTile.position, zed.onTile.position) <= sightRange){
					target = zed;
					return;
				}
			}
		}

		// select the unit by changing it's sorting layer
		public void Select(){
			isSelected = true;
			this.gameObject.layer = (int)sortingLayer.Selected;
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, 0);
		}

		// deselect the unit by changing it's sorting layer
		public void Deselect(){
			isSelected = false;
			this.gameObject.layer = (int)sortingLayer.NotSelected;
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, 255);
		}



		public void DrawPath(){

			if(isSelected){
				lineRenderer.SetVertexCount(currentWayPoint + 1);
				for (int x=currentWayPoint; x>=0; x--)
					lineRenderer.SetPosition(x, path[x].position);

			}else{
				lineRenderer.SetVertexCount(0);
			}

		}


		// called in update
		// move the unit closer to the next tile in it's path.
		new public void Move(){
			if(path == null)
				return;
			StopAttacking();
			SetState((int)state.Walking);

			// don't move in the Y direction.
			Vector3 moveVector = new Vector3(transform.position.x - path[currentWayPoint].position.x,
			                                 0,
			                                 transform.position.z - path[currentWayPoint].position.z).normalized;

			// update the position
			transform.position = new Vector3(transform.position.x - moveVector.x * speed * Time.deltaTime,
			                                 transform.position.y,
			                                 transform.position.z - moveVector.z * speed * Time.deltaTime);

			// unit has reached the waypoint
			if(Vector3.Distance( transform.position, path[currentWayPoint].position ) < minWaypointDisplacement){
				onTile.removeOccupant(this);
				path[currentWayPoint].addOccupant(this);
				onTile = path[currentWayPoint];

				// if you are at the end of the path, clear the path.
				currentWayPoint--;
				DrawPath();
				if( currentWayPoint < 0 ){


					StopMoving();
					if(gatheringState != gatheringTypes.NotGathering){
						if(gatheringState == gatheringTypes.Returning){
							AddResources();
						}else if(NextToResource()){


							//TODO: tell player they are gathering
						}else if(!FindRecource()){
							gatheringState = gatheringTypes.NotGathering;
							Debug.Log("not near a tree");
							//TODO: tell player the Resourse is not there.
						}
					}

					return;
				}

				animator.SetInteger("walking", path[currentWayPoint].GetDirection(onTile));

				// Look in the direction the unit is moving.
//				var newRotation = Quaternion.LookRotation(transform.position - path[currentWayPoint].position).eulerAngles;
//				newRotation.x = transform.eulerAngles.x;
//				newRotation.y = newRotation.y - 180;
//				newRotation.z = transform.eulerAngles.z;
//				transform.rotation = Quaternion.Euler(newRotation);
			}
		}

		private bool NextToResource(){
			foreach(WorldTile tile in onTile.borderTiles){
				if (tile != null && gatheringState == gatheringTypes.TravelingToWood && tile.HasTree() && tile.gatherer == null){
					gatheringFromTile = tile;
					tile.gatherer = this;
					gatheringStartTime = UnityEngine.Time.time;
					gatheringState = gatheringTypes.GatheringWood;
					return true;
				}

			}
			return false;
		}

		private bool FindRecource(){
			if(gatheringState == gatheringTypes.TravelingToWood){
				WorldTile tile = SearchForTileWithTree();

				if(tile != null){
					_UnitGod._eventHandler.moveTheUnit(tile, this);
					return true;
				}
			}

			return false;
		}

		private void GatherResourse(){
			// Subtract resource from tree
			if (gatheringFromTile.HasTree()){
				gatheringFromTile.GetTree().HarvestWood(woodStorageSize);

				// Put resource in the unit
				hasWood = true;
				gatheringState = gatheringTypes.DoneGathering;
			}else{
				SearchForTileWithTree();
			}
		}

		public void ReturnResourse(){
			Hut hut = _UnitGod.FindClosestHut(this);

			if (hut != null){
				_UnitGod._eventHandler.moveTheUnit (hut.onTile, this);
				gatheringState = gatheringTypes.Returning;
			}else{
				gatheringState = gatheringTypes.NotGathering;
			}
		}

		public bool HasResources(){
			return hasWood;
		}

		public bool IsGathering(){

			return gatheringState == gatheringTypes.GatheringWood;
		}

		private bool GatherDeleyHasPassed(){
			float finishedGatheringTime = gatheringStartTime + woodCuttingSpeed;
			bool returnValue = UnityEngine.Time.time > finishedGatheringTime;

			if(UnityEngine.Time.time > gatheringSoundTime){
				_UnitGod.PlayGatheringSound(gatheringState, audioSource);
				gatheringSoundTime = UnityEngine.Time.time + Random.Range(1.7f, 2.5f);
			}
			return returnValue;
		}

		private void AddResources(){
			gatheringTypes state;
			// Else if of all resource types
			if (hasWood){
				_UnitGod._eventHandler._hud.woodResourse += woodStorageSize;
				hasWood = false;
				state = gatheringTypes.TravelingToWood;
			}else{
				state = gatheringTypes.NotGathering;
			}
			_UnitGod._eventHandler.moveTheUnit (gatheringFromTile, this);
			gatheringState = state;
		}

		new public void KillSelf(){
			_UnitGod.DeReference(this);
			Destroy(gameObject);
		}

		/// <summary>
		/// Clears the fog.
		/// </summary>
		public WorldTile SearchForTileWithTree(){
			
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
					if(currentTile.HasTree() && currentTile.gatherer == null)
						return currentTile;
				}
			}
			return null;
		}

		public override void Heal(){
			House house = onTile.GetHouse();
			float now = Time.time;
			if(house != null && house.lastHealTime + house.healDelay <= now && health != maxHealth){
				if (health + house.healRate > maxHealth)
					health = maxHealth;
				else
					health += house.healRate;
				house.lastHealTime = now;
			}
		}

	}
}

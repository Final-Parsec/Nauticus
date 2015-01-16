using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombies{
	public class UnitBase : GameObjectBase {
		public float minWaypointDisplacement = .1f;
		public int currentWayPoint = 0;
		public List<WorldTile> path = null;
		public GUITexture selectedImage;

		// Unit attributes
		public float speed = 10;

		// Attack attr
		public GameObjectBase target;
		public WeaponBase weapon = null;
		public int attack = 5;
		protected float lastAttackTime = 0f;
		public int attackRange = 5;
		public float animationCancelDelta = .5f; // In seconds
		public float animationCancelExperation = 0f;
		public bool doTheDamage = false;

		protected unitID unitId;

		public AudioSource audioSource;

		// Use this for initialization
		void Start () {
			SetUp();
		}

		new protected void SetUp(){
			animator = GetComponent<Animator>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			audioSource = GetComponent<AudioSource>();
		}
		
		// return the unit's position
		public Vector3 getPosition(){
			return transform.position;
		}

		// set the path to a list of tiles the unit will walk to
		// when this is set, the unit will begin walking the path
		public void SetPath(List<WorldTile> path){
			if(path.Count == 0)
				return;

			if(this is PlayerUnit){
				onTile.reservedByUnit = null;
				path[0].reservedByUnit = this;
			}

			this.path = path;
			// Start walk animation.
			currentWayPoint = this.path.Count - 1;

			animator.SetInteger("walking", path[currentWayPoint].GetDirection(onTile));

			// Look in the direction the unit is moving.
//			if(transform.position - path[currentWayPoint].position != Vector3.zero){
//				Vector3 newRotation = Quaternion.LookRotation(transform.position - path[currentWayPoint].position).eulerAngles;
//				newRotation.x = transform.eulerAngles.x;
//				newRotation.y = newRotation.y - 180;
//				newRotation.z = transform.eulerAngles.z;
//				transform.rotation = Quaternion.Euler(newRotation);
//			}

		}

		// called in update
		// move the unit closer to the next tile in it's path.
		public void Move(){
			if(path == null)
				return;

			StopAttacking();

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
				if( currentWayPoint < 0 ){

					StopMoving();
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

		protected void StopAttacking(){
			animationCancelExperation = Time.time + animationCancelDelta;
			animator.SetInteger("attackDirection", (int)attackDirection.notAttacking);
		}

		public void StopMoving(){
			if(this is PlayerUnit){
				onTile.reservedByUnit = this;
				if(path != null)
					path[0].reservedByUnit = null;
			}
			path = null;
			// End walk animation.
			animator.SetInteger("walking", (int)border.center);
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <returns>The texture.</returns>
		public Texture2D GetTexture(){
			Debug.Log(spriteRenderer.sprite.name);
			Debug.Log(spriteRenderer.sprite.textureRect);
			return spriteRenderer.sprite.texture;
		}

		public Rect GetTextureRect(){

			return spriteRenderer.sprite.textureRect;
		}

		public int GetAttack(){

			return attack;
		}

		new public void Damage(UnitBase attacker){
			health = health - attacker.GetAttack();

			if(!IsAlive()){
				KillSelf();
				attacker.target = null;
			}
		}

		public unitID GetUnitID(){
			return unitId;
		}

		public void SetState(int stateId){
			if(animator.GetInteger("currentState") != stateId){
				animator.SetTrigger("resetState");
				animator.SetInteger("currentState",stateId);
			}
		}



	}
}

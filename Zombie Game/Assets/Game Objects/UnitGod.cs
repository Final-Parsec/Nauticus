// maintains a list of units in the game

using System;
using System.Collections.Generic;
using UnityEngine;
namespace Zombies
{
	public class UnitGod
	{
		public TileMap _tileMap;
		public EventHandler _eventHandler;
		// Singlton instance of this class
		public static UnitGod unitGod;
		public List<PlayerUnit> playerUnits = new List<PlayerUnit>();
		public List<ZombieUnit> zombieUnits = new List<ZombieUnit>();
		public List<Wall> walls = new List<Wall>();
		List<Floor> floors = new List<Floor>();
		List<Bed> beds = new List<Bed>();
		List<Hut> huts = new List<Hut>();
		List<House> houses = new List<House>();
		private Dictionary<unitID, AudioClip> selectionSounds = new Dictionary<unitID, AudioClip>();
		private Dictionary<unitID, AudioClip> attackSounds = new Dictionary<unitID, AudioClip>();
		private Dictionary<gatheringTypes, AudioClip> gatheringSounds = new Dictionary<gatheringTypes, AudioClip>();

		public bool Victory = false;

		public UnitGod (){
			_tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
			_eventHandler = GameObject.Find("TileMap").GetComponent<EventHandler>();
			unitGod = this;
			selectionSounds.Add (unitID.Player, _eventHandler.audioClips[0]);
			attackSounds.Add (unitID.Player, _eventHandler.audioClips[1]);
			gatheringSounds.Add (gatheringTypes.GatheringWood, _eventHandler.audioClips[2]);

		}

		/// <summary>
		/// Gets the tile from location.
		/// </summary>
		public WorldTile GetTileFromLocation(Vector3 location){
			Vector3 tileMapVector = _tileMap.toTileMapCoordnates(new Vector3((float)Math.Floor(location.x), 0, (float)Math.Floor(location.z)));
			return _tileMap.world[(int)tileMapVector.x, (int)tileMapVector.z];
			
		}

		// returns a singlton instance of this class
		public static UnitGod GetInstance(){
			if (unitGod == null)
				return new UnitGod();
			else
				return unitGod;
		}

		// adds a unit to the 
		public void AddPlayerUnit(PlayerUnit unit){
			if (!playerUnits.Contains(unit))
				playerUnits.Add(unit);
		}

		// adds a unit to the 
		public void AddZombieUnit(ZombieUnit unit){
			if (!zombieUnits.Contains(unit))
				zombieUnits.Add(unit);
		}


		/// <summary>
		/// Adds the wall.
		/// </summary>
		/// <param name="unit">Wall.</param>
		public void AddWall(Wall wall){
			if (!walls.Contains(wall))
				walls.Add(wall);
		}

		public void AddBed(Bed bed){
			if (!beds.Contains(bed))
				beds.Add(bed);
		}

		public void AddFloor(Floor floor){
			if (!floors.Contains(floor))
				floors.Add(floor);
		}

		public void AddHut(Hut hut){
			if (!huts.Contains(hut))
				huts.Add(hut);
		}

		public void AddHouse(House house){
			if (!houses.Contains(house))
				houses.Add(house);
		}

		/// <summary>
		/// Deselects all units.
		/// </summary>
		public void deselectAllUnits(){
			foreach(PlayerUnit unit in playerUnits){
				if (unit.isSelected)
					unit.Deselect();
			}
		}

		public List<UnitBase> GetSelectedUnits(){
			List<UnitBase> selectedUnits = new List<UnitBase>();

			foreach(PlayerUnit unit in playerUnits)
				if (unit.isSelected)
					selectedUnits.Add(unit);
			return selectedUnits;
		}

		public bool UnitsAreSelected(){
			foreach(PlayerUnit unit in playerUnits)
				if (unit.isSelected)
					return true;
			return false;
		}

		public void SetSelectedUnitsToGathering(gatheringTypes Resourse){
			foreach(PlayerUnit unit in GetSelectedUnits()){
				if(unit.HasResources() && Resourse != gatheringTypes.NotGathering){
					unit.gatheringState = gatheringTypes.DoneGathering;
				}else{
					unit.gatheringState = Resourse;
				}
			}
		}

		public Hut FindClosestHut(PlayerUnit unit){
			Hut closestHut = null;
			float shortestDistance = float.MaxValue;
			foreach(Hut hut in huts){
				float dist = Vector2.Distance(new Vector2(hut.onTile.position.x, hut.onTile.position.z),
				                              new Vector2(unit.onTile.position.x, unit.onTile.position.z));
				if(dist < shortestDistance){
					closestHut = hut;
					shortestDistance = dist;
				}

			}
			return closestHut;
		}

		public void DeReference(GameObjectBase unit){

			unit.onTile.removeOccupant(unit);

			if(unit is PlayerUnit){
				playerUnits.Remove((PlayerUnit)unit);
			}
			else if(unit is ZombieUnit){
				zombieUnits.Remove((ZombieUnit)unit);
			}
			else if(unit is Wall){
				walls.Remove((Wall)unit);
				unit.onTile.removeOccupant(unit);
			}
			else if(unit is House){
				houses.Remove((House)unit);
				unit.onTile.removeOccupant(unit);
			}
			else if(unit is Hut){
				huts.Remove((Hut)unit);
				unit.onTile.removeOccupant(unit);
			}


		}

		public void StopAttacks(){
			foreach(UnitBase unit in GetSelectedUnits())
				unit.doTheDamage = false;
		
		}

		public void PlaySelectionSound(unitID key){
			AudioClip clip;
			if(selectionSounds.TryGetValue(key, out clip))
				AudioSource.PlayClipAtPoint(clip, Vector3.zero, 1f * _eventHandler._hud.masterVolume);
			
		}

		public void PlayAttackSound(unitID key, AudioSource AS){
			AudioClip clip;
			if(attackSounds.TryGetValue(key, out clip)){
				AS.clip = clip;
				AS.volume =  1f * _eventHandler._hud.masterVolume;
				AS.Play();
				//AudioSource.PlayClipAtPoint(clip, location, 1f);
			}
			
		}

		public void PlayGatheringSound(gatheringTypes key, AudioSource AS){
			AudioClip clip;
			if(gatheringSounds.TryGetValue(key, out clip)){
				AS.clip = clip;
				AS.volume =  1f * _eventHandler._hud.masterVolume;
				AS.Play();
			}
		}

		public List<GameObjectBase> ThingsWithHealthBars(){
			List<GameObjectBase> returnList = new List<GameObjectBase>();

			foreach(PlayerUnit player in playerUnits)
				returnList.Add((GameObjectBase)player);

			foreach(Wall wall in walls)
				returnList.Add((GameObjectBase)wall);

			foreach(ZombieUnit zombie in zombieUnits)
				returnList.Add((GameObjectBase)zombie);

			foreach(Hut hut in huts)
				returnList.Add((BuildingBase)hut);
			
			foreach(House house in houses)
				returnList.Add((BuildingBase)house);

			return returnList;
		}
	}
}


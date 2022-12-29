using UnityEngine;
using System;
using System.Collections.Generic;
namespace Zombies
{
	public class WorldTile
	{
		public Color[] pixles;
		public bool isWalkable;
		public bool isBuildable;
		public gatheringTypes type;
		public string description = "Grass";
		// Use border enum to access tiles.
		public WorldTile[] borderTiles = new WorldTile[8];
		// Center of the box.
		public Vector3 position;
		public List<GameObjectBase> tileoccupants = new List<GameObjectBase>();

		//Dijkstra variables
		public int distance = int.MaxValue;
		public WorldTile previous = null;

		// A* variables
		public UnitBase reservedByUnit = null;
		public int gScore = int.MaxValue; // undefined 
		public int fScore = int.MaxValue; // undefined 
		public bool isInOpenSet = false;
		public bool isInClosedSet = false;
		public WorldTile parent = null;
		public String unitName = "";

		// For resource tiles
		public PlayerUnit gatherer = null;



		public WorldTile (){
		}

		public WorldTile (Color[] pixles, gatheringTypes type, bool isWalkable, bool isBuildable)
		{
			this.pixles = pixles;
			this.type = type;
			this.isWalkable = isWalkable;
			this.isBuildable = isBuildable;
		}

		public bool hasMultipleUnits(){
			bool aUnit = false;
			foreach(GameObjectBase GO in tileoccupants){
				if (aUnit && GO is UnitBase)
					return true;

				if(GO is UnitBase)
					aUnit = true;
			}
			return false;
		}

		public bool hasUnits(){
		
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is UnitBase)
					return true;

			return false;
		
		}

		public bool HasPlayerUnit(){
			
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is PlayerUnit)
					return true;
			
			return false;
			
		}

		/// <summary>
		/// A tile is occupied by the first unit in the occupants list.
		/// All the other units are just passing through.
		/// </summary>
		/// <returns>The by.</returns>
		public UnitBase OccupiedBy(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is UnitBase)
					return (UnitBase)gO;
			
			return null;
			
		}

		public bool hasFog(){
			
			foreach (GameObjectBase gO in tileoccupants) {
				if (gO is Fog) {
					
					return gO.GetComponent<Renderer>().enabled;
				}
			}
			return false;
			
		}

		public Fog getFog(){
			
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is Fog)
					return (Fog)gO;
			
			return null;
			
		}

		public bool HasTree(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is TreeNotUnity)
					return true;
			return false;
		}

		public bool hasWater(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is Water)
					return true;
			return false;
		}
		public TreeNotUnity GetTree(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is TreeNotUnity)
					return (TreeNotUnity)gO;
			return null;
		}

		public bool HasHut(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is Hut)
					return true;
			return false;
		}

		public bool HasHouse(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is House)
					return true;
			return false;
		}

		public House GetHouse(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is House)
					return (House)gO;
			return null;
		}

		public Wall GetWall(){
			foreach(GameObjectBase gO in tileoccupants)
				if (gO is Wall)
					return (Wall)gO;
			return null;
		}

		public bool hasOccupant(){

			// Every tile has fog, so ignore 1 occupant.
			if (tileoccupants.Count == 0)
				return false;
			return true;
		}

		public WorldTile clone(){
			return new WorldTile (pixles, type, isWalkable, isBuildable);
		}

		public void setPosition(Vector3 position){
			this.position = position;
		}

		/// <summary>
		/// Adds the occupant.
		/// </summary>
		/// <param name="gameObject">Game object.</param>
		public void addOccupant(GameObjectBase gameObject){
			if (tileoccupants.Contains (gameObject))
				return;


			if (gameObject is Wall) {
				tileoccupants.Add (gameObject);
				description = "Wall";
				isWalkable = false;
				isBuildable = false;

			}else if (gameObject is UnitBase){
				tileoccupants.Add(gameObject);

			}else if (gameObject is Floor){
				tileoccupants.Add(gameObject);
				isBuildable = false;
				description = "Floor";

			}else if (gameObject is Bed){
				tileoccupants.Add(gameObject);
				isBuildable = false;
				description = "Bed";

			}else if (gameObject is TreeNotUnity){
				tileoccupants.Add(gameObject);
				type = gatheringTypes.TravelingToWood;
				isWalkable = false;
				isBuildable = false;
				description = "Tree";

			}else if (gameObject is Hut){
				tileoccupants.Add(gameObject);
				isWalkable = true;
				isBuildable = false;
				description = "Gathering Hut";

			}else if (gameObject is House){
				tileoccupants.Add(gameObject);
				isWalkable = true;
				isBuildable = false;
				description = "House";

			}else if (gameObject is Fog){
				tileoccupants.Add(gameObject);

			}else if (gameObject is Water){
				tileoccupants.Add(gameObject);
				isWalkable = false;
				isBuildable = false;
				description = "Water";
			}
		}

		public void removeOccupant(GameObjectBase occupant){

			tileoccupants.Remove(occupant);

			MakeWalkable();
			MakeBuildable();

		}

		public void KillOccupant(GameObjectBase occupant){
			
			tileoccupants.Remove(occupant);
			
			if(occupant is TreeNotUnity){
				MakeWalkable();
				MakeBuildable();
				occupant.KillSelf();
			}
			
		}

		public bool IsNextTo(WorldTile tile){
			foreach(WorldTile neighbor in borderTiles)
				if(neighbor == tile)
					return true;
			return false;
		}

		public void removeAllOccupants(){
			foreach(GameObjectBase occupant in tileoccupants)
				removeOccupant(occupant);
			
		}

		public void removeAllOccupantsExcept(GameObjectBase thisOne){
			List<GameObjectBase> removeList = new List<GameObjectBase>();
			foreach(GameObjectBase occupant in tileoccupants){
				if(occupant != thisOne && !(occupant is Fog)){
					removeList.Add(occupant);
					Debug.Log("tree");
				}
			}
			foreach(GameObjectBase occupant in removeList)
				KillOccupant(occupant);
		}

		public void MakeWalkable(){
			isWalkable = true;
		}

		public void MakeBuildable(){
			isBuildable = true;
		}

		public List<PlayerUnit> getPlayerUnits(){
			List<PlayerUnit> units = new List<PlayerUnit>();
			foreach(object obj in tileoccupants){
				if (obj is PlayerUnit)
					units.Add((PlayerUnit)obj);
			}
			return units;
		}

		public WorldTile[] getDiagnalNeighbors(){
			return new WorldTile[4] {borderTiles[(int)border.downLeft],
									borderTiles[(int)border.downRight],
									borderTiles[(int)border.upLeft],
									borderTiles[(int)border.upRight]};
		}

		public WorldTile[] getCloseNeighbors(){
			return new WorldTile[4] {borderTiles[(int)border.Left],
									borderTiles[(int)border.Right],
									borderTiles[(int)border.Down],
									borderTiles[(int)border.Up]};
		}

		public void changetype(WorldTile newtype){
			type = newtype.type;
			isWalkable = newtype.isWalkable;
			isBuildable = newtype.isBuildable;
			pixles = (Color[])newtype.pixles.Clone();
		}

		public bool BorderTilesContains(WorldTile tile){
			foreach (WorldTile neighbor in borderTiles){
				if(neighbor == tile)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <returns>The description.</returns>
		public String GetDescription(){
			return description;
		}


		public void DestroyStructure(){
			List<BuildingParts> removeList = new List<BuildingParts>();
			foreach(GameObjectBase occupant in tileoccupants){
				if(occupant is BuildingParts)
					removeList.Add((BuildingParts)occupant);
			}
			foreach(BuildingParts part in removeList){
				if(part.partOf != null)
					part.partOf.KillSelf();
				else
					part.KillSelf();
			}
				
		}

		public int GetDirection(WorldTile tile){
			for(int index = 0; index < borderTiles.Length; index++){
				if(tile == borderTiles[index])
					return index;
			}
			return (int)border.center;

		}
		
	}
}


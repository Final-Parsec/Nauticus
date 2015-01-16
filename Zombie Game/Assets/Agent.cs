using UnityEngine;
using System;
using System.Collections.Generic;
namespace Zombies
{
	public class Agent
	{
		public WorldTile occupiedTile;
		public GameObject ownedObject;
		public int numberOfMoves;
		public bool isAlive = true;

		private TileMap _tileMap;

		/// <summary>
		/// Icreates default Agent
		/// </summary>
		/// <param name="occupiedTile">The tile that the Agent starts on</param>
		/// <param name="ownedTile">The tile type that the Agent will 
		/// the other tiles to.</param>
		/// <param name="numberOfMoves">The number of moves the agent can make
		/// before it is killed.</param>
		public Agent (WorldTile occupiedTile, GameObject ownedObject, int numberOfMoves, TileMap _tileMap)
		{
			this.occupiedTile = occupiedTile;
			this.ownedObject = ownedObject;
			this.numberOfMoves = numberOfMoves;
			this._tileMap = _tileMap;
			
			// Convert the Initial tile.
			convertTile();
		}

		// Moves the agent to a locations and changes the new location to 
		// the ownedTile type
		// this runs numberOfMoves times
		public void move(){
			while (isAlive){
				numberOfMoves--;
				List<WorldTile> Destinations = getPossibleDestinations();
				goToBestTile(Destinations);

				if (numberOfMoves == 0)
					isAlive = false;
			}

		}

		/// <summary>
		/// gives the tiles with the most chances a better chance of
		/// being chosen
		/// moves the agent to the chosen tile and converts it.
		/// </summary>
		/// <param name="Destinations">Destinations.</param>
		private void goToBestTile(List<WorldTile> Destinations){
			List<WorldTile> destinationsBucket = new List<WorldTile>();

			foreach (WorldTile destination in Destinations){

				int chances = getChances(destination);
				for (int x=0; x<chances; x++)
					destinationsBucket.Add(destination);
			}

			// if the agent can't move then kill it
			if (destinationsBucket.Count == 0){
				isAlive = false;
				return;
			}

			int index = UnityEngine.Random.Range(0,destinationsBucket.Count);
			occupiedTile = (WorldTile)destinationsBucket[index];

			convertTile();
		}

		/// <summary>
		/// Converts the current tile the to owned tile type.
		/// </summary>
		private void convertTile(){
			_tileMap.InstantiateObject(ownedObject, occupiedTile.position);

		}

		/// <summary>
		/// The Agent is more likly to move to a location that has the most
		/// tiles of it's type touching it. Should form big sections 
		/// with minimal lines..
		/// </summary>
		/// <returns>The chances.</returns>
		/// <param name="destination">Destination</param>
		private int getChances(WorldTile destination){
			int chances = 0;


			foreach (WorldTile borderingTile in destination.borderTiles){
				if (borderingTile != null && borderingTile.type == occupiedTile.type)
					chances++;
			}

			return chances;
		}

		/// <summary>
		/// Gets all the possible tiles that the agent can move to.
		/// </summary>
		/// <returns>The possible destinations.</returns>
		private List<WorldTile> getPossibleDestinations(){
			List<WorldTile> destinations = new List<WorldTile>();

				foreach (WorldTile borderingTile in occupiedTile.borderTiles){
					if (borderingTile != null && !borderingTile.hasOccupant())
						destinations.Add(borderingTile);

				}
			return destinations;
		}
	}


}


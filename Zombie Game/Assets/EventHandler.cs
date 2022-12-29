using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.Threading;

namespace Zombies{
	[RequireComponent(typeof(TileMap))]
	public class EventHandler : MonoBehaviour {
		
		public TileMap _tileMap;
		public UnitGod _unitGod;
		public HUD _hud;

		public GameObject HoverWallPrefab;

		public HoverObject _hoverHouse;
		public HoverObject _hoverHut;
		public HoverObject _hoverWall;
		public HoverObject _hoverBoat;

		public Vector3 startWallPosition = Vector3.zero;
		private List<GameObject> HoverWallList = new List<GameObject>();
		public List<AudioClip> audioClips;
		
		public Vector3 tileCoord;

		public AudioClip buttonPress;

		// Multiple unit selection variables
		private Vector2 startMousePosition;
		private Vector3 startTileCoordinates;
		private Vector3 cameraPosition;
		int dragDeadZone = 20;

		public PlunderedOverlay overlay;

		// Initialization
		void Start() {
			_tileMap = GetComponent<TileMap>();
			_unitGod = UnitGod.GetInstance();
			_hud = GameObject.Find("Camera").GetComponent<HUD>();
			_hoverHouse = GameObject.Find("HoverHouse").GetComponent<HoverObject>();
			_hoverHut = GameObject.Find("HoverHut").GetComponent<HoverObject>();



		}

		/// <summary>
		/// Raises the GU event.
		/// Draws the selection box.
		/// </summary>
		void OnGUI () {
			if(_unitGod.playerUnits.Count==0){
				return;
			}

			if (Input.GetMouseButton(0) && Vector2.Distance(startMousePosition, Input.mousePosition) > dragDeadZone 
			    && _hud.rightButtonPressed == (int)rightButtons.noButtonPressed){

				Vector3 currentCameraPos= _hud._camera.transform.position;



				Rect rect = getRect(startMousePosition, Input.mousePosition);
				GUI.Box(new Rect (rect.x + cameraPosition.x-currentCameraPos.x, Screen.height - rect.y + cameraPosition.y-currentCameraPos.y, rect.width, rect.height), "");
			}
		}

		// Update is called once per frame
		void Update () {
			//end game
			if(_unitGod.playerUnits.Count==0){
				if (overlay == null){
					overlay = GameObject.Find("plundered_overlay").GetComponent<PlunderedOverlay>();
					GameObject.Find("plundered_text").GetComponent<Text>().enabled = true;
					overlay.Fade(0.0,.35,5.0);
				}
				return;
			}else if(_unitGod.Victory){
				if (overlay == null){
					overlay = GameObject.Find("victory_overlay").GetComponent<PlunderedOverlay>();
					GameObject.Find("victory_text").GetComponent<Text>().enabled = true;
					overlay.Fade(0.0,1,3.0);
				}
				return;
			}



			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hitInfo;
			WorldTile selectedTile = null;
			
			if( GetComponent<Collider>().Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
				int x = Mathf.FloorToInt( hitInfo.point.x / _tileMap.tileSize);
				int z = Mathf.FloorToInt( hitInfo.point.z / _tileMap.tileSize);

				tileCoord = _tileMap.toTileMapCoordnates(new Vector3(x, 0, z));
				selectedTile = _tileMap.world[(int)tileCoord.x, (int)tileCoord.z];
				_hud.tileMouseOver = selectedTile.GetDescription();
			}

			// Left click down
			if(Input.GetMouseButtonDown(0)) {
				startTileCoordinates = tileCoord;
				startMousePosition = Input.mousePosition;
				cameraPosition = _hud._camera.transform.position;

				// Clicked on the HUD. Don't have access to the button they pressed yet...
				if (_hud.pointCollidesWithHud(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))){
					//Debug.Log(""+_hud.rightButtonPressed);

				// Clicked on the map after clicking a right button.
				}else if(_hud.rightButtonPressed != (int)rightButtons.noButtonPressed){
					if(_hud.rightButtonPressed == (int)rightButtons.Wall){
						startWallPosition = tileCoord;
					}else if(_hud.rightButtonPressed == (int)rightButtons.Hut){
						BuildHut();
						DeselectButtons();
					}else if(_hud.rightButtonPressed == (int)rightButtons.House){
						BuildHouse();
						DeselectButtons();
					}else if(_hud.rightButtonPressed == (int)rightButtons.Boat){
						BuildBoat();
						DeselectButtons();
					}else if(_hud.rightButtonPressed == (int)rightButtons.Destroy){
						DestroyStructure();
						DeselectButtons();
					}
				

				// Clicked on the map with no button selected.
				}else{
					deselectSelectUnits(tileCoord);
				}
			}

			// Left click up
			if(Input.GetMouseButtonUp(0)){
				//Build walls
				if(_hud.rightButtonPressed == (int)rightButtons.Wall && startWallPosition != Vector3.zero){
					buildWall();
					DeselectButtons();
					startWallPosition = Vector3.zero;

				//Select units
				}else if(Vector2.Distance(startMousePosition, Input.mousePosition) > dragDeadZone){
					Vector3 endTileCoordinates = tileCoord;


					Rect rect = getRect(new Vector2(startTileCoordinates.x, startTileCoordinates.z),
					                    new Vector2(endTileCoordinates.x, endTileCoordinates.z));


					WorldTile tile;
					List<PlayerUnit> units = null;
					unitID selectionSoundID = unitID.NoUnit;
					for (int x = (int)rect.x; x<(int)(rect.x+rect.width) + 1; x++){
						for (int y=(int)(rect.y - rect.height); y<(int)(rect.y + 1); y++){

							tile = _tileMap.world[x, y];
							units = tile.getPlayerUnits();
							foreach (PlayerUnit Unit in units){
								Unit.Select();
								if((int)selectionSoundID < (int)Unit.GetUnitID())
									selectionSoundID = Unit.GetUnitID();
							}
							
						}
					}
					//Play unit selection sound
					_unitGod.PlaySelectionSound(selectionSoundID);


				}
			}

			// Right click down
			else if(Input.GetMouseButtonDown(1)){


				// Cancel a right button action by right clicking
				if(_hud.rightButtonPressed != (int)rightButtons.noButtonPressed){
					DeselectButtons();
				// Cut down tree
				}else if(_unitGod.UnitsAreSelected() && selectedTile != null && selectedTile.HasTree()){
					AudioSource.PlayClipAtPoint(buttonPress, Vector3.zero, 1f * _hud.masterVolume);
					_unitGod.SetSelectedUnitsToGathering(gatheringTypes.TravelingToWood);
					moveTheUnits(_tileMap.world[(int)tileCoord.x, (int) tileCoord.z] ,_unitGod.GetSelectedUnits());
				// Move the unit when the player right clicks
				}else if (_unitGod.UnitsAreSelected()){
					AudioSource.PlayClipAtPoint(buttonPress, Vector3.zero, 1f * _hud.masterVolume);
					_unitGod.SetSelectedUnitsToGathering(gatheringTypes.NotGathering);
					moveTheUnits(_tileMap.world[(int)tileCoord.x, (int) tileCoord.z] ,_unitGod.GetSelectedUnits());
				}
			}

			//rotate clockwise
			if(Input.GetKeyDown(KeyCode.Q)){
				if(_hud.rightButtonPressed == (int)rightButtons.Wall){
				}else if(_hud.rightButtonPressed == (int)rightButtons.Hut){
					_hoverHut.Rotate(90);
				}else if(_hud.rightButtonPressed == (int)rightButtons.House){
					_hoverHouse.Rotate(90);
				}
			}

			//rotate counter clockwise
			if(Input.GetKeyDown(KeyCode.E)){
				if(_hud.rightButtonPressed == (int)rightButtons.Wall){
				}else if(_hud.rightButtonPressed == (int)rightButtons.Hut){
					_hoverHut.Rotate(-90);
				}else if(_hud.rightButtonPressed == (int)rightButtons.House){
					_hoverHouse.Rotate(-90);
				}
			}

			// Display the object before it is placed
			WorldTile currentTile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			if(_hud.rightButtonPressed == (int)rightButtons.Wall){

				foreach(GameObject hoverWall in HoverWallList)
					Destroy(hoverWall.gameObject);
				HoverWallList = new List<GameObject>();
				if(startWallPosition != Vector3.zero){
					Vector3 start = ((HoverWall)_hoverWall).GetStartCoordinate();

					for (int x = (int)start.x; x < start.x + _hoverWall.sizeX; x++){
						for (int z = (int)start.z; z > start.z - _hoverWall.sizeZ; z--){
							currentTile = _tileMap.world[x, z];
							HoverWallList.Add((GameObject)Instantiate(HoverWallPrefab, currentTile.position, Quaternion.Euler(new Vector3(0, 0, 0))));
						
						}
					}

				}else{
					_hoverWall.transform.position = currentTile.position;
				}
			}else if(_hud.rightButtonPressed == (int)rightButtons.Hut){
				_hoverHut.transform.position = currentTile.position;
			}else if(_hud.rightButtonPressed == (int)rightButtons.House){
				_hoverHouse.transform.position = currentTile.position;
			}else if(_hud.rightButtonPressed == (int)rightButtons.Boat){
				_hoverBoat.transform.position = new Vector3( currentTile.position.x, 1, currentTile.position.z);
			}


		}

		public void DeselectButtons(){
			if(_hud.rightButtonPressed == (int)rightButtons.Wall){
				_hoverWall.transform.position = new Vector3(15, -1, -5);
				startWallPosition = Vector3.zero;
			}else if(_hud.rightButtonPressed == (int)rightButtons.Hut){
				_hoverHut.transform.position = new Vector3(15, -1, -5);
			}else if(_hud.rightButtonPressed == (int)rightButtons.House){
				_hoverHouse.transform.position = new Vector3(15, -1, -5);
			}else if(_hud.rightButtonPressed == (int)rightButtons.Boat){
				_hoverBoat.transform.position = new Vector3(15, -1, -5);
			}
			_hud.rightButtonPressed = (int)rightButtons.noButtonPressed;
		}

		/// <summary>
		/// Displays a house.
		/// </summary>
		private void displayHouse(){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			Instantiate(_tileMap.HousePrefab, tile.position, Quaternion.Euler(new Vector3(0, 0, 0)));
		}

		/// <summary>
		/// Builds a house.
		/// </summary>
		private void BuildHouse(){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			if (_hoverHouse.CanBuild()) {
				_hoverHouse.UseResourse();
				Instantiate(_tileMap.HousePrefab, tile.position, _hoverHouse.transform.rotation);
				
			}
		}

		/// <summary>
		/// Builds a hut.
		/// </summary>
		private void BuildHut(){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			if (_hoverHut.CanBuild()) {
				_hoverHut.UseResourse();
				Instantiate(_tileMap.HutPrefab, tile.position, _hoverHut.transform.rotation);
				
			}
		}

		private void BuildBoat(){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			if (_hoverBoat.CanBuild()) {
				_hoverBoat.UseResourse();
				Instantiate(_tileMap.BoatPrefab, tile.position, _hoverBoat.transform.rotation);
				
			}
		}



		/// <summary>
		/// Builds a wall.
		/// </summary>
		private void buildWall(){
			if (_hoverWall.CanBuild()){
			}
			startWallPosition = Vector3.zero;
			foreach(GameObject hoverWall in HoverWallList)
				Destroy(hoverWall.gameObject);
		}

		/// <summary>
		/// Destroies the structure.
		/// </summary>
		private void DestroyStructure(){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			tile.DestroyStructure();
		}

		/// <summary>
		/// Moves the unit.
		/// </summary>
		public void moveTheUnits(WorldTile tile, List<UnitBase> units){
			StartCoroutine(WaitForIt(tile, units));

		}

		/// <summary>
		/// Moves the unit.
		/// </summary>
		public void moveTheUnit(WorldTile tile, UnitBase unit){
			List<UnitBase> temp = new List<UnitBase>();
			temp.Add(unit);
			StartCoroutine(WaitForIt(tile, temp));
		}

		IEnumerator WaitForIt(WorldTile tile, List<UnitBase> units) {

			if(tile != null){
				
				foreach(UnitBase unit in units){
					yield return new WaitForEndOfFrame();

					WorldTile temp = tile;

					temp = findWalkableTile(tile);

					WorldTile unitTile = unit.onTile;

					List<WorldTile> path = null;
					path = Astar (unitTile, temp);

					if (path != null){
						unit.StopMoving();
						unit.SetPath (path);

					}else{
						Debug.Log("no path!");
						break;
					}
					
				}

			}

		}


		/// <summary>
		/// Deselects or selects units.
		/// </summary>
		/// <param name="coord">Coordinate.</param>
		private void deselectSelectUnits(Vector3 tileCoord){
			WorldTile tile = _tileMap.world[(int)tileCoord.x, (int) tileCoord.z];
			
			List<PlayerUnit> units = tile.getPlayerUnits();
			_unitGod.deselectAllUnits();
			foreach(PlayerUnit unit in units)
				unit.Select();
		}

		/// <summary>
		/// Gets the rect.
		/// </summary>
		/// <returns>The rect.</returns>
		/// <param name="point1">Point1.</param>
		/// <param name="point2">Point2.</param>
		public Rect getRect(Vector2 point1, Vector2 point2){
		
			float width = Mathf.Abs(point1.x - point2.x);
			float height = Mathf.Abs(point1.y - point2.y);
			float x;
			float y;
			if(point1.x < point2.x)
				x = point1.x;
			else 
				x = point2.x;
			
			if(point1.y > point2.y)
				y = point1.y;
			else 
				y = point2.y;

			return new Rect (x, y, width, height);
			
		}


		// speeding up A*
		//use binary heap - http://www.policyalmanac.org/games/binaryHeaps.htm
		//other ideas here http://www.policyalmanac.org/games/aStarTutorial.htm
		//calculate islands, deadzonze
		//Flocking
		private List<WorldTile> Astar(WorldTile start, WorldTile goal){
			if (goal == null || start == goal)
				return null;

			// initialize pathfinding variables
			foreach( WorldTile tile in _tileMap.world){
				tile.gScore = int.MaxValue;
				tile.fScore = int.MaxValue;
				tile.parent = null;
				tile.isInOpenSet = false;
				tile.isInClosedSet = false;
					
			}

			MinHeap openSet = new MinHeap(start);

			start.gScore = 0;
			start.fScore = start.gScore + heuristic_cost_estimate(start, goal);

			while (openSet.heap.Count > 1){
				// get closest node
				WorldTile current = openSet.GetRoot();

				// if its the goal, return
				if (current == goal)
					return reconstruct_path(start, goal);
					
				// look at the neighbors of the node
				foreach (WorldTile neighbor in current.getDiagnalNeighbors()){
					// ignore the ones that are unwalkable or are in the closed set
					if (neighbor != null && neighbor.isWalkable && !neighbor.isInClosedSet){

						// if the new gscore is lower replace it
						int tentativeGscore =  current.gScore + 14;
						if (!neighbor.isInOpenSet || tentativeGscore< neighbor.gScore){

							neighbor.parent = current;
							neighbor.gScore = tentativeGscore;
							neighbor.fScore = neighbor.gScore + heuristic_cost_estimate(neighbor, goal);
						}

						// if neighbor's not in the open set add it
						if(!neighbor.isInOpenSet){
							openSet.BubbleUp(neighbor);
						}
					}
				}

				// look at the neighbors of the node
				foreach (WorldTile neighbor in current.getCloseNeighbors()){
					// ignore the ones that are unwalkable or are in the closed set
					if (neighbor != null && neighbor.isWalkable && !neighbor.isInClosedSet){

						// if the new gscore is lower replace it
						int tentativeGscore =  current.gScore + 10;
						if (!neighbor.isInOpenSet || tentativeGscore< neighbor.gScore){
							
							neighbor.parent = current;
							neighbor.gScore = tentativeGscore;
							neighbor.fScore = neighbor.gScore + heuristic_cost_estimate(neighbor, goal);
						}
						
						// if neighbor's not in the open set add it
						if(!neighbor.isInOpenSet){
							openSet.BubbleUp(neighbor);
						}
					}
				}

			}
			// Fail
			return null;
		}

		// For now, distance is only 1
		private int disttanceBetween(WorldTile tileA, WorldTile tileB){
			return 1;
		}

		/// <summary>
		/// Reconstruct_path the specified start and goal.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="goal">Goal.</param>
		private List<WorldTile> reconstruct_path(WorldTile start, WorldTile goal){
			List<WorldTile> path = new List<WorldTile>();
			path.Add(goal);

			WorldTile itr = goal;
			while (itr.parent != start){
				path.Add (itr.parent);
				itr = itr.parent;
			}

			return path;
		}

		public int heuristic_cost_estimate(WorldTile start, WorldTile goal){
			// manhattan heuristic, 10 is the cost of moving horizontally.
			int xComponent = (int) Mathf.Abs(start.position.x - goal.position.x);
			int zComponent = (int) Mathf.Abs(start.position.z - goal.position.z);

			//int y = (int)Mathf.Sqrt(Mathf.Pow(xComponent, 2) + Mathf.Pow(zComponent, 2));
			
			return (xComponent + zComponent) * 10;
			//return y * 14;
		}


		//could use dikstra for Resourse gathering units
		private List<WorldTile> dijkstra(WorldTile start, WorldTile end){
			List<WorldTile> Q = new List<WorldTile>();

			foreach( WorldTile vertex in _tileMap.world){
				vertex.distance = int.MaxValue;
				vertex.previous = null;

				Q.Add(vertex);
			}


			start.distance = 0;

			while (Q.Count != 0){

				Q = Q.OrderBy(o=>o.distance).ToList();

				WorldTile u = Q[0];
				Q.Remove(u);

				if (u == end)
					return getShortestPath(u);

				if(u.distance == int.MaxValue)
					break;


				foreach(WorldTile neighbor in u.borderTiles){
					if(neighbor != null && neighbor.isWalkable){

						int alt = u.distance + 1;

						if(alt < neighbor.distance){
							neighbor.distance = alt;
							neighbor.previous = u;

						}
					}
				}



			}
			//fail
			return null;
		}

		private List<WorldTile> getShortestPath(WorldTile end){
			List<WorldTile> s = new List<WorldTile>();
			WorldTile itr = end;
			while (itr.previous!=null){
				s.Add(itr);
				itr = itr.previous;
			}
			return s;
		}


		/// <summary>
		/// Finds a walkable tile by using djikstra's algorithm to find the closest one that
		/// the player can move to.
		/// </summary>
		/// <returns>The walkable tile.</returns>
		/// <param name="start">Start.</param>
		public WorldTile findWalkableTile(WorldTile itr){

			if (itr.isWalkable && itr.reservedByUnit == null)
				return itr;

			List<WorldTile> visitedTiles = new List<WorldTile>();
			List<WorldTile> activeTiles = new List<WorldTile>();
			activeTiles.Add(itr);


			while (activeTiles.Count != 0) {
				WorldTile currentTile = activeTiles[0];
				activeTiles.RemoveAt(0);
				visitedTiles.Add (currentTile);

				foreach (WorldTile tile in currentTile.borderTiles) {
					if (tile != null && !visitedTiles.Contains (tile) && tile.isWalkable && tile.reservedByUnit == null) {
							return tile;

					} else if(tile != null && !visitedTiles.Contains (tile)){
							activeTiles.Add (tile);
					}
				}
			}
			return null;
		}

		private WorldTile findTileByType(WorldTile itr, gatheringTypes type){
			
			List<WorldTile> visitedTiles = new List<WorldTile>();
			visitedTiles.Add(itr);
			
			itr = itr.borderTiles[(int)border.Left];
			
			while(itr.type != type){
				visitedTiles.Add(itr);
				
				// Move Down
				if (!visitedTiles.Contains(itr.borderTiles[(int)border.Up]) &&
				    visitedTiles.Contains(itr.borderTiles[(int)border.Right]) 		)
					
					itr = itr.borderTiles[(int)border.Up];
				
				// Move Right
				else if (visitedTiles.Contains(itr.borderTiles[(int)border.Down]) &&
				         !visitedTiles.Contains(itr.borderTiles[(int)border.Right]))
					
					itr = itr.borderTiles[(int)border.Right];
				
				// Move Up
				else if (!visitedTiles.Contains(itr.borderTiles[(int)border.Down]) && 
				         visitedTiles.Contains(itr.borderTiles[(int)border.Left]))
					
					itr = itr.borderTiles[(int)border.Down];
				
				// Move Left
				else
					itr = itr.borderTiles[(int)border.Left];
				
			}
			
			return itr;
		}


	}
}
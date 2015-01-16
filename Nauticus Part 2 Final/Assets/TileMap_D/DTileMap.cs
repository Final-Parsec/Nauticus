using UnityEngine;
using System.Collections.Generic;

public class DTileMap {
	
	/*protected class DTile {
		bool isWalkable = false;
		int tileGraphicId = 0;
		string name = "Unknown";
	}
	
	List<DTile> tileTypes;

	void InitTiles() {
		tileType[1].name = "Floor";
		tileType[1].isWalkable = true;
		tileType[1].tileGraphicId = 1;
		tileType[1].damagePerTurn = 0;
	}*/

	Vector2 spawnPos;
	int spawnChance;
	public Vector2 stairPos;
	public List<Vector2> enemySpawns = new List<Vector2>();

	protected class DRoom {
		public int left;
		public int top;
		public int width;
		public int height;
		
		public bool isConnected=false;
		
		public int right {
			get {return left + width - 1;}
		}
		
		public int bottom {
			get { return top + height - 1; }
		}
		
		public int center_x {
			get { return left + width/2; }
		}
		
		public int center_y {
			get { return top + height/2; }
		}
		
		public bool CollidesWith(DRoom other) {
			if( left > other.right-1 )
				return false;
			
			if( top > other.bottom-1 )
				return false;
			
			if( right < other.left+1 )
				return false;
			
			if( bottom < other.top+1 )
				return false;
			
			return true;
		}
		
		
	}
	
	int size_x;
	int size_y;

	int[,] map_data;
	
	List<DRoom> rooms;
	
	/*
	 * 0 = unknown
	 * 1 = floor
	 * 2 = wall
	 * 3 = stone
	 */
	
	public DTileMap(int size_x, int size_y, int enemySpawnChance) {
		spawnChance = enemySpawnChance;
		DRoom r;
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new int[size_x,size_y];
		
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				map_data[x,y] = 3;
			}
		}
		
		rooms = new List<DRoom>();
		
		int maxFails = 10;
		
		while(rooms.Count < 10) {
			int rsx = Random.Range(4,14);
			int rsy = Random.Range(4,10);
			
			r = new DRoom();
			r.left = Random.Range(0, size_x - rsx);
			r.top = Random.Range(0, size_y-rsy);
			r.width = rsx;
			r.height = rsy;
			
			if(!RoomCollides(r)) {			
				rooms.Add (r);
			}
			else {
				maxFails--;
				if(maxFails <=0)
					break;
			}
			
		}
		
		foreach(DRoom r2 in rooms) {
			MakeRoom(r2);
		}
		

		for(int i=0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = Random.Range(1, rooms.Count);
				MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count ]);
			}
		}
		
		MakeWalls();
		SpawnPirate();
		SpawnEnemies ();
		SpawnStaircase ();
	}
	
	bool RoomCollides(DRoom r) {
		foreach(DRoom r2 in rooms) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}
	
	public int GetTileAt(int x, int y) {
		return map_data[x,y];
	}
	
	void MakeRoom(DRoom r) {
		
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width-1 || y==0 || y == r.height-1) {
					map_data[r.left+x,r.top+y] = 2;
				}
				else {
					map_data[r.left+x,r.top+y] = 1;
				}
			}
		}
		
	}
	
	void MakeCorridor(DRoom r1, DRoom r2) {
		int x = r1.center_x;
		int y = r1.center_y;
		
		while( x != r2.center_x) {
			map_data[x,y] = 1;
			
			x += x < r2.center_x ? 1 : -1;
		}
		
		while( y != r2.center_y ) {
			map_data[x,y] = 1;
			
			y += y < r2.center_y ? 1 : -1;
		}
		
		r1.isConnected = true;
		r2.isConnected = true;
		
	}
	
	void MakeWalls() {
		for(int x=0; x< size_x;x++) {
			for(int y=0; y< size_y;y++) {
				if(map_data[x,y]==3 && HasAdjacentFloor(x,y)) {
					map_data[x,y]=2;
				}
			}
		}
	}
	
	/// <summary>
	/// Spawns ye pirate.
	/// </summary>
	void SpawnPirate() {
		for(int x=0; x<size_x; x++) {
			for(int y=0; y<size_y; y++) {
				if(map_data[x,y]==1){
					map_data[x,y] = 0;
					spawnPos = new Vector2(x,y);
					return;
				}
			}
		}
	}

	/// <summary>
	/// gets a list of enemy spawnpoints
	/// </summary>
	void SpawnEnemies() {
		int rnd;
		for(int x=0; x<size_x; x++) {
			for(int y=0; y<size_y; y++) {
				if(map_data[x,y]==1){
					rnd = Random.Range (1,spawnChance);
					if(rnd == 1)
						enemySpawns.Add(new Vector2(x,y));
				}
			}
		}
		//foreach(Vector2 pos in enemySpawns)
		//	Debug.Log(pos.x);
	}

	/// <summary>
	/// Spawns the staircase.  The player spawns in the bottom left, so the staircase must spawn in the top right or
	/// the bottom right
	/// </summary>
	public void SpawnStaircase(){
		switch(Random.Range (0,2)){
		case 1:
			//top right
			for (int x=size_x-1; x>=0; x--) {
				for (int y=size_y-1; y>=0; y--) {
					if(map_data[x,y]==1){
						stairPos = new Vector2 (x, y);
						return;
					}
				}
			}
			break;
		case 0:
			//bottom right
			for (int x=size_x-1; x>=0; x--)  {
				for (int y=0; y<size_y; y++)  {
					if(map_data[x,y]==1){
						stairPos = new Vector2 (x, y);
						return;
					}
				}
			}
			break;
		default:
			//bottom right
			for (int x=size_x; x>=0; x--)  {
				for (int y=0; y<size_y; y++)  {
					if(map_data[x,y]==1){
						stairPos = new Vector2 (x, y);
						return;
					}
				}
			}
			break;
		}
	}
	/// <summary>
	/// Gets the pirate.
	/// </summary>
	/// <returns>The pirate.</returns>
	public Vector2 getSpawnPos(){
				return spawnPos;
		}
	
	bool HasAdjacentFloor(int x, int y) {
		if( x > 0 && map_data[x-1,y] == 1 )
			return true;
		if( x < size_x-1 && map_data[x+1,y] == 1 )
			return true;
		if( y > 0 && map_data[x,y-1] == 1 )
			return true;
		if( y < size_y-1 && map_data[x,y+1] == 1 )
			return true;

		if( x > 0 && y > 0 && map_data[x-1,y-1] == 1 )
			return true;
		if( x < size_x-1 && y > 0 && map_data[x+1,y-1] == 1 )
			return true;
		
		if( x > 0 && y < size_y-1 && map_data[x-1,y+1] == 1 )
			return true;
		if( x < size_x-1 && y < size_y-1 && map_data[x+1,y+1] == 1 )
			return true;
		
		
		return false;
	}
}

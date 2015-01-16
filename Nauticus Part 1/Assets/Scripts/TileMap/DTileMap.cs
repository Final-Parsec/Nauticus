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
	
	protected class DIsland {
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

		/// <summary>
		/// Island collision checking. Ensures there is a gap large enough to form water (2 spaces).
		/// </summary>
		/// <returns>bool with true idicating a collision</returns>
		/// <param name="other">Other island.</param>
		public bool CollidesWith(DIsland other) {
			if( left > other.right-2 )
				return false;
			
			if( top > other.bottom+2 )
				return false;
			
			if( right < other.left-2 )
				return false;
			
			if( bottom < other.top+2 )
				return false;
			
			return true;
		}
		
		
	}
	
	int size_x;
	int size_y;
	
	int[,] map_data;
	
	List<DIsland> islands;
	
	/*
	 * 0 = unknown
	 * 1 = grass
	 * 2 = beach
	 * 3 = water
	 */
	
	public DTileMap(int size_x, int size_y) {
		DIsland r;
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new int[size_x,size_y];
		
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				if (Random.Range (0, 20) < 1) {
					map_data[x,y] = 3;
				}
				else {
					map_data[x,y] = 4;
				}
				
				// Uncomment this to make a shoreline along bottom of TileMap.
//				if (y > size_y-3) { 
//					map_data[x,y] = 12;
//				}
//				
//				if (y == size_y-1) { 
//					map_data[x,y] = 9;
//				}

				if (x == (size_x - 1) && y == (size_y - 1)) {
					map_data[x,y] = 0;
				}
			}
		}
		
		islands = new List<DIsland>();
		
		int maxFails = 10;
		
		while(islands.Count < 10) {
			int rsx = Random.Range(7,17);
			int rsy = Random.Range(9,16);
			
			r = new DIsland();
			r.left = Random.Range(0, size_x - rsx);
			r.top = Random.Range(0, size_y-rsy);
			r.width = rsx;
			r.height = rsy;
			
			if(!IslandCollides(r)) {			
				islands.Add (r);
			}
			else {
				maxFails--;
				if(maxFails <=0)
					break;
			}
			
		}
		
		foreach(DIsland r2 in islands) {
			MakeIsland(r2);
		}
		

		for(int i=0; i < islands.Count; i++) {
			if(!islands[i].isConnected) {
				//int j = Random.Range(1, islands.Count);
				//MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count ]);
			}
		}
		
		MakeBeaches();
	}
	
	bool IslandCollides(DIsland r) {
		foreach(DIsland r2 in islands) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}

	public int GetTileAt(int x, int y) {
		if (x < 0 || y < 0) {
			return -1;
		}
	
		if (x < size_x && y < size_y) {
			return map_data [x, y];
		} else {
			return -1;
		}
	}
	
	void MakeIsland(DIsland r) {
		
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width-1 || y==0 || y == r.height-1) {
					//map_data[r.left+x,r.top+y] = 2;
				}
				else {
					map_data[r.left+x,r.top+y] = 1;
				}
			}
		}
		
	}
	
	void MakeCorridor(DIsland r1, DIsland r2) {
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
	
	void MakeBeaches() {
		for(int x=0; x< size_x;x++) {
			for(int y=0; y< size_y;y++) {
				if((map_data[x,y]==3 || map_data[x,y] == 4)) {
					int beach_tile = GetBeachTile(x,y);

					if (beach_tile > 0) {
						map_data[x,y]=beach_tile;
					}
				}
			}
		}
	}

	int GetBeachTile(int x, int y) {
		if( x > 0 && map_data[x-1,y] == 1 )
			return 10;
		if( x < size_x-1 && map_data[x+1,y] == 1 )
			return 8;
		if( y > 0 && map_data[x,y-1] == 1 )
			return 6;
		if( y < size_y-1 && map_data[x,y+1] == 1 )
			return 12;
		
		if( x > 0 && y > 0 && map_data[x-1,y-1] == 1 )
			return 7;
		if( x < size_x-1 && y > 0 && map_data[x+1,y-1] == 1 )
			return 5;
		
		if( x > 0 && y < size_y-1 && map_data[x-1,y+1] == 1 )
			return 13;
		if( x < size_x-1 && y < size_y-1 && map_data[x+1,y+1] == 1 )
			return 11;
		
		
		return -1;
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

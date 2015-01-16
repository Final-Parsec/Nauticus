using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	public int size_x = 100;
	public int size_z = 50;
	public float tileSize = 1.0f;

	public int currLevel = 1;

	public Texture2D terrainTiles;
	public int tileResolution;
	public DTileMap map;


	List<Vector2> enemyPositions;
	public Vector2 staircasePosition;

	public GameObject Piro;
	public GameObject ZombieEnemy;
	public GameObject Boss;
	public GameObject Staircase;
	public GameObject Crate;

	public Vector2 spawnPos;

	int enemySpawnChance = 15;

	void Start () {
		BuildMesh();
	}
	
	Color[][] ChopUpTiles() {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow*numRows][];
		
		for(int y=0; y<numRows; y++) {
			for(int x=0; x<numTilesPerRow; x++) {
				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels( x*tileResolution , y*tileResolution, tileResolution, tileResolution );
			}
		}

		return tiles;
	}

	/// <summary>
	/// Builds the texture.
	/// </summary>
	void BuildTexture() {
		map = new DTileMap(size_x, size_z, enemySpawnChance);

		enemyPositions = map.enemySpawns;
		staircasePosition = map.stairPos;
		
		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles();
		
		for(int y=0; y < size_z; y++) {
			for(int x=0; x < size_x; x++) {
				Color[] p = tiles[ map.GetTileAt(x,y) ];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		Debug.Log ("Done Texture!");
	}
	
	public void BuildMesh() {
		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];

		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, -z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / size_x, 1f - (float)z / size_z );
			}
		}
		Debug.Log ("Done Verts!");
		
		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 5] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 4] = z * vsize_x + x + 		   1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");

		BuildTexture();  //grabs dtilemap; needed to find spawnpos
		spawnPos = map.getSpawnPos ();
		if (Application.loadedLevelName == "Game3")
						spawnBoss ();
		else
			spawnStaircase ();
		spawnEnemies ();
		//if it's not level 1, you don't need new pirates!
		if(currLevel == 1)
			spawnPirate ();

	}

	/// <summary>
	/// Spawns the pirate (moves it to the correct spawn position on the map.
	/// </summary>
	public void spawnPirate(){
			//_Piro = GameObject.Find("pirate").GetComponent<PirateHero>();
			//_Piro.spawnPirate(spawnPos);
			Piro = (GameObject)Instantiate (Piro, gameObject.transform.position, Quaternion.identity);
		
			//Debug.Log (piro.ToString ());
			//Piro.GetComponent<PirateHero>().spawnPirate (spawnPos);
		}

	/// <summary>
	/// Spawns the crate.
	/// </summary>
	public void spawnCrate(Vector3 transformPos, int x_pos, int y_pos){
		GameObject SuperCrate;
		SuperCrate = (GameObject)Instantiate (Crate, transformPos, Crate.transform.rotation);
		SuperCrate.GetComponent<ItemCrate> ().x_pos = x_pos;
		SuperCrate.GetComponent<ItemCrate> ().y_pos = y_pos;
	}

	/// <summary>
	/// Spawns the enemies.
	/// </summary>
	public void spawnEnemies(){
		foreach (Vector2 vec in enemyPositions) {
			if( vec != null){
				ZombieEnemy = (GameObject)Instantiate (ZombieEnemy, Piro.transform.position, Quaternion.identity);
				ZombieEnemy.GetComponent<Zombie> ().setStartPosition ((int)vec.x, (int)vec.y, (int)tileSize);
			}
		}
	}

	/// <summary>
	/// Spawns the staircase.  The player spawns in the bottom left, so the staircase must spawn in the top right or
	/// the top bottom right
	/// </summary>
	public void spawnStaircase(){
			Staircase = (GameObject)Instantiate (Staircase, gameObject.transform.position, Quaternion.identity);
			Staircase.GetComponent<StairsDown> ().setStartPosition ((int)staircasePosition.x, (int)staircasePosition.y, (int)tileSize);
		}

	/// <summary>
	/// Spawns the boss.
	/// </summary>
	public void spawnBoss(){
		Boss = (GameObject)Instantiate (Boss, gameObject.transform.position, Quaternion.identity);
		Boss.GetComponent<Boss> ().setStartPosition ((int)staircasePosition.x, (int)staircasePosition.y, (int)tileSize);
	}


	public Vector2 getSpawnPos(){
			return spawnPos;
		}

	/// <summary>
	/// Cans the move.
	/// </summary>
	/// <returns><c>true</c>, if move was caned, <c>false</c> otherwise.</returns>
	/// <param name="direction">Direction.</param>
	/// <param name="x_pos">X_pos.</param>
	/// <param name="y_pos">Y_pos.</param>
	public bool canMove(string direction, int x_pos, int y_pos){
			if(direction.Equals("up"))
				if(map.GetTileAt(x_pos, y_pos + 1) == 1)
					return true;
			if(direction.Equals("down"))

				if(map.GetTileAt(x_pos, y_pos - 1) == 1)
					return true;
			if(direction.Equals("left"))
				if(map.GetTileAt(x_pos+1, y_pos) == 1)
					return true;
			if(direction.Equals("right"))
				if(map.GetTileAt(x_pos+1, y_pos) == 1)
					return true;
			return false;
	}

	/// <summary>
	/// Cans the move.
	/// </summary>
	/// <returns><c>true</c>, if move was caned, <c>false</c> otherwise.</returns>
	/// <param name="x_pos">X_pos.</param>
	/// <param name="y_pos">Y_pos.</param>
	public bool canMove(int x_pos, int y_pos){
		if(map.GetTileAt(x_pos, y_pos) == 1)
			return true;
		return false;
	}

	/// <summary>
	/// Nexts the level.
	/// </summary>
	/// <returns>The new pirate spawnposition vector 2</returns>
	/// <param name="paraPiro">persistent HERO PIRATE PIRO</param>
	public Vector2 nextLevel(GameObject paraPiro){
		Staircase.GetComponent<StairsDown>().killyoself ();
		Piro = paraPiro;
		currLevel++;
		BuildMesh ();
		return getSpawnPos ();
	}

}
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System;

namespace Zombies
{

	[RequireComponent(typeof(EventHandler))]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class TileMap : MonoBehaviour {
		private Stopwatch stopWatch = new Stopwatch();


		public int size_x = 100;
		public int size_z = 50;
		public float tileSize = 1.0f;

		public GameObject PiroThePirateHero;
		public GameObject UnitPrefab;
		public GameObject WallPrefab;
		public GameObject HousePrefab;
		public GameObject HutPrefab;
		public GameObject BoatPrefab;
		public GameObject TreePrefab;
		public GameObject WaterPrefab;
		public GameObject ZombiePrefab;
		public int startingZombieNum = 100;
		public Texture2D terrain;
		public int tileResolution = 32;

		public WorldTile[,] world;

		public WorldTile[] tiles = new WorldTile[2];

		public List<WorldTile> dirtyTiles = new List<WorldTile>();

		public EventHandler _eventHandler;

		// Use this for initialization
		void Start () {
			_eventHandler = GameObject.Find("TileMap").GetComponent<EventHandler>();
			stopWatch.Start();
			UnityEngine.Debug.Log("Starting");
			BuildMesh();
			spawnPlayer();
			spawnZombies ();
		}

		void spawnZombies(){
			int numZeds = 0;
			WorldTile location;

			while(numZeds < startingZombieNum){
				location = world[UnityEngine.Random.Range(0, size_x), UnityEngine.Random.Range(0, size_z)];

				if (!location.hasOccupant()){
					Instantiate(ZombiePrefab, location.position, Quaternion.Euler(new Vector3(90, 0, 0)));
					numZeds++;
				}
			}
		}

		void spawnPlayer(){
			WorldTile location = world[UnityEngine.Random.Range(0, size_x), UnityEngine.Random.Range(0, size_z)];
			while(!location.isWalkable ||
			      (location.borderTiles[(int)border.Down] == null ) ||
			      (location.borderTiles[(int)border.Up] == null) ||
			      (location.borderTiles[(int)border.Left] == null) ||
			      (location.borderTiles[(int)border.Right] == null)){
				location = world[UnityEngine.Random.Range(0, size_x), UnityEngine.Random.Range(0, size_z)];
				UnityEngine.Debug.Log("reroll spawn location");
			}

			Instantiate(PiroThePirateHero, location.position, Quaternion.Euler(new Vector3(90, 0, 0)));
			Instantiate(UnitPrefab, location.borderTiles[(int)border.Down].position, Quaternion.Euler(new Vector3(90, 0, 0)));
			Instantiate(UnitPrefab, location.borderTiles[(int)border.Up].position, Quaternion.Euler(new Vector3(90, 0, 0)));
			Instantiate(UnitPrefab, location.borderTiles[(int)border.Left].position, Quaternion.Euler(new Vector3(90, 0, 0)));
			Instantiate(UnitPrefab, location.borderTiles[(int)border.Right].position, Quaternion.Euler(new Vector3(90, 0, 0)));

			//Unit _player1 = GameObject.Find("Player1").GetComponent<Unit>();
			CameraMovement _camera = GameObject.Find("Camera").GetComponent<CameraMovement>();

			_camera.setStartingPosition(location.position);
		}

		// Update is called once per frame
		void Update(){
			if (dirtyTiles.Count == 0)
				return;

			MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
			Texture2D texture = (Texture2D)mesh_renderer.sharedMaterials[0].mainTexture;
			foreach (WorldTile dirtyTile in dirtyTiles){
				texture.SetPixels((int)dirtyTile.position.x*tileResolution, (int)dirtyTile.position.y*tileResolution, tileResolution, tileResolution, dirtyTile.pixles);
			}

			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.Apply();

			mesh_renderer.sharedMaterials[0].mainTexture = texture;
			dirtyTiles = new List<WorldTile>();

		}

		public void connectMap(){
			for (int y=0; y<size_z; y++){
				for (int x=0; x<size_x; x++){
					// Awefull looking if statements
					if (x - 1 >= 0){
						world[x,y].borderTiles[(int)border.Right] = world[x-1,y];
						if(y - 1 >= 0)
							world[x,y].borderTiles[(int)border.downRight] = world[x-1,y-1];
						
						if(y + 1 < world.GetLength(1))
							world[x,y].borderTiles[(int)border.upRight] = world[x-1,y+1];
					}
					
					if (x + 1 < world.GetLength(0)){
						world[x,y].borderTiles[(int)border.Left] = world[x+1,y];
						if(y - 1 >= 0)
							world[x,y].borderTiles[(int)border.downLeft] = world[x+1,y-1];
						
						if(y + 1 < world.GetLength(1))
							world[x,y].borderTiles[(int)border.upLeft] = world[x+1,y+1];
					}
					
					if(y - 1 >= 0)
						world[x,y].borderTiles[(int)border.Up] = world[x,y-1];
					
					if(y + 1 < world.GetLength(1))
						world[x,y].borderTiles[(int)border.Down] = world[x,y+1];
				}
			}
		}

		void splitTiles(){
			// Note: Unity imports textures and resizes them to powers of 2

			Color[] pixles = terrain.GetPixels(64, 480, tileResolution, tileResolution); 
			tiles[(int)tileTypes.Grass] = new WorldTile (pixles, gatheringTypes.NotGathering, true, true);

			pixles = terrain.GetPixels(64 + 32, 480, tileResolution, tileResolution); 
			tiles[(int)tileTypes.Water] = new WorldTile (pixles, gatheringTypes.NotGathering, false, false);


		}

		void BuildTexture(){
			stopWatch.Start();
			splitTiles();

			int texWidth = size_x * tileResolution;
			int texHeight = size_z * tileResolution;

			world = new WorldTile[size_x,size_z];
			Texture2D texture = new Texture2D(texWidth, texHeight);

			fillWithGrass();

			for (int y=0; y<size_z; y++){
				for (int x=0; x<size_x; x++){
					texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, world[x,y].pixles);
					//Instantiate(FogOfWarPrefab, world[x,y].position, Quaternion.Euler(new Vector3(90, 0, 0)));
				}
			}

			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.Apply();
			
			MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
			mesh_renderer.sharedMaterials[0].mainTexture = texture;

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			                                   ts.Hours, ts.Minutes, ts.Seconds,
			                                   ts.Milliseconds / 10);
			stopWatch.Reset();
			UnityEngine.Debug.Log ("Done Texture: "+ elapsedTime);

			connectMap();
			fillResources();
		}

		void fillWithGrass(){
			for (int y=0; y<size_z; y++){
				for (int x=0; x<size_x; x++){
				
						world[(int)x,(int)y] = tiles[(int)tileTypes.Grass].clone();
						world[(int)x,(int)y].setPosition(toUnityCoordnates(new Vector3(x + (tileSize / 2), 0, y + (tileSize / 2))));

					if(y<10 +UnityEngine.Random.Range(0, 5) ){
						InstantiateObject(WaterPrefab, world[(int)x,(int)y].position);
						
					}
				}
			}
		}

		void makeAgents(GameObject Resource, double basePercent, double deltaPercent, int numberToProduce, List<Agent> agents){
			int baseNumber = (int)(size_x * size_z * basePercent);
			int delta = (int)(baseNumber * deltaPercent);
			int numberOfAgents = UnityEngine.Random.Range(baseNumber-delta, baseNumber+delta);

			for (int x=0; x<numberOfAgents; x++){
				WorldTile startingTile = world[UnityEngine.Random.Range(0, size_x), UnityEngine.Random.Range(0, size_z)];
				agents.Add( new Agent(startingTile, Resource, numberToProduce, this));
			}
		}

		void makeWater(int numberToProduce){
			for (int x=0; x<numberToProduce; x++){
				WorldTile startingTile =  _eventHandler.findWalkableTile(world[0, 0]);
				InstantiateObject(WaterPrefab, startingTile.position);
			}
		}

		public void InstantiateObject(GameObject resource, Vector3 position){
			Instantiate(resource, position, Quaternion.Euler(new Vector3(90, 0, 0)));
		}


		void fillResources(){
			stopWatch.Start();
			List<Agent> agents = new List<Agent>();
			makeAgents(TreePrefab, .001, .05, UnityEngine.Random.Range(90, 120), agents);

			//makeAgents((WorldTile)tiles[(int)tileTypes.Mountain].clone(),
			//           .001, .5, Random.Range(20, 40), agents);

			//makeWaterAgents(WaterPrefab,
			//           UnityEngine.Random.Range(900, 1000), agents);

			foreach (Agent agent in agents){
				agent.move();
			}

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			                                   ts.Hours, ts.Minutes, ts.Seconds,
			                                   ts.Milliseconds / 10);
			stopWatch.Reset();
			UnityEngine.Debug.Log ("Done recources: "+ elapsedTime);
		}

		public float changeRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax){
			float newValue = (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
			return newValue;

		}

		public Vector3 toUnityCoordnates(Vector3 coordnates){

			return new Vector3(coordnates.x,
					           coordnates.y,
					           -(size_z - coordnates.z));
		}

		public Vector3 toTileMapCoordnates(Vector3 coordnates){
			
			return new Vector3(coordnates.x,
			                   coordnates.y,
			                   (size_z  + coordnates.z));
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

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			                                   ts.Hours, ts.Minutes, ts.Seconds,
			                                   ts.Milliseconds / 10);
			stopWatch.Reset();
			UnityEngine.Debug.Log("Done Mesh: "+elapsedTime);


			BuildTexture();
		}
	}
}

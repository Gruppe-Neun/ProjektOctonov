﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realtime.Messaging.Internal;
using System.IO;
using UnityEngine.AI;

/// <summary>
/// The world MonoBehavior is in charge of creating, updating and destroying chunks based on the player's location.
/// These mechanisms are completed with the help of Coroutines (IEnumerator methods). https://docs.unity3d.com/Manual/Coroutines.html
/// </summary>
public class World : MonoBehaviour
{
	public GameObject player;
	public Material textureAtlas;
	public Material fluidTexture;
	public static int columnHeight = 16;
	public static int chunkSize = 8;
	public static int radius = 4;
	public static uint maxCoroutines = 1000;
	public static ConcurrentDictionary<string, Chunk> chunks;
	public static List<string> toRemove = new List<string>();
    public string LevelName = "default";

	public static CoroutineQueue queue;

	public Vector3 lastbuildPos;

    private NavMeshBaker navMeshBaker = new NavMeshBaker();

    //everything need to be loaded first
    private void Awake() {
        Item.loadSprites();
        Item.loadModels();
        Crafting.loadRecipes("Assets/Recipes.txt");
    }

    /// <summary>
    /// Creates a name for the chunk based on its position
    /// </summary>
    /// <param name="v">Position of tje chunk</param>
    /// <returns>Returns a string witht he chunk's name</returns>
	public static string BuildChunkName(Vector3 v)
	{
		return (int)v.x + "_" + 
			         (int)v.y + "_" + 
			         (int)v.z;
	}

    /// <summary>
    /// Creates a name for the column based on its position
    /// </summary>
    /// <param name="v">Position of the column</param>
    /// <returns>Returns a string witht he column's name</returns>
	public static string BuildColumnName(Vector3 v)
	{
		return (int)v.x + "_" + (int)v.z;
	}

    /// <summary>
    /// Get block based on world coordinates
    /// </summary>
    /// <param name="pos">Rough position of the block to be returned</param>
    /// <returns>Returns the block related to the input position</returns>
	public static Block GetWorldBlock(Vector3 pos)
	{
        // Cast float to int to specify the actual chunk and block, which might got hit a by a raycast
        // Chunk
		int cx = (int) (Mathf.Round(pos.x)/(float)chunkSize) * chunkSize;
		int cy = (int) (Mathf.Round(pos.y)/(float)chunkSize) * chunkSize;
		int cz = (int) (Mathf.Round(pos.z)/(float)chunkSize) * chunkSize;

        // Block
		int blx = (int) (Mathf.Round(pos.x) - cx);
		int bly = (int) (Mathf.Round(pos.y) - cy);
		int blz = (int) (Mathf.Round(pos.z) - cz);

        // Create chunk name 
		string cn = BuildChunkName(new Vector3(cx,cy,cz));
		Chunk c;
        // Find block in chunk
		if(chunks.TryGetValue(cn, out c))
		{
			return c.chunkData[blx,bly,blz];
		}
		else
			return null;
	}

    public static Chunk GetWorldChunk(Vector3 pos) {

        int cx = (int)(Mathf.Round(pos.x) / (float)chunkSize) * chunkSize;
        int cy = (int)(Mathf.Round(pos.y) / (float)chunkSize) * chunkSize;
        int cz = (int)(Mathf.Round(pos.z) / (float)chunkSize) * chunkSize;



        string cn = BuildChunkName(new Vector3(cx, cy, cz));

        Chunk c;
        if (chunks.TryGetValue(cn, out c)) {
            return c;
        } else {
            return null;
        }

    }

    /// <summary>
    /// Instantiates a new chunk at a specified location.
    /// </summary>
    /// <param name="x">y position of the chunk</param>
    /// <param name="y">y position of the chunk</param>
    /// <param name="z">z position of the chunk</param>
	private void BuildChunkAt(int x, int y, int z)
	{
		Vector3 chunkPosition = new Vector3(x*chunkSize, 
											y*chunkSize, 
											z*chunkSize);
					
		string n = BuildChunkName(chunkPosition);
		Chunk c;

		if(!chunks.TryGetValue(n, out c))
		{
			c = new Chunk(LevelName,chunkPosition, textureAtlas, fluidTexture);
			c.chunk.transform.parent = this.transform;
			c.fluid.transform.parent = this.transform;
            c.chunk.AddComponent(typeof(NavMeshSurface));
            c.chunk.GetComponent<NavMeshSurface>().agentTypeID = -1372625422;
            navMeshBaker.addSurface(c.chunk.GetComponent<NavMeshSurface>());
			chunks.TryAdd(c.chunk.name, c);
		}
	}

    /// <summary>
    /// Coroutine to to recursively build chunks of the world depending on some location and a radius.
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <param name="z">z position</param>
    /// <param name="startrad">Starting radius (is necessary for recursive calls of this function)</param>
    /// <param name="rad">Desired radius</param>
    /// <returns></returns>
	IEnumerator BuildRecursiveWorld(int x, int y, int z, int startrad, int rad)
	{
		int nextrad = rad-1;
		if(rad <= 0 || y < 0 || y > columnHeight) yield break;
		// Build chunk front
		BuildChunkAt(x,y,z+1);
		queue.Run(BuildRecursiveWorld(x,y,z+1,rad,nextrad));
		yield return null;

		// Build chunk back
		BuildChunkAt(x,y,z-1);
		queue.Run(BuildRecursiveWorld(x,y,z-1,rad,nextrad));
		yield return null;
		
		// Build chunk left
		BuildChunkAt(x-1,y,z);
		queue.Run(BuildRecursiveWorld(x-1,y,z,rad,nextrad));
		yield return null;

		// Build chunk right
		BuildChunkAt(x+1,y,z);
		queue.Run(BuildRecursiveWorld(x+1,y,z,rad,nextrad));
		yield return null;
		
		// Build chunk up
		BuildChunkAt(x,y+1,z);
		queue.Run(BuildRecursiveWorld(x,y+1,z,rad,nextrad));
		yield return null;
		
		// Build chunk down
		BuildChunkAt(x,y-1,z);
		queue.Run(BuildRecursiveWorld(x,y-1,z,rad,nextrad));
		yield return null;
	}

    /// <summary>
    /// Coroutine to render chunks that are in the DRAW state. Adds chunks to the toRemove list, which are outside the player's radius.
    /// </summary>
    /// <returns></returns>
	IEnumerator DrawChunks()
	{
		toRemove.Clear();
		foreach(KeyValuePair<string, Chunk> c in chunks)
		{
			if(c.Value.status == Chunk.ChunkStatus.DRAW) 
			{
				c.Value.DrawChunk();
			}
			if(c.Value.chunk && Vector3.Distance(player.transform.position,
								c.Value.chunk.transform.position) > radius*chunkSize)
				toRemove.Add(c.Key);

			yield return null;
		}
	}

    /// <summary>
    /// Coroutine to save and then to unload unused chunks.
    /// </summary>
    /// <returns></returns>
	IEnumerator RemoveOldChunks()
	{
		for(int i = 0; i < toRemove.Count; i++)
		{
			string n = toRemove[i];
			Chunk c;
			if(chunks.TryGetValue(n, out c))
			{
				Destroy(c.chunk);
				c.Save();
				chunks.TryRemove(n, out c);
				yield return null;
			}
		}
	}

    /// <summary>
    /// Builds chunks that are inside the player's radius.
    /// </summary>
	public void BuildNearPlayer()
	{
        // Stop the coroutine of building the world, because it is getting replaced
		StopCoroutine("BuildRecursiveWorld");
		queue.Run(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
											(int)(player.transform.position.y/chunkSize),
											(int)(player.transform.position.z/chunkSize), radius, radius));
	}

    private void loadLevel(string name) {
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/leveldata/" + name + "/");
        foreach (string filePath in filePaths) {
            if (Path.GetExtension(filePath).Equals(".dat")) {
                string[] coordinates = filePath.Substring(filePath.LastIndexOf('/') + 1).Split('_');
                int x = System.Convert.ToInt32(coordinates[1]), y = System.Convert.ToInt32(coordinates[2]), z = System.Convert.ToInt32(coordinates[3]);
                BuildChunkAt((int)x / chunkSize, (int)y / chunkSize, (int)z / chunkSize);
            } 
        }
    }

	/// <summary>
    /// Unity lifecycle start method. Initializes the world and its first chunk and triggers the building of further chunks.
    /// Player is disabled during Start() to avoid him falling through the floor. Chunks are built using coroutines.
    /// </summary>
	void Start ()
    {

        player.SetActive(false);
        


        
        lastbuildPos = player.transform.position;
        player.SetActive(false);
		chunks = new ConcurrentDictionary<string, Chunk>();
		this.transform.position = Vector3.zero;
		this.transform.rotation = Quaternion.identity;	

		queue = new CoroutineQueue(maxCoroutines, StartCoroutine);

        loadLevel(LevelName);
        navMeshBaker.buildNavMesh();

        foreach (KeyValuePair<string, Chunk> c in chunks) {
            if (c.Value.status == Chunk.ChunkStatus.DRAW) {
                c.Value.DrawChunk();
            }
        }



        player.SetActive(true);


        /*
        // Build starting chunk
        BuildChunkAt((int)(player.transform.position.x/chunkSize),
											(int)(player.transform.position.y/chunkSize),
											(int)(player.transform.position.z/chunkSize));
		// Draw starting chunk
		queue.Run(DrawChunks());

		// Create further chunks
		queue.Run(BuildRecursiveWorld((int)(player.transform.position.x/chunkSize),
											(int)(player.transform.position.y/chunkSize),
											(int)(player.transform.position.z/chunkSize),radius,radius));

        */
	}
	
    /// <summary>
    /// Unity lifecycle update method. Actviates the player's GameObject. Updates chunks based on the player's position.
    /// </summary>
	void Update ()
    {

        /*
        // Determine whether to build/load more chunks around the player's location
		Vector3 movement = lastbuildPos - player.transform.position;

        
		if(movement.magnitude > chunkSize )
		{
			lastbuildPos = player.transform.position;
			BuildNearPlayer();
		}
        */

        // Draw new chunks and removed deprecated chunks
		queue.Run(DrawChunks());

	}
}

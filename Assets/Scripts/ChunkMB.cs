using System.Collections;
using UnityEngine;

/// <summary>
/// MonoBehavior of a chunk' GameObject to process Coroutines.
/// </summary>
public class ChunkMB: MonoBehaviour
{
	Chunk owner;
	public ChunkMB(){}

    /// <summary>
    /// Assigns the reference to the chunk who possesses this MonoBehavior.
    /// </summary>
    /// <param name="o"></param>
	public void SetOwner(Chunk o)
	{
		owner = o;
		//InvokeRepeating("SaveProgress",10,1000);
	}
}

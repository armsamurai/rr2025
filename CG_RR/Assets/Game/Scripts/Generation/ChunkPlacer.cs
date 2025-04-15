using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform player;
    public Chunk[] chunkPrefabs;
    public Chunk firstChunk;

    List<Chunk> spawnedChunks = new List<Chunk>();

    void Start()
    {
        spawnedChunks.Add(firstChunk);

        for (int i = 0; i < 3; i++)
        {
            SpawnChunk();
        }
    }
    
    void Update()
    {
        if (player.position.x > spawnedChunks[spawnedChunks.Count - 3].end.position.x - 45)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        Chunk newChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);

        newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
        spawnedChunks.Add(newChunk);

        if (spawnedChunks.Count > 4)
        {
            Destroy(spawnedChunks[0].gameObject);
            spawnedChunks.RemoveAt(0);
        }
    }
}

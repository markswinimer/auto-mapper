using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map/Layout")]
public class MapGenerationData : ScriptableObject
{

    public Vector2Int MapSize;
    public Vector3Int TileSize;

    [Serializable]
    private class MapTileSetup
    {
        public int tileAmount;
        public TileType tileType;
        public GameObject[] tilePrefabs;
    }

    [SerializeField] private MapTileSetup[] mapTiles;

    public int GetTileCount()
    {
        return mapTiles.Length;
    }
    
    public TileType GetTileTypeAtIndex(int index)
    {
        if ( index < 0 || index >= mapTiles.Length )
        {
            Debug.LogError("Tile index out of range");
            return TileType.Empty;
        }
        return mapTiles[index].tileType;
    }

    public int GetTileAmountAtIndex(int index)
    {
        if (mapTiles.Length <= index)
        {
            Debug.LogError("Tile index out of range");
            return -1;
        }
        return mapTiles[index].tileAmount;
    }

    public GameObject[] GetTilePrefabsAtIndex(int index)
    {
        if (index < 0 || index >= mapTiles.Length)
        {
            Debug.LogError("Tile index out of range");
            return null;  // Return null to indicate out of range
        }
        return mapTiles[index].tilePrefabs;
    }

}
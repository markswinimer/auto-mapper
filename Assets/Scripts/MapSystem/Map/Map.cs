using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {
    public Grid _grid;
    public Vector2Int MapWorldDimensions {get; set;}
    private Dictionary<Vector2Int, Tile> _tiles;

    public static Map Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        _tiles = new Dictionary<Vector2Int, Tile>();
    }

    public Vector3 GetPositionFromCoords(Vector2Int coords)
    {
        Vector3Int worldCoords = new Vector3Int(coords.x, 1, coords.y);
        return _grid.GetCellCenterWorld(worldCoords);
    }

    public Tile GetTileAtCoordinate(Vector2Int coordinate)
    {
        _tiles.TryGetValue(coordinate, out Tile tile);
        return tile;
    }

    public void AddTile(Vector2Int coordinate, Tile tile)
    {
        _tiles[coordinate] = tile;
    }

    public void SetMapWorldDimensions()
    {

    }

    public Vector2Int GetMapWorldDimensions()
    {
    
        Debug.Log(_grid);
        return new Vector2Int(1,1);
    }
}
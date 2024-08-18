using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapGenerationData mapGenerationData;
    [SerializeField] private GameObject tilePrefab;

    private Map _map;
    private Grid _grid;
    private Vector2Int _mapSize;
    private Vector3Int _tileSize;
    private bool _isCompleted;

    private Dictionary<TileType, GameObject[]> prefabDictionary;

    public void GenerateMap()
    {
        Debug.Log("Starting map generation...");

        SetDependencies();
        Debug.Log("Dependencies set.");

        CreateMapObject();
        Debug.Log("Map object created.");

        GenerateTiles();
        Debug.Log("Tiles generated.");
        
        MapManager.Instance.UpdateMapState(MapState.MapInstantiated);
    }

    private void CreateMapObject()
    {
        GameObject mapObject = new GameObject("Map");

        // consider breaking this into a map script init function 
        // Add the Map script to the new GameObject
        _map = mapObject.AddComponent<Map>();

        // Add the Grid component to the same GameObject
        _grid = mapObject.AddComponent<Grid>();
        _map.Grid = _grid;

        _mapSize = mapGenerationData.MapSize;
        _tileSize = mapGenerationData.TileSize;

        _map.MapWorldDimensions = new Vector2Int(_mapSize.x * _tileSize.x, _mapSize.y * _tileSize.z);
        
        float mapCenterX = _map.MapWorldDimensions.x / 2;
        float mapCenterY = _map.MapWorldDimensions.y / 2;
        
        _map.MapCenter = new Vector3(mapCenterX, 0, mapCenterY);
        
        _grid.cellSize = new Vector3Int(_tileSize.x, 1, _tileSize.z);

        _grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
        _grid.cellLayout = GridLayout.CellLayout.Rectangle;
    }

    private void SetDependencies()
    {
        prefabDictionary = new Dictionary<TileType, GameObject[]>();
    }

    private void GenerateTiles()
    {
        var coordinates = GenerateGridCoordinates();
        Debug.Log(coordinates);
        var shuffledCoordinates = ShuffleCoordinates(coordinates);

        Dictionary<TileType, int> tileCounts = GetTileCountsAndPrefabs();

        PlaceTiles(shuffledCoordinates, tileCounts);
    }

    private List<Vector2Int> GenerateGridCoordinates()
    {

        var coordinates = new List<Vector2Int>();
        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                coordinates.Add(new Vector2Int(x, y));
            }
        }
        return coordinates;
    }

    private IEnumerable<Vector2Int> ShuffleCoordinates(List<Vector2Int> coordinates)
    {
        var rand = new System.Random(420);
        return coordinates.OrderBy(t => rand.Next());
    }

    // this information is set in the unity editor
    // and is defined by the scriptable object MapGenerationData
    // RoomCount, Prefab Arrays, Room Types 
    private Dictionary<TileType, int> GetTileCountsAndPrefabs()
    {
        var tileCounts = new Dictionary<TileType, int>();

        for (int i = 0; i < mapGenerationData.GetTileCount(); i++)
        {
            TileType type = mapGenerationData.GetTileTypeAtIndex(i);
            int count = mapGenerationData.GetTileAmountAtIndex(i);
            GameObject[] tilePrefabs = mapGenerationData.GetTilePrefabsAtIndex(i);

            if (!prefabDictionary.ContainsKey(type))
            {
                prefabDictionary.Add(type, tilePrefabs);
            }

            if (count > 0)
            {
                tileCounts[type] = count;
            }
        }
        return tileCounts;
    }

    private void PlaceTiles(IEnumerable<Vector2Int> coordinates, Dictionary<TileType, int> tileCounts)
    {
        foreach (var coordinate in coordinates)
        {
            if (tileCounts.Count == 0)
            {
                break;
            }

            foreach (var tileType in tileCounts.Keys.ToList())
            {
                if (tileCounts[tileType] > 0)
                {
                    CreateTileAndInitialize(coordinate, tileType);
                    tileCounts[tileType]--;
                    if (tileCounts[tileType] == 0)
                    {
                        tileCounts.Remove(tileType);
                    }
                    break;
                }
            }
        }
    }

    private GameObject GetPrefabByTileType(TileType tileType)
    {
        if (prefabDictionary.ContainsKey(tileType))
        {
            return prefabDictionary[tileType][UnityEngine.Random.Range(0, prefabDictionary[tileType].Length)];
        }
        else
        {
            Debug.LogError("Room type not found in dictionary: " + tileType);
            return null;
        }
    }

    public void CreateTileAndInitialize(Vector2Int coords, TileType tileType)
    {
        Vector3Int worldCoords = new Vector3Int(coords.x, 0, coords.y);
        Debug.Log("Creating tile at " + coords + " with type " + tileType);

        var position = _grid.GetCellCenterWorld(worldCoords);
        Debug.Log("World Position: " + position);

        GameObject tileViewPrefab = GetPrefabByTileType(tileType);
        string tileName = "tile_" + coords.x.ToString() + "_" + coords.y.ToString() + "_" + tileType.ToString();

        // Instantiate the tile prefab
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, _map.transform);
        tile.name = tileName;

        // Get the Tile component and initialize it
        Tile tileComponent = tile.GetComponent<Tile>();
        if (tileComponent != null)
        {
            // Initialize the Tile component with data and the TileView prefab
            tileComponent.Initialize(coords, tileType, tileViewPrefab);
        }
        else
        {
            Debug.LogError("Tile prefab does not have a Tile component attached.");
        }
    }
}
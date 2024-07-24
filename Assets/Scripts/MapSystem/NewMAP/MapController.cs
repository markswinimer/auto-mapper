using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;


[RequireComponent(typeof(RoomCreator))]
public class MapController : MonoBehaviour {
    [SerializeField] private MapLayout startingMapLayout;
    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private Map map;

    private Grid _grid;
    
    private RoomCreator roomCreator;

    private Dictionary<string, GameObject[]> prefabDictionary;

    private void Awake()
    {
        SetDependencies();
        prefabDictionary = new Dictionary<string, GameObject[]>();
        ConfigMapCamera();
    }

    private void SetDependencies()
    {
        //map has a grid component on it, so we can get it
        _grid = map.GetComponent<Grid>();
        roomCreator = GetComponent<RoomCreator>();
    }
    private void ConfigMapCamera()
    {
        float width = startingMapLayout.MapSize.x * startingMapLayout.RoomTileSize.x;
        float height = startingMapLayout.MapSize.y * startingMapLayout.RoomTileSize.y;
        mapCamera.SetCameraTargetPosition(width, height);
    }

    private void Start()
    {
        InitMap();
    }

    private void InitMap()
    {
        CreateMapFromLayout(startingMapLayout);
    }
    private void CreateMapFromLayout(MapLayout layout)
    {
        // Set from options
        Vector2Int _size = layout.MapSize;
        _grid.cellSize = layout.RoomTileSize;

        // Force specific defaults on these
        _grid.cellLayout = GridLayout.CellLayout.Rectangle;
        _grid.cellSwizzle = GridLayout.CellSwizzle.XZY;

        // Initialize coordinates list to store grid positions
        var coordinates = new List<Vector3Int>();

        // Generate grid coordinates
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                coordinates.Add(new Vector3Int(x, y, 0)); // Adding third dimension for 3D grid handling
            }
        }

        // Shuffle coordinates and take the same number as rooms to be created
        var rand = new System.Random(420);
        var shuffledCoordinates = coordinates.OrderBy(t => rand.Next());

        // Create a dictionary to manage the counts of each room type
        Dictionary<string, int> roomCounts = new Dictionary<string, int>();

        for (int i = 0; i < layout.GetRoomCount(); i++)
        {
            string type = layout.GetRoomTypeAtIndex(i);
            int count = layout.GetRoomAmountAtIndex(i);
            GameObject[] roomPrefabs = layout.GetRoomPrefabsAtIndex(i);

            // Check for duplicate room types and add prefabs to dictionary
            if (!prefabDictionary.ContainsKey(type))
            {
                prefabDictionary.Add(type, roomPrefabs);
            }
            else
            {
                Debug.LogWarning("Duplicate room type found, updating prefabs: " + type);
            }

            // Add room counts to dictionary
            if (count > 0)
            {
                roomCounts[type] = count;
            }
        }
        // Use the shuffled coordinates to place rooms
        foreach (var coordinate in shuffledCoordinates)
        {
            if (roomCounts.Count == 0)
            {
                break; // Stop if all room types have been placed according to their counts
            }

            foreach (var roomType in roomCounts.Keys.ToList()) // ToList to modify collection in loop
            {
                if (roomCounts[roomType] > 0)
                {
                    // get the roomprefab type
                    CreateRoomAndInitialize(coordinate, roomType);
                    roomCounts[roomType]--;
                    // Instantiate(roomPrefabs[(int)roomType], _grid.CellToLocal(coordinate), Quaternion.identity);

                    // Break after placing one room to move to the next coordinate
                    break;
                }
                else
                {
                    // Remove the room type from dictionary if its count reaches zero
                    roomCounts.Remove(roomType);
                }
            }
        }
    }

    private GameObject GetPrefab(string roomType)
    {
        if (prefabDictionary.ContainsKey(roomType))
        {
            return prefabDictionary[roomType][UnityEngine.Random.Range(0, prefabDictionary[roomType].Length)];
        }
        else
        {
            Debug.LogError("Room type not found in dictionary: " + roomType);
            return null;
        }
    }
    //not implemented yet
    private void CreateRoomAndInitialize(Vector3Int coords, string roomType)
    {
        Debug.Log("Creating room at " + coords + " with type " + roomType);
        var position = _grid.GetCellCenterWorld(coords);
        Room newRoom = roomCreator.CreateRoom(position).GetComponent<Room>();

        GameObject selectedPrefab = GetPrefab(roomType);
        RoomState roomState = RoomState.Unvisited;
        newRoom.SetData(coords, roomType, roomState, selectedPrefab, map);
    }
}
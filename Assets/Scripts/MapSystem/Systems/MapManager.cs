using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(RoomCreator))]
public class MapManager : MonoBehaviour
{
    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string spriteSheetName;
    [SerializeField] private MapGenerator mapGenerator;

    private Dictionary<string, GameObject[]> prefabDictionary;
    private Dictionary<Vector3Int, Room> rooms;
    private Map map;

    private void Awake()
    {
        Map existingMap = FindObjectOfType<Map>();

        if (existingMap == null)
        {
            mapGenerator.GenerateMap();
        } 
        else
        {
            // need to create
            // mapLoader.LoadMap();
        } 

        // SetDependencies();
        // InitializePrefabDictionary();
        // ConfigMapCamera();
    }

    private void SetDependencies()
    {
        // _grid = map.GetComponent<Grid>();
        // roomCreator = GetComponent<RoomCreator>();
    }

    private void InitializePrefabDictionary()
    {
        prefabDictionary = new Dictionary<string, GameObject[]>();
    }

//     private void ConfigMapCamera()
//     {
//         float width = startingMapLayout.MapSize.x * startingMapLayout.RoomTileSize.x;
//         float height = startingMapLayout.MapSize.y * startingMapLayout.RoomTileSize.y;
//     }

//     private void Start()
//     {
//         InitMap();
//     }

//     private void InitMap()
//     {
//         CreateMapFromLayout(startingMapLayout);
//     }

//     private void CreateMapFromLayout(MapLayout layout)
//     {
//         Vector2Int mapSize = layout.MapSize;
//         _grid.cellSize = layout.RoomTileSize;
//         _grid.cellLayout = GridLayout.CellLayout.Rectangle;
//         _grid.cellSwizzle = GridLayout.CellSwizzle.XZY;

//         rooms = new Dictionary<Vector3Int, Room>();

//         var coordinates = GenerateGridCoordinates(mapSize);
//         var shuffledCoordinates = ShuffleCoordinates(coordinates);

//         Dictionary<string, int> roomCounts = GetRoomCountsAndPrefabs(layout);

//         PlaceRooms(shuffledCoordinates, roomCounts);
//     }

//     private List<Vector3Int> GenerateGridCoordinates(Vector2Int mapSize)
//     {
//         var coordinates = new List<Vector3Int>();
//         for (int x = 0; x < mapSize.x; x++)
//         {
//             for (int y = 0; y < mapSize.y; y++)
//             {
//                 coordinates.Add(new Vector3Int(x, y, 0));
//             }
//         }
//         return coordinates;
//     }

//     private IEnumerable<Vector3Int> ShuffleCoordinates(List<Vector3Int> coordinates)
//     {
//         var rand = new System.Random(420);
//         return coordinates.OrderBy(t => rand.Next());
//     }

//     private Dictionary<string, int> GetRoomCountsAndPrefabs(MapLayout layout)
//     {
//         var roomCounts = new Dictionary<string, int>();
//         for (int i = 0; i < layout.GetRoomCount(); i++)
//         {
//             string type = layout.GetRoomTypeAtIndex(i);
//             int count = layout.GetRoomAmountAtIndex(i);
//             GameObject[] roomPrefabs = layout.GetRoomPrefabsAtIndex(i);

//             if (!prefabDictionary.ContainsKey(type))
//             {
//                 prefabDictionary.Add(type, roomPrefabs);
//             }

//             if (count > 0)
//             {
//                 roomCounts[type] = count;
//             }
//         }
//         return roomCounts;
//     }

//     private void PlaceRooms(IEnumerable<Vector3Int> coordinates, Dictionary<string, int> roomCounts)
//     {
//         foreach (var coordinate in coordinates)
//         {
//             if (roomCounts.Count == 0)
//             {
//                 break;
//             }

//             foreach (var roomType in roomCounts.Keys.ToList())
//             {
//                 if (roomCounts[roomType] > 0)
//                 {
//                     CreateRoomAndInitialize(coordinate, roomType);
//                     roomCounts[roomType]--;
//                     if (roomCounts[roomType] == 0)
//                     {
//                         roomCounts.Remove(roomType);
//                     }
//                     break;
//                 }
//             }
//         }
//     }

//     private GameObject GetPrefab(string roomType)
//     {
//         if (prefabDictionary.ContainsKey(roomType))
//         {
//             return prefabDictionary[roomType][UnityEngine.Random.Range(0, prefabDictionary[roomType].Length)];
//         }
//         else
//         {
//             Debug.LogError("Room type not found in dictionary: " + roomType);
//             return null;
//         }
//     }

//     private void CreateRoomAndInitialize(Vector3Int coords, string roomType)
//     {
//         Debug.Log("Creating room at " + coords + " with type " + roomType);
//         var position = _grid.GetCellCenterWorld(coords);
//         Room newRoom = roomCreator.CreateRoom(position).GetComponent<Room>();

//         GameObject selectedPrefab = GetPrefab(roomType);
//         RoomState roomState = RoomState.Unvisited;
//         newRoom.SetData(coords, roomType, roomState, selectedPrefab, map);

//         rooms.Add(coords, newRoom);

//         AddIconAboveRoom(newRoom);
//     }

//     private void AddIconAboveRoom(Room room)
//     {
//         // Create a new VisualElement for the icon
//         VisualElement icon = new VisualElement();
//         icon.AddToClassList("icon-style"); // Assuming you have a class in your USS for the icon style

//         // Add the icon to the iconContainer
//         iconContainer.Add(icon);

//         // Convert the room's world position to screen position
//         Vector3 screenPos = Camera.main.WorldToScreenPoint(room.transform.position);

//         // Set the icon's position in the UI
//         icon.style.left = screenPos.x;
//         icon.style.top = Screen.height - screenPos.y; // UI coordinates have (0,0) at top left

//         // Optional: Store reference to the icon in the room object if you need to update it later
//         // room.SetIcon(icon);
//     }


//     public Room GetRoomAt(Vector3Int coordinates)
//     {
//         foreach (var roomEntry in rooms)
//         {
//             Vector3Int roomCoords = roomEntry.Key;
//             Room roomy = roomEntry.Value;
//             Debug.Log("Room at " + roomCoords + ": " + roomy);
//         }
//         if (rooms.TryGetValue(coordinates, out Room room))
//         {
//             Debug.Log("found at coords: " + coordinates);
//             Debug.Log("FOUND ROOM: " + room);
//             return room;
//         }
//         else
//         {
//             Debug.LogError("No room found at coordinates: " + coordinates);
//             return null;
//         }
//     }

//     public Dictionary<Vector3Int, Room> GetAllRooms()
//     {
//         return rooms;
//     }

//     public Vector3 GetWorldPosition(Vector2Int gridPosition)
//     {
//         return _grid.GetCellCenterWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
//     }

//     public Vector3Int GetGridPosition(Vector3 worldPosition)
//     {
//         return _grid.WorldToCell(worldPosition);
//     }
}

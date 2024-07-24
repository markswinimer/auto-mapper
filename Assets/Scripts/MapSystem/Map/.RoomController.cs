using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;
    public int x;
    public int y;
    public string type;
}

// this file will:
// - load rooms
// - keep track of loaded rooms
// - control scene loading
// - keep track of room states?
public class RoomController : MonoBehaviour
{
    // this singleton allows only one instance of the class to exist
    public static RoomController instance;

    string currentRegion = "Jungle";
    RoomInfo currentLoadRoomData;

    //this will be used to load scenes
    // Queue<RoomSceneInfo> loadRoomSceneQueue = new Queue<RoomSceneInfo>();
    // public List<RoomScene> loadedRoomScenes = new List<RoomScene>();
    // bool isLoadingRoom = false;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    public Room[,] roomGrid;
    public Vector2Int finalGridSize = Vector2Int.zero;

// try dictionary approach
    void Awake()
    {
        instance = this;
    }
    
    public void Start()
    {
        //

    }

    public void Update()
    {
        //this will be used when scenes need to be loaded in.
        // UpdateRoomQueue();
    }

    // void UpdateRoomQueue()
    // {
    //     if (isLoadingRoom)
    //     {
    //         return;
    //     }
    //     if (loadRoomQueue.Count == 0)
    //     {
    //         return;
    //     }
    //     currentLoadRoomData = loadRoomQueue.Dequeue();
    //     isLoadingRoom = true;
    //     //here is where the scene will be loaded. disable until that part is ready
    //     StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    // }

    // public void LoadRoom(string name, int x, int y)
    public void LoadRoom(string name, int x, int y, Vector3 position, Room prefab)
    {
        if ( DoesRoomExist(x, y) )
        {
            return;
        }
        Room newRoom = Instantiate(prefab, position, Quaternion.Euler(0, 90f, 0), transform);

        GameObject tile = new GameObject($"Tile_{i}_{j}");
        tile.transform.position = new Vector3(i, 0, j); // Set position; adjust as needed

        // Instantiate the black tile prefab as a child of the tile
        GameObject blackTile = Instantiate(blackTilePrefab, tile.transform);

        // Optionally instantiate the actual room prefab but disable it
        GameObject room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], tile.transform);
        room.SetActive(false); // Disable the room on start

        // Store the parent tile in the grid array for later access
        tiles[i, j] = tile;
        
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;
        loadRoomQueue.Enqueue(newRoomData);

        //branch off here and load scene for the room when clicked into
    }


    // make this handle scene
    // IEnumerator LoadRoomSceneRoutine(RoomInfo info)
    // {
    //     string roomName = currentRegion + "_" + currentLoadRoomData.x + "_" + currentLoadRoomData.y;

    //     // LoadSceneMode.Additive will load the scene into the same scene, overlapping the current scene
    //     AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

    //     while (!loadRoom.isDone)
    //     {
    //         yield return null;
    //     }
    // }

    public void RegisterRoom( Room room )
    {
        currentLoadRoomData = loadRoomQueue.Dequeue();
        room.x = currentLoadRoomData.x;
        room.y = currentLoadRoomData.y;
        room.name = currentRegion + "_" + currentLoadRoomData.x + "_" + currentLoadRoomData.y;
        room.transform.parent = transform;
        room.type = currentLoadRoomData.type;
        
        roomGrid[currentLoadRoomData.x, currentLoadRoomData.y] = room;
        loadedRooms.Add(room);
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find( item => item.x == x && item.y == y) != null;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.Media;
// using UnityEngine;
// using UnityEngine.UIElements;

// namespace MapPlayer
// {
//     public class PlayerPositionController : MonoBehaviour
//     {
//         public MapController mapController;
//         public GameObject playerPrefab;
//         private GameObject playerInstance;
//         public Vector2Int currentPlayerPosition;
//         public GameState gameState = GameState.ChoosingTile;
//         private UIDocument ui;

//         void Start()
//         {
//             // Set player to bottom left coord
//             currentPlayerPosition = new Vector2Int(0, 0);
//             SpawnPlayerAtPosition(currentPlayerPosition);
//             UpdateTileStates();
//             ui = GetComponent<UIDocument>();
//         }

//         void Update()
//         {
//             if (gameState == GameState.ChoosingTile && Input.GetMouseButtonDown(0))
//             {
//                 GameObject clickedObject = MousePosition3D.GetMouseHitObject();
//                 if (clickedObject != null)
//                 {
//                     Room clickedRoom = clickedObject.GetComponentInParent<Room>();
//                     if (clickedRoom != null)
//                     {
//                         Vector2Int clickedTile = new Vector2Int(clickedRoom.mapCoords.x, clickedRoom.mapCoords.y);
//                         Debug.Log("Clicked tile position: " + clickedTile); // Log the clicked tile coordinates
//                         if (IsMoveValid(clickedTile))
//                         {
//                             MovePlayer(clickedTile);
//                             gameState = GameState.CompletingEvent;
//                             StartEventOnTile(clickedTile);
//                         }
//                     }
//                 }
//             }
//         }

//         public void MovePlayer(Vector2Int newPosition)
//         {
//             if (IsMoveValid(newPosition))
//             {
//                 currentPlayerPosition = newPosition;
//                 MovePlayerInstanceToPosition(currentPlayerPosition);
//                 UpdateTileStates();
//             }
//         }

//         bool IsMoveValid(Vector2Int newPosition)
//         {
//             // Validate the move by checking if the room exists and is visitable
//             // im not sure why im checking in vector3 still. only x,z matters. try to simplify?
//             Room newRoom = mapController.GetRoomAt(new Vector3Int(newPosition.x, newPosition.y, 0));
//             return newRoom != null && newRoom.roomState == RoomState.CanVisit;
//         }

//         void UpdateTileStates()
//         {
//             foreach (var roomEntry in mapController.GetAllRooms())
//             {
//                 Room room = roomEntry.Value;
//                 if (room.mapCoords == currentPlayerPosition)
//                 {
//                     room.roomState = RoomState.Visited;
//                 }
//                 else if (IsAdjacent(room.mapCoords, currentPlayerPosition) && room.roomState == RoomState.Unvisited)
//                 {
//                     room.roomState = RoomState.CanVisit;
//                 }
//                 else if (room.roomState != RoomState.Visited)
//                 {
//                     room.roomState = RoomState.CannotVisit;
//                 }
//             }
//         }

//         bool IsAdjacent(Vector2Int pos1, Vector2Int pos2)
//         {
//             // Checks if pos1 and pos2 are adjacent
//             return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) == 1;
//         }

//         void SpawnPlayerAtPosition(Vector2Int position)
//         {
//             Vector3 worldPosition = mapController.GetWorldPosition(position);
//             playerInstance = Instantiate(playerPrefab, worldPosition, Quaternion.identity);
//         }

//         void MovePlayerInstanceToPosition(Vector2Int position)
//         {
//             Vector3 worldPosition = mapController.GetWorldPosition(position);
//             playerInstance.transform.position = worldPosition;
//         }

//         void StartEventOnTile(Vector2Int tilePosition)
//         {
//             Debug.Log(ui);
//             // ui.UpdateMapLog("Event started on tile: " + tilePosition);

//             // Start the event associated with the tile
//             Debug.Log("Event started on tile: " + tilePosition);
//             // Simulate event completion
//             StartCoroutine(CompleteEventAfterDelay(2.0f)); // Replace with actual event logic
//         }

//         IEnumerator CompleteEventAfterDelay(float delay)
//         {
//             yield return new WaitForSeconds(delay);
//             CompleteEvent();
//         }

//         void CompleteEvent()
//         {
//             Debug.Log("Event completed");
//             gameState = GameState.WaitingForNextRound;
//             StartCoroutine(StartNextRoundAfterDelay(1.0f)); // Delay to simulate waiting for next round
//         }

//         IEnumerator StartNextRoundAfterDelay(float delay)
//         {
//             yield return new WaitForSeconds(delay);
//             StartNextRound();
//         }

//         void StartNextRound()
//         {
//             Debug.Log("Starting next round");
//             gameState = GameState.ChoosingTile;
//         }
//     }
// }

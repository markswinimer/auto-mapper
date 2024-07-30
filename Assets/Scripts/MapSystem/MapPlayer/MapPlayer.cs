// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // [RequireComponent(typeof(IObjectTweener))]
// // [RequireComponent(typeof(PrefabSetter))]
// public abstract class MapPlayer : MonoBehaviour 
// {
//     // public RoomController roomController { protected get; set; }
//     public Vector2Int CurrentRoom { get; set; }
//     public bool movementComplete { get; private set;}

//     public List<Vector2Int> availableRooms = new List<Vector2Int>();
    
//     // this will move the player on the map
//     // private IObjectTweener tweener;

//     public abstract List<Vector2Int> SelectAvailableRooms();

//     private void Awake()
//     {
//         availableRooms = new List<Vector2Int>();
//         // tweener = GetComponent<IObjectTweener>();
//         // this could be used to control the party
//         // prefabSetter = GetComponent<PrefabSetter>();
//     }
//     public void SetPrefab(GameObject prefab)
//     {
//         // set the player prefab
//     }
//     public bool CanMoveTo(Vector2Int coords)
//     {
//         return availableRooms.Contains(coords);
//     }
//     public virtual void MoveToRoom(Vector2Int coords)
//     {
//         // move the player to the given coordinates
//     }
//     // protected void TryToMoveToRoom(Vector2Int coords)
//     // {
//     //     // check if the player can move to the given coordinates
//     //     availableMoves.Add(coords);
//     // }
//     // // set the player information up, can add class and whatnot in the future
//     // public void SetData(Vector2Int coords, RoomController roomController)
//     // {
//     //     // set the player data
//     //     this.CurrentRoom = coords;
//     //     this.roomController = roomController;
//     //     transform.position = roomController.GetPositionFrom(coords).GetRoomCentre();
//     // }
// }
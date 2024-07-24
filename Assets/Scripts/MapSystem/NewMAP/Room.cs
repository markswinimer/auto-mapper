using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// [RequireComponent(typeof(IObjectTweener))]
// [RequireComponent(typeof(PrefabSetter))]
public class Room : MonoBehaviour
{
    // public RoomController roomController { protected get; set; }
    public Map map { protected get; set; }
    public Vector2Int mapCoords { get; set; }
    public Vector3Int mapPosition { get; set; }
    public RoomState roomState { get; set; }
    public RoomType roomType { get; set; }

    // this will move the player on the map
    // private IObjectTweener tweener;

    // public List<Vector2Int> SelectAvailableRooms();

    private void Awake()
    {
        // prefabSetter = GetComponent<PrefabSetter>();
    }
    public void SetPrefab(GameObject prefab)
    {
        GameObject child = Instantiate(prefab, this.transform);
        // child.transform.localScale = this.transform.localScale;
        child.transform.localPosition = Vector3.zero;  // Set local position to zero
        child.transform.localRotation = Quaternion.identity;  // Set local rotation to identity
        child.transform.localScale = new Vector3(1, 1, 1);  // Reset local scale


        // GameObject child = Instantiate(prefab, this.transform.position, Quaternion.identity);
        // child.transform.SetParent(this.transform);

        // Since you want the child to be exactly the same size and position as the parent:
        // child.transform.localPosition = Vector3.zero;  // Set local position to zero
        // child.transform.localRotation = Quaternion.identity;  // Set local rotation to identity

        // Optional: Explicitly set the same scale as the parent, only if necessary
    }
    public void SetData(Vector3Int coords, string roomType, RoomState roomState, GameObject roomPrefab, Map map)
    {
        this.name = "Room " + roomType + ", " + coords.x + ", " + coords.y;
        this.map = map;
        this.mapPosition = coords;
        this.mapCoords = new Vector2Int(coords.x, coords.y);
        this.roomState = RoomState.Unvisited;
        this.roomType = (RoomType)System.Enum.Parse(typeof(RoomType), roomType);
        transform.position = map.GetPositionFromCoords(coords);
        SetPrefab(roomPrefab);
        // SetPrefab
    }
}
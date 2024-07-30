using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Map map { protected get; set; }
    public Vector2Int mapCoords { get; set; }
    public Vector3Int mapPosition { get; set; }
    public RoomState roomState { get; set; }
    public RoomType roomType { get; set; }

    private void Awake()
    {
        // Initialization code, if any
    }

    public void SetPrefab(GameObject prefab)
    {
        GameObject child = Instantiate(prefab, this.transform);
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetData(Vector3Int coords, string roomType, RoomState roomState, GameObject roomPrefab, Map map)
    {
        this.name = "Room " + roomType + ", " + coords.x + ", " + coords.y;
        this.map = map;
        this.mapPosition = coords;
        this.mapCoords = new Vector2Int(coords.x, coords.y);
        this.roomState = roomState;
        this.roomType = (RoomType)System.Enum.Parse(typeof(RoomType), roomType);
        transform.position = map.GetPositionFromCoords(coords);
        SetPrefab(roomPrefab);
    }
}

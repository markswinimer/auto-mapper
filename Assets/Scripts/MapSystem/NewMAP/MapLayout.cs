using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map/Layout")]
public class MapLayout : ScriptableObject
{

    [SerializeField] public Vector2Int MapSize;
    [SerializeField] public Vector3 RoomTileSize;

    [Serializable]
    private class MapRoomSetup
    {
        public int roomAmount;
        public RoomType roomType;
        public GameObject[] roomPrefabs;
    }

    [SerializeField] private MapRoomSetup[] mapRooms;

    public int GetRoomCount()
    {
        return mapRooms.Length;
    }
    
    public string GetRoomTypeAtIndex(int index)
    {
        if (mapRooms.Length <= index)
        {
            Debug.LogError("Room index out of range");
            return "";
        }
        return mapRooms[index].roomType.ToString();
    }

    public int GetRoomAmountAtIndex(int index)
    {
        if (mapRooms.Length <= index)
        {
            Debug.LogError("Room index out of range");
            return -1;
        }
        return mapRooms[index].roomAmount;
    }

    public GameObject[] GetRoomPrefabsAtIndex(int index)
    {
        if (index < 0 || index >= mapRooms.Length)
        {
            Debug.LogError("Room index out of range");
            return null;  // Return null to indicate out of range
        }
        return mapRooms[index].roomPrefabs;
    }

}
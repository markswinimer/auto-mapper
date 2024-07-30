using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour {
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject room;

    // private Dictionary<string, GameObject> nameToRoomDict = new Dictionary<string, GameObject>();

    // private void Awake()
    // {
    //     foreach (var room in roomPrefabs)
    //     {
    //         nameToRoomDict.Add(room.GetComponent<Room>().GetType().ToString(), room);
    //         Debug.Log(room.GetComponent<Room>().GetType().ToString());
    //     }
    // }
    public GameObject CreateRoom(Vector3 position)
    {
        GameObject newRoom = Instantiate(room, position, Quaternion.Euler(0, 90f, 0), transform);

        return newRoom;
        // if (prefab)
        // {
            // GameObject newRoom = Instantiate(prefab);
            // return newRoom;
        // }
        // return null;
    }
}
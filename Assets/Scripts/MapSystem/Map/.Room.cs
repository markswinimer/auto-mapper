using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int x;
    public int y;
    public string type;

    void Start()
    {
        Debug.Log("Room Start");
        if(RoomController.instance == null)
        {
            Debug.Log("RoomController not found");
            return;
        }
        RoomController.instance.RegisterRoom(this);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));    
    }
    
    public Vector3 GetRoomCentre()
    {
        return new Vector3(x * Width, y * Height);
    }
}
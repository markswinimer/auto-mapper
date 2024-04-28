using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeState State { get; set; }
    private Vector3Int _coordinate;
    private bool _isOccupied;

    // Initialization method with coordinate and node type
    public void Init(Vector3Int coordinate, NodeState nodeState)
    {
        _coordinate = coordinate;
        _isOccupied = false; 
        float _one = 0f;
        
        State = nodeState;
        
        //rotate node to fit into grid properly.
        //TODO: look into a more explicit way to handle this
        transform.rotation = Quaternion.Euler(0, 90f, 0);
    }
    private void Update() 
    {
        //if the state is inaccessible, set the prefab to innaccessible
        if (State == NodeState.Inaccessible)
        {
            // GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
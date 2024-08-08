using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObject : MonoBehaviour
{
	public bool Placed { get; private set; }
	public Vector3Int Size { get; private set; }
	private Vector3[] _vertices;
	
	private void Start() 
	{
		GetColliderVertexPositionsLocal();
		CalculateSizeInCells();	
		var drag = GetComponent<ObjectDrag>();
		if(drag == null)
		{
			//preplaced combatant, set placed
			Placed = true;
		}
	}
	
	private void GetColliderVertexPositionsLocal()
	{
		var boxCollider = GetComponent<BoxCollider>();
		_vertices = new Vector3[4];
		_vertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
		_vertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
		_vertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
		_vertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
	}
	
	private void CalculateSizeInCells()
	{
		var vertices = new Vector3Int[_vertices.Length];
		
		for(int i = 0; i < vertices.Length; i++)
		{
			var worldPos = transform.TransformPoint(_vertices[i]);
			vertices[i] = PlacementSystem.Instance.GridLayout.WorldToCell(worldPos);
		}
		
		Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x),
							Math.Abs((vertices[0] - vertices[3]).y), 1);
	}
	
	public Vector3 GetStartPosition()
	{
		return transform.TransformPoint(_vertices[0]);
	}
	
	public virtual void Place()
	{
		var drag = GetComponent<ObjectDrag>();
		Destroy(drag);
		
		var combatant = GetComponent<Combatant>();
		combatant.ReadyForCombat = true;
		
		Placed = true;
	}
}

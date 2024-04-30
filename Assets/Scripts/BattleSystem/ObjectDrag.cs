using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectDrag : MonoBehaviour
{
	private Vector3 _offset;
	
	private void OnMouseDown()
	{
		Debug.Log("There");
		_offset = transform.position - MousePosition3D.GetMouseWorldPosition();
	}
	
	private void OnMouseDrag()
	{
		Debug.Log("Here");
		var pos = MousePosition3D.GetMouseWorldPosition() + _offset;
		Debug.Log("Dragging");
		var newPos = PlacementSystem.SnapCoordinateToGrid(pos);
		transform.position = newPos;
	}
	
	
}

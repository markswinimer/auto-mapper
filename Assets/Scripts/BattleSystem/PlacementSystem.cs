using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
	public static PlacementSystem Instance { get; private set;}
	public GridLayout GridLayout;
	public Grid Grid;
	
	[SerializeField] private Tilemap _mainTileMap;
	[SerializeField] private TileBase _whiteTile;
	public GameObject Unit;

	private PlaceableObject _objectToPlace;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			InitializeWithGameObject(Unit);
		}
		
		if(!_objectToPlace) return;
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(CanBePlaced(_objectToPlace))
			{
				_objectToPlace.Place();
				var start = GridLayout.WorldToCell(_objectToPlace.GetStartPosition());
				TakeArea(start, _objectToPlace.Size);
			}
			else
			{
				Destroy(_objectToPlace.gameObject);
			}
		}
		else if(Input.GetKeyDown(KeyCode.Escape)) Destroy(_objectToPlace.gameObject);
	}
	
	public static Vector3 SnapCoordinateToGrid(Vector3 position) => Instance.SnapCoordinateToGrid_Instance(position);
	public static Vector3 GetGridPositionToMove(Vector3 startPosition, Vector3 targetPosition)
	 => Instance.GetGridPositionToMove_Instance(startPosition, targetPosition);

	private void Awake()
	{
		Instance = this;
		Grid = GridLayout.gameObject.GetComponent<Grid>();
	}
	
	public Vector3 SnapCoordinateToGrid_Instance(Vector3 position)
	{
		var cell = GridLayout.WorldToCell(position);
		position = Grid.GetCellCenterWorld(cell);
		return position;
	}
	
	public Vector3 GetGridPositionToMove_Instance(Vector3 startPosition, Vector3 targetPosition)
	{
		var direction = (targetPosition - startPosition).normalized;
		var cell = GridLayout.WorldToCell(direction);
		var position = Grid.GetCellCenterWorld(cell);
		return position;
	}
	
	public void InitializeWithGameObject(GameObject prefab)
	{
		var pos = SnapCoordinateToGrid_Instance(Vector3.zero);
		
		Debug.Log("Here2");
		var obj = Instantiate(prefab, pos, Quaternion.identity);
		Debug.Log("Here2");
		_objectToPlace = obj.GetComponent<PlaceableObject>();
		Debug.Log("Here2");
		obj.AddComponent<ObjectDrag>();
		Debug.Log("Here2");
	}
	
	private static TileBase[] GetTileBlocks(BoundsInt area, Tilemap tilemap)
	{
		var array = new TileBase[area.size.x * area.size.y * area.size.z];
		var counter = 0;
		
		foreach(var vector in area.allPositionsWithin)
		{
			var pos = new Vector3Int(vector.x, vector.y, 0);
			array[counter] = tilemap.GetTile(pos);
			counter++;
		}
		return array;
	}
	
	private bool CanBePlaced(PlaceableObject placeableObject)
	{
		var area = new BoundsInt();
		area.position = GridLayout.WorldToCell(_objectToPlace.GetStartPosition());
		area.size = placeableObject.Size;
		
		var baseArray = GetTileBlocks(area, _mainTileMap);
		
		foreach(var tileBase in baseArray)
		{
			if(tileBase == _whiteTile) return false;
		}
		return true;
	}
	
	public void TakeArea(Vector3Int start, Vector3Int size)
	{
		_mainTileMap.BoxFill(start, _whiteTile, start.x, start.y,
		start.x + size.x, start.y + size.y);
	}
}

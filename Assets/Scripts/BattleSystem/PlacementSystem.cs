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
	private float _tileSize;
	private static bool _isLocked; 
	// Start is called before the first frame update
	void Start()
	{
		_tileSize = Grid.cellSize.x * Grid.cellSize.y / 2;
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
			if(CanClaimTargetArea_Instance(_objectToPlace.GetStartPosition(), _objectToPlace.Size))
			{
				_objectToPlace.Place();
				var start = GridLayout.WorldToCell(_objectToPlace.GetStartPosition());
				TakeArea_Instance(start, _objectToPlace.Size);
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
	 
	public static void ReleaseArea(Vector3 targetPosition, Vector3Int size)
	 => Instance.ReleaseArea_Instance(targetPosition, size);

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
		var direction = (targetPosition - startPosition).normalized * _tileSize;
		Debug.Log("direction="+direction.ToString());
		var targetTile = startPosition + direction;
		var cell = GridLayout.WorldToCell(targetTile);
		var position = Grid.GetCellCenterWorld(cell);
		return position;
	}
	
	public void InitializeWithGameObject(GameObject prefab)
	{
		var pos = SnapCoordinateToGrid_Instance(Vector3.zero);
		
		var obj = Instantiate(prefab, pos, Quaternion.identity);
		_objectToPlace = obj.GetComponent<PlaceableObject>();
		obj.AddComponent<ObjectDrag>();
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
	
	private bool CanClaimTargetArea_Instance(Vector3 targetPosition, Vector3Int size)
	{
		var area = new BoundsInt();
		area.position = GridLayout.WorldToCell(targetPosition);
		area.size = size;
		
		var baseArray = GetTileBlocks(area, _mainTileMap);
		
		foreach(var tileBase in baseArray)
		{
			if(tileBase == _whiteTile) return false;
		}
		return true;
	}
	
	public void TakeArea_Instance(Vector3Int start, Vector3Int size)
	{
		_mainTileMap.BoxFill(start, _whiteTile, start.x, start.y,
		start.x + size.x, start.y + size.y);
	}
	
	public void ReleaseArea_Instance(Vector3 targetPosition, Vector3Int size)
	{
		var start = GridLayout.WorldToCell(targetPosition);
		_mainTileMap.BoxFill(start, null, start.x, start.y,
		start.x + size.x, start.y + size.y);
	}
}

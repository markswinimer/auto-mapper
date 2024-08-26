using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{
	private PlayerInventory _playerInventory;
	private PlayerRobot _playerRobot;
	private void Start() {
		_playerInventory = FindFirstObjectByType<PlayerInventory>();
		_playerRobot = FindFirstObjectByType<PlayerRobot>();
	}
	
	private void Update() {
		if(Input.GetKeyDown(KeyCode.P))
		{
			var data = GetUIData();
			var printData = "Frames = " + string.Join(',', data.Frames.Select(f => f.Name));
			printData += $"; Heads = " + string.Join(',', data.Heads.Select(f => f.Name));
			printData += $"; Gambits = " + string.Join(',', data.Gambits.Select(f => f.Name));
			printData += $"; Robots = " + string.Join(',', data.Robots.Select(f => f.Name));
			Debug.Log(printData);	
		}
	}
	
	public UIData GetUIData()
	{
		return new()
		{
			Frames = _playerInventory.Frames,
			Heads = _playerInventory.Heads,
			Gambits = _playerInventory.Gambits,
			Robots = _playerRobot.Robots
		};
		//Robot.Frame.GambitSlots = currently equipped gambit / slot data
		//Robot contains Stats except for weapon type, which is under Frame
	}
}
	//just putting this here for now
public class UIData
{
	public List<Frame> Frames;
	public List<Head> Heads;
	public List<Gambit> Gambits;
	public List<Combatant> Robots;
}
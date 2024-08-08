using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePhaseController : MonoBehaviour
{
	
	public BattleState CurrentBattleState;
	private BattleController _battleController;
	// Start is called before the first frame update
	void Start()
	{
		CurrentBattleState = BattleState.Placing;
		_battleController = FindFirstObjectByType<BattleController>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F) && CurrentBattleState == BattleState.Placing)
		{
			var combatants = FindObjectsOfType<Combatant>().ToList();
			if(combatants.FirstOrDefault(c => c.CombatTeam == Team.Player) == null)
			{
				//Probably show some error saying "cant start fight without units placed
				return;
			}
			if(combatants.All(c => c.ReadyForCombat))
			{
				StartFight(combatants);
			}
		}
		else if(CurrentBattleState == BattleState.Fighting && _battleController.IsBattleOver())
		{
			EndFight();
		}
	}
	
	private void StartFight(List<Combatant> combatants)
	{
		//get all combatants, set them to begin fighting
		_battleController.InitializeTeams(combatants);
		combatants.ForEach(c => c.StartFighting());
		
		CurrentBattleState = BattleState.Fighting;
	}
	
	private void EndFight()
	{
		//decide victor, handle win/loss
		var combatants = FindObjectsByType<Combatant>(FindObjectsSortMode.None).ToList();
		combatants.ForEach(c => c.StopFighting());
		
		CurrentBattleState = BattleState.Placing;
	}
}

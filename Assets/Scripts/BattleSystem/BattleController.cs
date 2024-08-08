using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
	private Dictionary<Team, List<Combatant>> _teams;
	// Start is called before the first frame update
	void Start()
	{
		_teams = new Dictionary<Team, List<Combatant>>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void InitializeTeams(List<Combatant> combatants)
	{
		foreach (var combatant in combatants)
		{
			var team = combatant.CombatTeam;
			if(_teams.TryGetValue(team, out var targetTeam))
			{
				targetTeam.Add(combatant);	
			}
			else
			{
				var newTeam = new List<Combatant>
				{
					combatant
				};
				_teams.Add(team, newTeam);
			}
		}
	}
	
	public Combatant FindTarget(Combatant combatant)
	{
		var team = combatant.CombatTeam;
		
		var enemies = new List<Combatant>();
		foreach(var combatTeam in _teams)
		{
			if(combatTeam.Key == team) continue;
			enemies.AddRange(combatTeam.Value);
		}
		
		Combatant closest = null;
		var maxDistance = Mathf.Infinity;

		foreach(var enemy in enemies)
		{
			var targetDistance = Vector3.Distance(combatant.transform.position, enemy.transform.position);

			if (targetDistance < maxDistance)
			{
				closest = enemy;
				maxDistance = targetDistance;
			}
		}

		return closest;
	}
	
	public bool IsBattleOver()
	{
		var teamsRemaining = 0;
		foreach(var team in _teams)
		{
			if(team.Value.Count > 0) teamsRemaining++;
			if(teamsRemaining > 1) return false;
		}
		return true;
	}
	
	public void HandleUnitDeath(Combatant combatant)
	{
		_teams[combatant.CombatTeam].Remove(combatant);
	}
}

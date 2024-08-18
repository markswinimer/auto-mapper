using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
	private Dictionary<Team, List<Combatant>> _teams;
	private Dictionary<TargetCriteria, Func<Combatant, List<Combatant>, Combatant>> _targetingMethod;
	public float GambitDelay = 2f;
	
	// Start is called before the first frame update
	void Start()
	{
		_teams = new Dictionary<Team, List<Combatant>>();
		_targetingMethod = new Dictionary<TargetCriteria, Func<Combatant, List<Combatant>, Combatant>>
		{
			{ TargetCriteria.Closest, GetClosestUnit },
			{ TargetCriteria.HighestMaxHp, GetHighestMaxHpUnit },
			{ TargetCriteria.HighestHp, GetHighestHpUnit },
			{ TargetCriteria.LowestMaxHp, GetLowestMaxHpUnit },
			{ TargetCriteria.LowestHp, GetLowestHpUnit }
		};
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
	
	public Combatant FindTarget(Combatant combatant, TargetCriteria criteria = TargetCriteria.None)
	{
		if(criteria == TargetCriteria.None) criteria = combatant.TargetCriteria;
		if(criteria == TargetCriteria.Self) return combatant;
		
		var team = combatant.CombatTeam;
		
		var enemies = new List<Combatant>();
		foreach(var combatTeam in _teams)
		{
			if(combatTeam.Key == team) continue;
			enemies.AddRange(combatTeam.Value);
		}
		
		return _targetingMethod[criteria](combatant, enemies);
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
	
	private Combatant GetClosestUnit(Combatant combatant, List<Combatant> enemies)
	{
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
	
	private Combatant GetHighestMaxHpUnit(Combatant combatant, List<Combatant> enemies)
	{
		Combatant highest = enemies[0];
		
		foreach(var enemy in enemies)
		{
			if (enemy.MaxHp > highest?.MaxHp)
			{
				highest = enemy;
			}
		}

		return highest;
	}
	
	private Combatant GetHighestHpUnit(Combatant combatant, List<Combatant> enemies)
	{
		Combatant highest = enemies[0];
		
		foreach(var enemy in enemies)
		{
			if (enemy.CurrentHp > highest?.CurrentHp)
			{
				highest = enemy;
			}
		}

		return highest;
	}
	
	private Combatant GetLowestMaxHpUnit(Combatant combatant, List<Combatant> enemies)
	{
		Combatant lowest = enemies[0];
		
		foreach(var enemy in enemies)
		{
			if (enemy.MaxHp < lowest?.MaxHp)
			{
				lowest = enemy;
			}
		}

		return lowest;
	}
	
	private Combatant GetLowestHpUnit(Combatant combatant, List<Combatant> enemies)
	{
		Combatant lowest = enemies[0];
		
		foreach(var enemy in enemies)
		{
			if (enemy.CurrentHp < lowest?.CurrentHp)
			{
				lowest = enemy;
			}
		}

		return lowest;
	}
}

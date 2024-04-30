using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
	private int _hp = 10;
	private int _attack = 2;
	private float _attackSpeed = 1;
	private float _moveSpeed = 1;
	private float _range = 3;
	
	public Team CombatTeam;
	public bool IsFighting;
	public bool ReadyForCombat;
	
	private Combatant _target;
	private BattleController _battleController;
	// Start is called before the first frame update
	void Start()
	{
		_battleController = FindFirstObjectByType<BattleController>();
		if(CombatTeam != Team.Player) ReadyForCombat = true;
	}

	// Update is called once per frame
	void Update()
	{
		if(_hp <= 0) Die();
	}
	
	public void StartFighting()
	{
		IsFighting = true;
		StartCoroutine(BattleRoutine());
		
		//find enemy, move within range, attack til dead
	}
	
	public void StopFighting()
	{
		IsFighting = false;
	}
	
	private void Die()
	{
		//death animation, stop being targetable, etc
		//for now, just die
		
		Destroy(gameObject);
	}
	
	private IEnumerator BattleRoutine()
	{
		while(IsFighting)
		{
			_target = _battleController.FindTarget(this);
			if(_target != null)
			{
				if(IsTargetInRange())
				{
					_target.DealDamage(_attack);
					yield return new WaitForSeconds(_attackSpeed);
				}
				else
				{
					yield return MoveToTarget();
				}
			}
		}
	}
	
	private void DealDamage(int attack)
	{
		_hp -= attack;
	}
	
	private bool IsTargetInRange()
	{
		var distance = Vector3.Distance(transform.position, _target.transform.position);
		return distance <= _range;
	}
	
	private IEnumerator MoveToTarget()
	{
		//TODO: Claim tile/check for claimed tile so people can't overlap
		var endPosition = PlacementSystem.GetGridPositionToMove(transform.position, _target.transform.position);
		var startPosition = transform.position;
		var travelPercent = 0f;

		while (travelPercent < 1f && _target != null)
		{
			travelPercent += Time.deltaTime * _moveSpeed;
			transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
			yield return new WaitForEndOfFrame();
		}
	}
}

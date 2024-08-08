using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Combatant : MonoBehaviour
{
	[SerializeField] private int _hp = 10;
	[SerializeField] private int _attack = 2;
	[SerializeField] private float _attackSpeed = 1;
	[SerializeField] private float _moveSpeed = 2f;
	[SerializeField] private float _range = 3;
	
	private const float _timeMultiplierForMovement = 3;
	
	public Team CombatTeam;
	public bool IsFighting;
	public bool ReadyForCombat;
	
	private Combatant _target;
	private BattleController _battleController;
	public PlaceableObject PlaceableObject;
	public Vector3 TargetPosition;
	private NavMeshAgent _navMeshAgent;
	// Start is called before the first frame update
	void Start()
	{
		_battleController = FindFirstObjectByType<BattleController>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.speed = _moveSpeed;
		PlaceableObject = GetComponent<PlaceableObject>();
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
		PlacementSystem.ReleaseArea(transform.position, PlaceableObject.Size);
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
		_battleController.HandleUnitDeath(this);
		Destroy(gameObject);
	}
	
	private IEnumerator BattleRoutine()
	{
		while(IsFighting)
		{
			_target = _battleController.FindTarget(this);
			if(_target != null)
			{
				Debug.Log("Check if in range/move");
				if(IsTargetInRange())
				{
					_navMeshAgent.isStopped = true;
					_target.DealDamage(_attack);
					yield return new WaitForSeconds(_attackSpeed);
				}
				else
				{
					_navMeshAgent.isStopped = false;
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
		////TODO: Claim tile/check for claimed tile so people can't overlap
		//var direction = (_target.transform.position - transform.position).normalized * Time.deltaTime * (_moveSpeed + _timeMultiplierForMovement);
		//var startPosition = transform.position;
		//var endPosition = startPosition + direction;
		//transform.position = Vector3.Lerp(startPosition, endPosition, 1);
		//yield return new WaitForEndOfFrame();
		
		_navMeshAgent.SetDestination(_target.transform.position);
		yield return new WaitForEndOfFrame();
	}
}

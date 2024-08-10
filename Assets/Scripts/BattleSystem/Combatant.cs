using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Combatant : MonoBehaviour
{
	[SerializeField] public int CurrentHp = 10;
	[SerializeField] public int MaxHp = 10;
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
	public TargetCriteria TargetCriteria; 
	public List<Gambit> Gambits;
	private GambitHelper _gambitHelper;
	// Start is called before the first frame update
	void Start()
	{
		_battleController = FindFirstObjectByType<BattleController>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.speed = _moveSpeed;
		PlaceableObject = GetComponent<PlaceableObject>();
		if(CombatTeam != Team.Player) ReadyForCombat = true;
		Gambits = new List<Gambit>();
		_gambitHelper = FindFirstObjectByType<GambitHelper>();
		if(CombatTeam == Team.Player)
		{
			Gambits.Add(Instantiate(_gambitHelper.FirePrefab).GetComponent<Gambit>());
			Gambits.Add(Instantiate(_gambitHelper.FreezePrefab).GetComponent<Gambit>());
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(CurrentHp <= 0) Die();
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
			var readyGambits = Gambits.Where(g => g.IsReady);
			Gambit gambitToActivate = null;
			Debug.Log("Gams = "+ Gambits.Count().ToString());
			Debug.Log("readyGams = "+ readyGambits.Count().ToString());
			foreach(var gambit in readyGambits)
			{
				_target = _battleController.FindTarget(this, gambit.TargetCriteria);
				Debug.Log("target=" + _target.ToString());
				if(IsTargetInRange(gambit.Range))
				{
					gambitToActivate = gambit;
					break;
				}
			}
			
			if(gambitToActivate != null)
			{
				Debug.Log("Activate");
				_navMeshAgent.isStopped = true;
				GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
				yield return new WaitForSeconds(gambitToActivate.ActivationTime);
				gambitToActivate.ActivateBase(this, _target);
				GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
			}
			else
			{
				_target = _battleController.FindTarget(this);
				if(_target != null)
				{
					Debug.Log("Check if in range/move");
					if(IsTargetInRange(_range))
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
	}
	
	public void DealDamage(int attack)
	{
		CurrentHp -= attack;
	}
	
	private bool IsTargetInRange(float range)
	{
		var distance = Vector3.Distance(transform.position, _target.transform.position);
		return distance <= range;
	}
	
	private IEnumerator MoveToTarget()
	{
		_navMeshAgent.SetDestination(_target.transform.position);
		yield return new WaitForEndOfFrame();
	}
}

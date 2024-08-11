using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Combatant : MonoBehaviour
{
	public int CurrentHp { get; set; }
	public int MaxHp { get; set; }
	
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
	public Frame Frame;
	public Head Head;
	// Start is called before the first frame update
	void Start()
	{
		_battleController = FindFirstObjectByType<BattleController>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		PlaceableObject = GetComponent<PlaceableObject>();
		if(CombatTeam != Team.Player) ReadyForCombat = true;
		Gambits = new List<Gambit>();
		if(CombatTeam == Team.Player)
		{
			Frame = FindFirstObjectByType<FrameHelper>().SharpShotPrefab.GetComponent<Frame>();
			Head = FindFirstObjectByType<FrameHelper>().SharpHeadPrefab.GetComponent<Head>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(IsFighting)
		{
			if(CurrentHp <= 0) Die();
		}
		
	}
	
	public void StartFighting()
	{
		IsFighting = true;
		PlacementSystem.ReleaseArea(transform.position, PlaceableObject.Size);
		StartCoroutine(BattleRoutine());
		
		//find enemy, move within range, attack til dead
	}
	
	public void SetupFrame()
	{
		GetComponentInChildren<MeshRenderer>().material.color = Frame.Color;
		GetComponentInChildren<HeadHelper>().GetComponent<MeshRenderer>().material.color = Head.Color;
		_navMeshAgent.speed = Frame.MoveSpeed;
		MaxHp = Frame.BaseHp + Head.BaseHp;
		CurrentHp = MaxHp;
		Gambits = Frame.Gambits;
		Gambits.AddRange(Head.Gambits);
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
			var readyGambits = Gambits.Where(g => g.IsReady(this));
			Gambit gambitToActivate = null;
			Debug.Log("Gams = "+ Gambits.Count().ToString());
			Debug.Log("readyGams = "+ readyGambits.Count().ToString());
			foreach(var gambit in readyGambits)
			{
				var gambitTargetCriteria = gambit.TargetCriteria == TargetCriteria.None ? TargetCriteria : gambit.TargetCriteria;
				_target = _battleController.FindTarget(this, gambitTargetCriteria);
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
				GetComponentInChildren<MeshRenderer>().material.color = Frame.Color;
			}
			else
			{
				_target = _battleController.FindTarget(this);
				if(_target != null)
				{
					Debug.Log("Check if in range/move");
					if(IsTargetInRange(Frame.Range))
					{
						_navMeshAgent.isStopped = true;
						_target.DealDamage(Frame.Damage);
						yield return new WaitForSeconds(Frame.AttackDelay);
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
		if(Frame.WeaponTypes.Contains(WeaponType.Block))
		{
			if(UnityEngine.Random.value <= Frame.BlockRate) attack = (int)Math.Floor(attack * Frame.BlockDamageReductionPercentage);
		}
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

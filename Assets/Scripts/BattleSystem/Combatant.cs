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
	public List<GambitSlot> Gambits;
	public Frame Frame;
	public Head Head;
	// Start is called before the first frame update
	void Start()
	{
		_battleController = FindFirstObjectByType<BattleController>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		PlaceableObject = GetComponent<PlaceableObject>();
		if(CombatTeam != Team.Player) ReadyForCombat = true;
		Gambits = new List<GambitSlot>();
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
	}
	
	public void SetupFrame()
	{
		GetComponentInChildren<MeshRenderer>().material.color = Frame.Color;
		GetComponentInChildren<HeadHelper>().GetComponent<MeshRenderer>().material.color = Head.Color;
		_navMeshAgent.speed = Frame.MoveSpeed;
		MaxHp = Frame.BaseHp + Head.BaseHp;
		CurrentHp = MaxHp;
		
		//TODO: Eventually UI / real requirements, for now just hardcode it
		Gambits = Frame.Gambits.Where(g => CanUseGambit(g)).Select(g => new GambitSlot(g, GambitModifier.None)).ToList();
		Gambits.AddRange(Head.Gambits.Where(g => CanUseGambit(g)).Select(g => new GambitSlot(g, GambitModifier.None)).ToList());
		Gambits = Gambits.Take(Frame.GambitSlotCount).ToList();
		Gambits.ForEach(g => g.ModfiyGambit());
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
			foreach(var gambitSlot in Gambits)
			{
				var gambit = gambitSlot.Gambit;
				var gambitTargetCriteria = gambit.TargetCriteria == TargetCriteria.None ? TargetCriteria : gambit.TargetCriteria;
				_target = _battleController.FindTarget(this, gambitTargetCriteria);
				Debug.Log("target=" + _target.ToString());
				if(IsTargetInRange(gambit.Range))
				{
					Debug.Log("Activate");
					_navMeshAgent.isStopped = true;
					GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
					yield return new WaitForSeconds(_battleController.GambitDelay);
					gambit.ActivateBase(this, _target);
					GetComponentInChildren<MeshRenderer>().material.color = Frame.Color;
				}
				else
				{
					//dont activate, but set wait period anyways to keep in sync, and then move on, maybe just move?
					_navMeshAgent.isStopped = false;
					yield return MoveToTarget(_battleController.GambitDelay);
				}
			}
			if(Gambits.Count <= 0)
			{
				Debug.Log("No gambits, yielding");
				yield return new WaitForEndOfFrame();
			}
			
			
			
			
			
			
			
			
			
			
			//var readyGambits = Gambits.Where(g => g.IsReady(this));
			//Gambit gambitToActivate = null;
			//Debug.Log("Gams = "+ Gambits.Count().ToString());
			//Debug.Log("readyGams = "+ readyGambits.Count().ToString());
			//foreach(var gambit in readyGambits)
			//{
			//	var gambitTargetCriteria = gambit.TargetCriteria == TargetCriteria.None ? TargetCriteria : gambit.TargetCriteria;
			//	_target = _battleController.FindTarget(this, gambitTargetCriteria);
			//	Debug.Log("target=" + _target.ToString());
			//	if(IsTargetInRange(gambit.Range))
			//	{
			//		gambitToActivate = gambit;
			//		break;
			//	}
			//}
			//
			//if(gambitToActivate != null)
			//{
			//	Debug.Log("Activate");
			//	_navMeshAgent.isStopped = true;
			//	GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
			//	yield return new WaitForSeconds(gambitToActivate.ActivationTime);
			//	gambitToActivate.ActivateBase(this, _target);
			//	GetComponentInChildren<MeshRenderer>().material.color = Frame.Color;
			//}
			//else
			//{
			//	_target = _battleController.FindTarget(this);
			//	if(_target != null)
			//	{
			//		Debug.Log("Check if in range/move");
			//		if(IsTargetInRange(Frame.Range))
			//		{
			//			_navMeshAgent.isStopped = true;
			//			_target.DealDamage(Frame.Damage);
			//			yield return new WaitForSeconds(Frame.AttackDelay);
			//		}
			//		else
			//		{
			//			_navMeshAgent.isStopped = false;
			//			yield return MoveToTarget();
			//		}
			//	}
			//}
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
	
	private IEnumerator MoveToTarget(float durationToMove)
	{
		var currentDuration = 0f;
		while(currentDuration < durationToMove)
		{
			_navMeshAgent.SetDestination(_target.transform.position);
			currentDuration += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
	
	private bool CanUseGambit(Gambit gambit)
	{
		foreach(var weaponType in gambit.RequiredWeaponTypes)
		{
			if(!Frame.WeaponTypes.Contains(weaponType)) return false;
		}
		return true;
	}
}

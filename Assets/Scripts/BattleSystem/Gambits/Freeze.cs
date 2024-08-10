using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Gambit
{
	private int _damage;
	
	// Start is called before the first frame update
	void Start()
	{
		Cooldown = 10f;
		_damage = 2;
		ActivationTime = 1f;
		TargetCriteria = TargetCriteria.HighestMaxHp;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.DealDamage(_damage);
		target.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
	}
}

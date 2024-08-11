using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : Gambit
{
	private int _damage;
	
	// Start is called before the first frame update
	void Start()
	{
		Cooldown = 5f;
		_damage = 3;
		ActivationTime = 2f;
		TargetCriteria = TargetCriteria.None;
		Range = 15f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.DealDamage(_damage);
		target.DealDamage(_damage);
		target.DealDamage(_damage);
		target.DealDamage(_damage);
		target.DealDamage(_damage);
		target.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
	}
}

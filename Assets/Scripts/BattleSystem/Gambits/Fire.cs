using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gambit
{
	private int _damage;
	
	// Start is called before the first frame update
	void Start()
	{
		Cooldown = 5f;
		_damage = 5;
		ActivationTime = 2f;
		TargetCriteria = TargetCriteria.LowestMaxHp;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.DealDamage(_damage);
		target.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
	}
}

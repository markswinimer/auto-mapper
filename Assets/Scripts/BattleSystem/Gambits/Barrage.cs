using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : Gambit
{
	// Start is called before the first frame update
	void Start()
	{
		TargetCriteria = TargetCriteria.None;
		Range = 15f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.DealDamage((int)Power);
		target.DealDamage((int)Power);
		target.DealDamage((int)Power);
		target.DealDamage((int)Power);
		target.DealDamage((int)Power);
	}
}

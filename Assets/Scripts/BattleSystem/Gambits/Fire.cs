using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gambit
{
	// Start is called before the first frame update
	void Start()
	{
		Power = 5;
		TargetCriteria = TargetCriteria.LowestMaxHp;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.DealDamage((int)Power);
	}
}

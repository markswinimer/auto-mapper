using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Gambit
{
	// Start is called before the first frame update
	void Start()
	{
		Power = 2;
		TargetCriteria = TargetCriteria.HighestMaxHp;
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

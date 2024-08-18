using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : Gambit
{
	// Start is called before the first frame update
	void Start()
	{
		Power = 5;
		TargetCriteria = TargetCriteria.Self;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.CurrentHp += (int)Power;
	}
}

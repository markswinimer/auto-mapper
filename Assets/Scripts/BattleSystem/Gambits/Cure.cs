using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : Gambit
{
	private int _damage;
	
	// Start is called before the first frame update
	void Start()
	{
		Cooldown = 2f;
		_damage = 5;
		ActivationTime = 0.5f;
		TargetCriteria = TargetCriteria.Self;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public override void Activate(Combatant self, Combatant target)
	{
		target.CurrentHp += _damage;
	}

    public override bool IsReady(Combatant combatant)
    {
        return !OnCooldown && combatant.CurrentHp <= 8;
    }
}

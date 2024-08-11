using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValorEdge : Frame
{
	// Start is called before the first frame update
	void Start()
	{
		WeaponTypes = new List<WeaponType>
		{
			WeaponType.Melee,
			WeaponType.Block
		};
		BlockRate = 25f;
		Damage = 2;
		BaseHp = 50;
		AttackDelay = 1f;
		Color = Color.white;
		Range = 3f;
		MoveSpeed = 1f;
		BlockDamageReductionPercentage = 0.5f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}

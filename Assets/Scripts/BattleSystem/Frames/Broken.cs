using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : Frame
{
	// Start is called before the first frame update
	void Start()
	{
		WeaponTypes = new List<WeaponType>
		{
			WeaponType.Melee
		};
		Damage = 2;
		BaseHp = 15;
		AttackDelay = 2f;
		Color = Color.black;
		Range = 3f;
		MoveSpeed = 0.5f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}

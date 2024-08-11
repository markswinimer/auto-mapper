using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpShot : Frame
{
	// Start is called before the first frame update
	void Start()
	{
		WeaponTypes = new List<WeaponType>
		{
			WeaponType.Melee,
			WeaponType.Range
		};
		Damage = 3;
		BaseHp = 25;
		AttackDelay = 0.5f;
		Color = Color.cyan;
		Range = 15f;
		MoveSpeed = 2f;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}

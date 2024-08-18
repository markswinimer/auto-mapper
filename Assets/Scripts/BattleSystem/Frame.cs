using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Frame : MonoBehaviour
{
	public List<WeaponType> WeaponTypes;
	public Color Color;
	public List<Gambit> Gambits;
	public int Damage;
	public float AttackDelay;
	public int BaseHp;
	public float BlockRate;
	public float BlockDamageReductionPercentage;
	public float MoveSpeed;
	public float Range;
	public int GambitSlotCount = 4;
	
	// Start is called before the first frame update
	void Start()
	{
		Color = Color.gray;
		Gambits = new List<Gambit>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}

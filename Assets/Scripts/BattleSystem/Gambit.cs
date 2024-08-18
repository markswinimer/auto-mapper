using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gambit : MonoBehaviour
{
	public TargetCriteria TargetCriteria;
	public float Range = 20f;
	public float Power;
	public List<WeaponType> RequiredWeaponTypes;
	
	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("GambitStart");
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void ActivateBase(Combatant self, Combatant target)
	{
		Activate(self, target);
	}
	
	public abstract void Activate(Combatant self, Combatant target);
}

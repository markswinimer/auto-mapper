using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gambit : MonoBehaviour
{
	public TargetCriteria TargetCriteria;
	public float Cooldown;
	public bool IsReady;
	public float Range = 20f;
	public float ActivationTime = 1f;
	
	// Start is called before the first frame update
	void Start()
	{
		Cooldown = 5f;
		IsReady = true;
		Debug.Log("GambitStart");
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void ActivateBase(Combatant self, Combatant target)
	{
		IsReady = false;
		Activate(self, target);
		StartCoroutine(StartCooldown());
	}
	
	private IEnumerator StartCooldown()
	{
		yield return new WaitForSeconds(Cooldown);
		IsReady = true;
	}
	
	public abstract void Activate(Combatant self, Combatant target);
}

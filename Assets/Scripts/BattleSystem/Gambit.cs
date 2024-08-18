using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gambit : MonoBehaviour
{
	public TargetCriteria TargetCriteria;
	public float Range = 20f;
	public float Power;
	public List<WeaponType> RequiredWeaponTypes;
	public bool HasParticleEffects;
	public float ParticleSystemDuration;
	private GambitParticleEffect _gambitParticleEffect;
	
	// Start is called before the first frame update
	void Awake()
	{
		Debug.Log("GambitStart");
		if(HasParticleEffects)
		{
			_gambitParticleEffect = GetComponentInChildren<GambitParticleEffect>();
			Debug.Log($"Has particles = "+ (_gambitParticleEffect != null).ToString());
		}
	}
	

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void ActivateBase(Combatant self, Combatant target)
	{
		Activate(self, target);
	}
	
	public void HandleParticles(Transform transform)
	{
		if(HasParticleEffects) 
		{
			Debug.Log("play particles");
			_gambitParticleEffect = GetComponentInChildren<GambitParticleEffect>();
			StartCoroutine(_gambitParticleEffect.PlayEffect(ParticleSystemDuration, transform));
		}
	}
	
	public abstract void Activate(Combatant self, Combatant target);
}

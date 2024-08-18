using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambitParticleEffect : MonoBehaviour
{
	private ParticleSystem _particleSystem;
	// Start is called before the first frame update
	void Awake()
	{
		_particleSystem = GetComponentInChildren<ParticleSystem>();
	}
	
	public IEnumerator PlayEffect(float duration, Transform targetTransform)
	{
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		Debug.Log($"Has particle system set = "+ (_particleSystem != null).ToString());
		transform.LookAt(targetTransform.position);
		Debug.Log(targetTransform.position.x.ToString());
		_particleSystem.Play(true);
		yield return new WaitForSeconds(duration);
		_particleSystem.Stop();
	}
}

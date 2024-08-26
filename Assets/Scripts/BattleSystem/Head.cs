using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Head : MonoBehaviour
{
	public Color Color;
	public List<Gambit> Gambits;
	public int BaseHp;
	public string Name;
	
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

using UnityEngine;

public class GambitSlot
{
	public Gambit Gambit;
	public TargetCriteria TargetCriteria;
	public GambitModifier GambitModifier;
	public float ModifierValue;
	
	public GambitSlot(Gambit gambit, GambitModifier modifier, float modifierValue = 0, TargetCriteria targetCriteria = TargetCriteria.None)
	{
		Gambit = gambit;
		GambitModifier = modifier;
		ModifierValue = modifierValue;
		TargetCriteria = targetCriteria;
	}
	
	public void ModfiyGambit()
	{
		switch(GambitModifier)
		{
			case GambitModifier.AoE:
				//make aoe
				return;
			case GambitModifier.Duration:
				//increase duration
				return;
			case GambitModifier.TargetOverride:
				Gambit.TargetCriteria = TargetCriteria;
				return;
			case GambitModifier.Range:
				Gambit.Range = ModifierValue;
				return;
			case GambitModifier.Power:
				Gambit.Power = ModifierValue;
				return;
			case GambitModifier.None:
			default: 
				return;
		}
	}
}

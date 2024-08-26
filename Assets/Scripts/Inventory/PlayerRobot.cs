using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRobot : MonoBehaviour
{
	public List<Combatant> Robots;
	private void Start() {
		Robots = FindObjectsByType<Combatant>(FindObjectsSortMode.None).Where(c => c.CombatTeam == Team.Player).ToList();
		Robots.ForEach(r => r.SetupFrame());
	}
}
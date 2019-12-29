using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicBridge : MagicEffectBase
{
	public override bool DoMoon(MagicOrb orb) {
		return false;		// Do Nothing
	}

	public override bool DoSun(MagicOrb orb) {
		gameObject.GetComponent<Animator>()?.Play("Sun");	// déclencher l'animation
		return true;
	}

	/// <summary>
	///  activer les NavMesh Links pour autoriser le franchissement fu pont
	///  (événement déclenché en fin d'animation)
	/// </summary>
	public void EnableLinks() {
		var links = GetComponentsInChildren<NavMeshLink>();
		foreach (NavMeshLink link in links)
			link.enabled = true;
	}
}

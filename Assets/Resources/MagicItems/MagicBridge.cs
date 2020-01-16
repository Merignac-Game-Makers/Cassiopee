using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicBridge : MagicEffectBase
{
	bool done = false;

	public override bool IsInteractable() {
		if (!oneShot)
			return base.IsInteractable();
		return base.IsInteractable() && !done;
	}
	public override bool DoMoon(MagicOrb orb) {
		return false;		// Do Nothing
	}

	public override bool DoSun(MagicOrb orb) {
		gameObject.GetComponent<Animator>()?.Play("Sun");   // déclencher l'animation
		done = true;
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

	public override void Highlight(bool on) {
		base.Highlight(on);
	}
}

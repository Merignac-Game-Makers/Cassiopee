using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicBridge : MagicEffectBase
{
	public override void DoMoon(MagicOrb orb) {
		// Do Nothing
	}

	public override void DoSun(MagicOrb orb) {
		gameObject.GetComponent<Animator>()?.Play("Sun");
	}

	public void EnableLinks() {
		var links = GetComponentsInChildren<NavMeshLink>();
		foreach (NavMeshLink link in links)
			link.enabled = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicWater : MagicEffectBase
{
	public QuestBase quest;

	public override void DoMoon(MagicOrb orb) {
		var ps = gameObject.GetComponent<ParticleSystem>();
		ps.Play();
		quest.QuestDone();
	}

	public override void DoSun(MagicOrb orb) {
		// Do Nothing
	}

}

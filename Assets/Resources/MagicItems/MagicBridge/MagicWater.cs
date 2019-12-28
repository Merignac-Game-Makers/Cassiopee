using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicWater : MagicEffectBase
{
	public QuestBase quest;     // accomplir cette magie est une quête : indiquer laquelle

	public override void DoMoon(MagicOrb orb) {
		Destroy(GetComponentInChildren<Loot>()?.gameObject);    // détruire le cube
		var ps = gameObject.GetComponent<ParticleSystem>();     // activer l'animation
		ps.Play();
		quest.QuestDone();                                      // renseigner la quête => 'terminée'

	}

	public override void DoSun(MagicOrb orb) {
		// Do Nothing
	}

}

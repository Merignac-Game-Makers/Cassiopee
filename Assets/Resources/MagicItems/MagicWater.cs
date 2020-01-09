using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicWater : MagicEffectBase
{
	public QuestBase quest;     // accomplir cette magie est une quête : indiquer laquelle
	public PNJ Kub;



	public override bool DoMoon(MagicOrb orb) {
		var loot = gameObject.GetComponentInParent<Activable>()?.gameObject.GetComponentInChildren<Loot>()?.gameObject;
		Destroy(loot);											// détruire le cube
		var ps = gameObject.GetComponent<ParticleSystem>();     // activer l'animation
		ps.Play();
		quest.QuestDone();										// renseigner la quête => 'terminée'
		return true;
	}

	public override bool DoSun(MagicOrb orb) {
		return false;	// Do Nothing
	}


	public void Reverse() {
	}
}

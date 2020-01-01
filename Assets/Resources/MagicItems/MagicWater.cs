using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicWater : MagicEffectBase
{
	public QuestBase quest;     // accomplir cette magie est une quête : indiquer laquelle
	public PNJ Kub;
	DefaultDialogDispatcher kubDialogueValidation;



	public override bool DoMoon(MagicOrb orb) {
		var loot = gameObject.GetComponentInParent<Activable>()?.gameObject.GetComponentInChildren<Loot>()?.gameObject;
		Destroy(loot);											// détruire le cube
		var ps = gameObject.GetComponent<ParticleSystem>();     // activer l'animation
		ps.Play();
		kubDialogueValidation = Kub.gameObject.GetComponentInChildren<DefaultDialogDispatcher>();
		kubDialogueValidation.GetCurrent(quest).PassedQuestNode = 17;	// modifier le point d'entrée du dialogue de Kub
		quest.QuestDone();												// renseigner la quête => 'terminée'
		return true;
	}

	public override bool DoSun(MagicOrb orb) {
		return false;	// Do Nothing
	}


	public void Reverse() {
	}
}

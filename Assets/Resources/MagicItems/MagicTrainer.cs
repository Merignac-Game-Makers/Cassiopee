using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MagicTrainer : MagicEffectBase
{
	public MagicTraining quest;     // accomplir cette magie est une quête : indiquer laquelle
	public PNJ PNJ;

	public override bool DoMoon(MagicOrb orb) {
		var ps = gameObject.GetComponent<ParticleSystem>();     // activer l'animation
		ps.Play();
		
		quest.step1 = true;                                      // renseigner l'étape 1 de la quête => 'terminée'
		if (quest.step1 && quest.step2)
			Success();

		return true;
	}

	public override bool DoSun(MagicOrb orb) {
		var ps = gameObject.GetComponent<ParticleSystem>();     // activer l'animation
		ps.Play();
		
		quest.step2 = true;                                      // renseigner l'étape 1 de la quête => 'terminée'
		if (quest.step1 && quest.step2)
			Success();

		return true;
	}

	private void Success() {
		PNJ.gameObject.GetComponentInChildren<MagicTrainingManager>()?.QuestPassed();
		//magicBook.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(null));

	}

	public void Reverse() {
	}
}

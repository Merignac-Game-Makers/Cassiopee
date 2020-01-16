using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MagicTrainer : MagicEffectBase
{
	public MagicTraining quest;				// accomplir cette magie est une quête : indiquer laquelle
	public PNJ PNJ;							// le PNJ qui donne la quête (le grimoire)
	public ParticleSystem Moon;				// l'effet activé par l'orbe LUNE
	public ParticleSystem Sun;				// l'effet activé par l'orbe SOLEIL

	/// <summary>
	/// Effet LUNE
	/// </summary>
	/// <param name="orb"></param>
	/// <returns></returns>
	public override bool DoMoon(MagicOrb orb) {
		Moon.Play();											// activer l'animation
		quest.step1 = true;                                     // renseigner l'étape 1 de la quête => 'terminée'
		if (quest.step1 && quest.step2)							// si les 2 étapes sont réalisées => succès
			Success();

		return true;
	}

	public override bool DoSun(MagicOrb orb) {
		Sun.Play();												// activer l'animation
		quest.step2 = true;                                     // renseigner l'étape 1 de la quête => 'terminée'
		if (quest.step1 && quest.step2)							// si les 2 étapes sont réalisées => succès	
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

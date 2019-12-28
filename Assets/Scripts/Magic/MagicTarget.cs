using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Cible de magie
///		SI un orbe de magie est déposé sur un objet qui porte ce script
///		on déclenche l'action magique associée
/// </summary>
public class MagicTarget : InteractableObject
{
	public bool oneShot = true;				// par défaut, l'action est activable une seule fois

	// la cible est active si le grimoire est activé
	public override bool IsInteractable => MagicUI.Instance.isFullScreen && enabled;

	// la cible est 'libre' si elle ne contient pas déjà un orbe
	public bool isFree => !GetComponentInChildren<MagicOrb>();

	/// <summary>
	/// action magique
	/// </summary>
	/// <param name="orb">l'orbe déposé sur la cible</param>
	public void MakeMagicalStuff(MagicOrb orb) {
		GetComponentInChildren<MagicEffectBase>()?.Act(orb);	// générer l'action magique
		if (oneShot)											// si la cible est 'oneShot'
			enabled = false;									//		=> désactiver la cible magique
	}
}
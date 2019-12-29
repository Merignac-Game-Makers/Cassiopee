using UnityEngine;
using UnityEngine.AI;

public abstract class MagicEffectBase : InteractableObject
{
	public Page page;                       // la page qui décrit la constellation utilisée pour cet effet magique
	public bool oneShot = true;             // par défaut, l'action est activable une seule fois

	public override bool IsInteractable => MagicUI.Instance.isFullScreen && enabled;      // actif seulement si le grimoire est actif
	public bool isFree => !GetComponentInChildren<MagicOrb>();                  // la cible est 'libre' si elle ne contient pas déjà un orbe

	/// <summary>
	/// action magique
	/// </summary>
	/// <param name="orb">l'orbe déposé sur la cible</param>
	public void MakeMagicalStuff(MagicOrb orb) {
		if (enabled && orb.constellation == page.constellation) {              // l'action n'est déclenchée que si l'orbe a été acquis avec la bonne constellation
			Act(orb);                                               // générer l'action magique
			if (oneShot)                                            // si la cible est 'oneShot'
				enabled = false;                                    //		=> désactiver la cible magique
		}
	}

	public void Act(MagicOrb orb) {
		if (orb.orbType == MagicOrb.OrbType.Moon) {
			// Debug.Log("DO MOON MAGIC !!!");
			DoMoon(orb);
		} else {
			// Debug.Log("DO SUN MAGIC !!!");
			DoSun(orb);
		}
		Destroy(orb.gameObject);
	}


	public abstract void DoMoon(MagicOrb orb);
	public abstract void DoSun(MagicOrb orb);

}

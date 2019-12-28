using UnityEngine;
using UnityEngine.AI;

public abstract class MagicEffectBase : MonoBehaviour
{
	public Page page;			// la page qui décrit la constellation utilisée pour cet effet magique

	public virtual bool isAvailable() => MagicUI.Instance.isFullScreen;		// actif seulement si le grimoire est actif

	public void Act(MagicOrb orb) {
		if (orb.constellation == page.constellation) {						// l'action n'est déclenchée que si l'orbe a été acquis avec la bonne constellation
			if (orb.GetComponentInChildren<MagicOrb>().orbType == MagicOrb.OrbType.Moon) {
				// Debug.Log("DO MOON MAGIC !!!");
				DoMoon(orb);
			} else {
				// Debug.Log("DO SUN MAGIC !!!");
				DoSun(orb);
			}
		}
		Destroy(orb.gameObject);
	}

	public abstract void DoMoon(MagicOrb orb);
	public abstract void DoSun(MagicOrb orb);

}

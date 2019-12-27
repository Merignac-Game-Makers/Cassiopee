using UnityEngine;
using UnityEngine.AI;

public abstract class MagicEffectBase : MonoBehaviour
{
	public virtual bool isAvailable() {
		return MagicUI.Instance.isFullScreen;
	}


	public void Act(MagicOrb orb) {
		if (orb.GetComponentInChildren<MagicOrb>().orbType == MagicOrb.OrbType.Moon) {
			Debug.Log("DO MOON MAGIC !!!");
			DoMoon(orb);
		} else {
			Debug.Log("DO SUN MAGIC !!!");
			DoSun(orb);
		}
		Destroy(orb.gameObject);
	}

	public abstract void DoMoon(MagicOrb orb);
	public abstract void DoSun(MagicOrb orb);

}

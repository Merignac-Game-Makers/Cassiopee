using UnityEngine;
using UnityEngine.AI;

public class MagicTarget : InteractableObject
{

	public bool oneShot = true;

	public override bool IsInteractable => BookUI.Instance.IsOpen && enabled;

	public bool isFree => !GetComponentInChildren<MagicOrb>();

	protected override void Start() {
		base.Start();
	}

	public override void InteractWith(HighlightableObject target) {

	}

	public void MakeMagicalStuff(MagicOrb orb) {
		GetComponentInChildren<MagicEffectBase>()?.Act(orb);
		if (oneShot)
			enabled = false;
	}
}

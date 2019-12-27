﻿using UnityEngine;
using UnityEngine.AI;

public class MagicTarget : InteractableObject
{

	public bool oneShot = true;

	public override bool IsInteractable => MagicUI.Instance.isFullScreen && enabled;

	public bool isFree => !GetComponentInChildren<MagicOrb>();

	protected override void Start() {
		base.Start();
	}

	public override void InteractWith(HighlightableObject target) {

	}

	public void MakeMagicalStuff(MagicOrb orb) {
		Destroy(GetComponentInChildren<Loot>()?.gameObject);	//détruire le cube
		GetComponentInChildren<MagicEffectBase>()?.Act(orb);	// générer l'eau
		enabled = false;
	}
}

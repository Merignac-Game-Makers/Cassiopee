﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using static QuestBase.QuestStatus;


/// <summary>
/// Describes an InteractableObject that can be picked up and grants a specific item when interacted with.
///
/// It will also play a small animation (object going in an arc from spawn point to a random point around) when the
/// object is actually "spawned", and the object becomes interactable only when that animation is finished.
///
/// Finally it will notify the LootUI that a new loot is available in the world so the UI displays the name.
/// </summary>
public class MagicLoot : InteractableObject
{
	public GameObject magicButton;
	public QuestBase nextQuest;


	public override bool IsInteractable() => true;



	protected override void Start() {
		base.Start();
	}


	public override void InteractWith(HighlightableObject target) {
		base.InteractWith(target);
		SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData(){Clip = SFXManager.PickupSound});
		Destroy(gameObject);
		magicButton.SetActive(true);
		PlayerManager.Instance.gameObject.GetComponentInChildren<CharacterData>().isMagicEquiped = true;

		if (nextQuest) {                    // s'il existe une quête liée
			nextQuest.QuestAvailable();		//	 => elle est accessible
		}
	}

}

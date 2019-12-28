﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInWell : QuestBase
{
	public GameObject holder;		// le puits
	public Loot item;				// un cube
	public QuestBase nextQuest;		// une quête suivante qui ne sera accessible qu'après celle-ci

	/// <summary>
	/// bien qu'apparement inutiles les méthodes 'Quest...'
	/// sont nécessaires ici pour que le VIDE_Dialogue Editor puisse les utiliser
	/// (il ne gère pas les classes héritées)
	/// </summary>
	public override void QuestAvailable() {
		base.QuestAvailable();
	}
	public override void RefuseQuest() {
		base.RefuseQuest();
	}
	public override void AcceptQuest() {
		base.AcceptQuest();
	}
	public override void QuestDone() {
		base.QuestDone();
	}
	public override void QuestFailed() {
		base.QuestFailed();
	}
	public override void QuestPassed() {
		base.QuestPassed();
	}

	/// <summary>
	/// Conditions de succès de cette quête
	/// </summary>
	/// <returns></returns>
	public override bool TestSuccess() {
		if (status == QuestStatus.Accepted) {
			var loots = holder.GetComponentsInChildren<Loot>();		// le puits (holder) doit contenir un objet d'inventaire
			foreach (Loot loot in loots) {
				if (loot.name.StartsWith("Cube")) {					// qui doit être un cube
					QuestDone();                            // quête au statut 'Done'			
					nextQuest?.QuestAvailable();            // s'il existe une quête liée => elle devient accessible
					return true;							// SUCCES
				}
			}
			if (loots.Length > 0) {									// si le puits contient autre chose qu'un cube
				QuestFailed();								// quête au statut 'Failed'		
				return false;								// ECHEC
			}
		}
		return false;	// si la quête n'était pas en cours : ECHEC
	}

	public override void Reset() {
		var loots = holder.GetComponentsInChildren<Loot>();
		foreach (Loot loot in loots) {
			loot.InteractWith(PlayerManager.Instance.m_CharacterData);
		}
		AcceptQuest();
	}
}

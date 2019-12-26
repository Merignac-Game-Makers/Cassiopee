using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInWell : QuestBase
{
	public GameObject holder;
	public Loot item;
	public QuestBase nextQuest;

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

	public override bool TestSuccess() {
		if (status == QuestStatus.Accepted) {
			var loots = holder.GetComponentsInChildren<Loot>();
			foreach (Loot loot in loots) {
				if (loot.name.StartsWith("Cube")) {
					QuestDone();
					nextQuest?.QuestAvailable();
					return true;
				}
			}
			if (loots.Length > 0) {
				QuestFailed();
				return false;
			}
		}
		return false;
	}

	public override void Reset() {
		var loots = holder.GetComponentsInChildren<Loot>();
		foreach (Loot loot in loots) {
			loot.InteractWith(PlayerControl.Instance.m_CharacterData);
		}
		AcceptQuest();
	}
}

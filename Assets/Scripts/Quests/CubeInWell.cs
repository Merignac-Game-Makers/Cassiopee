using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInWell : QuestBase
{
	public GameObject holder;
	public Loot item;

	public void RefuseQuest() {
		status = QuestStatus.Refused;
	}
	public void AcceptQuest() {
		status = QuestStatus.Accepted;
	}
	public void QuestDone() {
		status = QuestStatus.Done;
	}
	public void QuestFailed() {
		status = QuestStatus.Failed;
	}
	public void QuestPassed() {
		status = QuestStatus.Passed;
	}

	public override bool TestSuccess() {
		if (status == QuestStatus.Accepted) {
			var loots = holder.GetComponentsInChildren<Loot>();
			foreach (Loot loot in loots) {
				if (loot.name.StartsWith("Cube")) {
					QuestDone();
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
			Destroy(loot.gameObject);
		}
		AcceptQuest();
	}
}

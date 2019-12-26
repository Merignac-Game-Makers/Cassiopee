using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInWell : QuestBase
{

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
		// mis à jour par MagicWater.cs
		return status == QuestStatus.Done;
	}

	public override void Reset() {
		// no reset
	}
}

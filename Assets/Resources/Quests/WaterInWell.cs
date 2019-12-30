using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterInWell : QuestBase
{
	/// <summary>
	/// bien qu'apparement inutiles les méthodes 'Quest...'
	/// sont nécessaires ici pour que le VIDE_Dialogue Editor puisse les utiliser
	/// (il ne gère pas les classes héritées)
	/// </summary>
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
		// QuestStatus.Done est mis à jour par MagicWater.cs
		return status == QuestStatus.Done;
	}

	public override void Reset() {
		// no reset
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaterInWell)), CanEditMultipleObjects]
public class WaterInWellEditor : QuestBaseEditor
{
	public override void OnEnable() {
		base.OnEnable();
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
	}

}
#endif

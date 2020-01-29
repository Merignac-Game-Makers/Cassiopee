using UnityEditor;
using UnityEngine;

public class MagicTraining : QuestBase
{

	[HideInInspector]
	public bool step1, step2;       // étapes de la quête

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
	public override void UpdateStatus() {
		if (!IsDone() &&  step1 && step2)
			QuestDone(); // la quête est accomplie lorsque toutes les étapes sont franchies
	}

	public override void Reset() {
		// no reset
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(MagicTraining)), ]
public class MagicTrainingEditor : QuestBaseEditor
{
	public override void OnEnable() {
		base.OnEnable();
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
	}

}
#endif

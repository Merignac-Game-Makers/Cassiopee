using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static QuestBase.QuestStatus;

public class CubeInWell : QuestBase
{
	public GameObject holder;       // le puits
	public Loot item;               // un cube
	public QuestBase nextQuest;     // une quête suivante qui ne sera accessible qu'après celle-ci

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
	public override void UpdateStatus() {
		if (status == Accepted) {
			var loot = holder.GetComponentInChildren<Loot>();
			if (loot != null) {                         // le puits (holder) doit contenir un objet d'inventaire
				if (loot.name.StartsWith("Cube")) {		// qui doit être un cube
					QuestDone();						//	quête au statut 'Done'	
				} else {                                // si le puits contient autre chose qu'un cube
					QuestFailed();                      //	quête au statut 'Failed'		
				}
			}
		}
	}

	// Remise à zéro
	public override void Reset() {
		var loots = holder.GetComponentsInChildren<Loot>();
		// ramasser tous les objets contenus dans le réceptacle (le puits)
		foreach (Loot loot in loots) {				
			loot.InteractWith(PlayerManager.Instance.characterData);
		}
		AcceptQuest();		// mettre la quête au statut 'acceptée'
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(CubeInWell)), CanEditMultipleObjects]
public class CubeInWellEditor : QuestBaseEditor
{
	SerializedProperty p_holder;
	SerializedProperty p_item;
	SerializedProperty p_nextQuest;

	CubeInWell thisQuest;

	public override void OnEnable() {
		base.OnEnable();
		thisQuest = (CubeInWell)target;
		p_holder = serializedObject.FindProperty(nameof(thisQuest.holder));
		p_item = serializedObject.FindProperty(nameof(thisQuest.item));
		p_nextQuest = serializedObject.FindProperty(nameof(thisQuest.nextQuest));
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.PropertyField(p_holder);
		EditorGUILayout.PropertyField(p_item);
		EditorGUILayout.PropertyField(p_nextQuest);

		serializedObject.ApplyModifiedProperties();
	}

}
#endif
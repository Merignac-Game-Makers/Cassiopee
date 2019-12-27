using UnityEditor;
using UnityEngine;

/// <summary>
/// Objet intéractible sur lequel on peut déposer un objet d'inventaire (loot)
/// </summary>
public class Target : InteractableObject
{

	public override bool IsInteractable => true;

	public bool isFree => !GetComponentInChildren<Loot>();

	protected override void Start() {
		base.Start();
	}


	public override void InteractWith(HighlightableObject target) {

	}
}

/// <summary>
/// Masquer l'éditeur par défaut lié à InterractableObject
/// => éditeur vide (pas de paramètre)
/// </summary>
#if UNITY_EDITOR
[CustomEditor(typeof(Target))]
public class TargetObjectEditor : Editor
{
	public override void OnInspectorGUI() {
		EditorStyles.label.wordWrap = true;
		EditorGUILayout.LabelField("Objet intéractible sur lequel on peut déposer un objet d'inventaire (loot)");
	}
}
#endif

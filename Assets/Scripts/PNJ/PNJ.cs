﻿using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Classe générique pour les PNJ
/// => Intéraction par défaut = interrompre le déplacement + lancer le dialogue
/// </summary>
public class PNJ : InteractableObject
{
	public string PNJName;          // nom du PNJ (pour les dialogues)
	public Sprite image;            // image du PNJ (pour les dialogues)
	public int onTheFlyRadius;  // rayon d'action du mode onTheFlyOnce

	float initialRadius = 1.5f;
	CapsuleCollider cCollider;

	public override bool IsInteractable => true;

	protected override void Start() {
		base.Start();
		var dialogue = gameObject.GetComponentInChildren<VIDE_Assign>();
		if (dialogue != null) {									// s'il y a un dialogue
			if (image != null) {								//		et un image pour le PNJ
				dialogue.defaultNPCSprite = image;				//		=> affecter l'image du PNJ au dialogue
			}
			if (!String.IsNullOrEmpty(PNJName)) {				//		et un nom
				dialogue.alias = PNJName;						//		=> affecter le nom du PNJ au dialogue
			}
		}
		if (mode != Mode.onClick) {										// si le mode est onTheFlyOnce
			cCollider = gameObject.GetComponent<CapsuleCollider>();		
			if (cCollider && onTheFlyRadius> cCollider.radius) {		// et qu'il y a un CapsuleCollider
				initialRadius = cCollider.radius;						//	=> mettre en place le rayon élargi
				SetColliderRadius(onTheFlyRadius);
			}
		}
	}

	void SetColliderRadius(float radius) {
		cCollider.radius = radius;
	}

	public void ResetColliderRadius() {
		SetColliderRadius(initialRadius);
	}

	public void DisableCollider() {
		cCollider.enabled = false;
		cCollider.isTrigger = false;
	}

	// intéraction avec un PNJ
	public override void InteractWith(HighlightableObject target) {
		DialogueTrigger dt = GetComponentInChildren<DialogueTrigger>();     // si il y a un dialogue
		if (dt) {
			PlayerManager.Instance.StopAgent();                             //	stopper la navigation
			GetComponentInChildren<DialogueTrigger>().Run();                //	démarrer le dialogue	

			if (mode == Mode.onTheFlyOnce) {                                // si le PNJ est en mode 'onTheFlyOnce'
				Collider dtc = dt.gameObject.GetComponent<Collider>();      // et qu'il existe un collider spécifique de ce mode
				ResetColliderRadius();										// restaurer le rayon initial du collider (pour éviter à l'avenir une intéraction sur un rayon élargi)
			}
		}

		base.InteractWith(target);                                          // intéraction par défaut
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(PNJ))]
[CanEditMultipleObjects]
public class PNJEditor : Editor
{
	SerializedProperty p_mode;
	SerializedProperty p_radius;
	SerializedProperty p_PNJName;
	SerializedProperty p_image;

	PNJ m_PNJ;

	public void OnEnable() {
		m_PNJ = (PNJ)target;
		p_mode = serializedObject.FindProperty(nameof(m_PNJ.mode));
		p_PNJName = serializedObject.FindProperty(nameof(m_PNJ.PNJName));
		p_image = serializedObject.FindProperty(nameof(m_PNJ.image));
		p_radius = serializedObject.FindProperty(nameof(m_PNJ.onTheFlyRadius));
	}


	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(p_mode);
		if (m_PNJ.mode != InteractableObject.Mode.onClick) {
			EditorGUILayout.PropertyField(p_radius);
		}
		EditorGUILayout.PropertyField(p_PNJName);
		p_image.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Default NPC Sprite", "Default NPC sprite for this component"), m_PNJ.image, typeof(Sprite), false);

		serializedObject.ApplyModifiedProperties();
	}
	void OnInspectorUpdate() {
		Repaint();
	}
}
#endif

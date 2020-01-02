using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Base class for interactable object, inherit from this class and override InteractWith to handle what happen when
/// the player interact with the object.
/// </summary>
public abstract class InteractableObject : HighlightableObject
{

	public enum Mode { onClick, onTheFly, onTheFlyOnce }

	public Mode mode;

	public abstract bool IsInteractable { get; }

	[HideInInspector]
	public bool Clicked;

	public virtual void InteractWith(HighlightableObject target) {
		Clicked = false;
		if (mode == Mode.onTheFlyOnce)
			mode = Mode.onClick;
	}

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(InteractableObject))]
//public class InteractableObjectEditor : Editor
//{
//	SerializedProperty m_IsQuest;
//	SerializedProperty m_Quest;
//	SerializedProperty m_Mode;

//	void OnEnable() {
//		m_IsQuest = serializedObject.FindProperty("IsQuest");
//		m_Quest = serializedObject.FindProperty("Quest");
//		m_Mode = serializedObject.FindProperty("m_Mode");

//		serializedObject.ApplyModifiedProperties();
//	}

//	public override void OnInspectorGUI() {
//		serializedObject.Update();

//		EditorGUILayout.PropertyField(m_Mode);
//		EditorGUILayout.PropertyField(m_IsQuest);

//		if (m_IsQuest.boolValue) {
//			EditorGUILayout.PropertyField(m_Quest);
//		}

//		serializedObject.ApplyModifiedProperties();

//	}
//}
//#endif

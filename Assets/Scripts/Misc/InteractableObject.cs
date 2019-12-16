using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Base class for interactable object, inherit from this class and override InteractWith to handle what happen when
/// the player interact with the object.
/// </summary>
public abstract class InteractableObject : HighlightableObject
{

	public enum mode { onClick, onTheFly, onTheFlyOnce }

	[SerializeField]
	public mode m_Mode;

	public abstract bool IsInteractable { get; }

	[HideInInspector]
	public bool Clicked;

	public abstract void InteractWith(HighlightableObject target);
}

#if UNITY_EDITOR
[CustomEditor(typeof(SFXManager))]
public class InteractableObjectEditor : Editor
{
	SerializedProperty m_Mode;

	void OnEnable() {
		m_Mode = serializedObject.FindProperty("m_Mode");

		int mode = Enum.GetValues(typeof(InteractableObject.mode)).Length;
		serializedObject.ApplyModifiedProperties();
	}
}
#endif

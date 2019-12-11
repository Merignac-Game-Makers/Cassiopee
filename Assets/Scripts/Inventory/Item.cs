﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base clase of all items in the game. This is an abstract class and need to be inherited to specify behaviour.
/// The project offer 3 type of items : UsableItem, Equipment and Weapon
/// </summary>
public abstract class Item : ScriptableObject
{
	public string ItemName;
	public Sprite ItemSprite;
	public string Description;
	public GameObject WorldObjectPrefab;

	/// <summary>
	/// Called by the inventory system when the object is "used" (double clicked)
	/// </summary>
	/// <param name="user">The CharacterDate that used that item</param>
	/// <returns>If it was actually used (allow the inventory to know if it can remove the object or not)</returns>
	public virtual bool UsedBy(CharacterData user) {
		return false;
	}

	public virtual string GetDescription() {
		return Description;
	}
}


#if UNITY_EDITOR
public class ItemEditor
{
	SerializedProperty m_NameProperty;
	SerializedProperty m_IconProperty;
	SerializedProperty m_DescriptionProperty;
	SerializedProperty m_WorldObjectPrefabProperty;

	public void Init(SerializedObject target) {
		m_NameProperty = target.FindProperty(nameof(Item.ItemName));
		m_IconProperty = target.FindProperty(nameof(Item.ItemSprite));
		m_DescriptionProperty = target.FindProperty(nameof(Item.Description));
		m_WorldObjectPrefabProperty = target.FindProperty(nameof(Item.WorldObjectPrefab));
	}

	public void GUI() {
		EditorGUILayout.PropertyField(m_IconProperty);
		EditorGUILayout.PropertyField(m_NameProperty);
		EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));
		EditorGUILayout.PropertyField(m_WorldObjectPrefabProperty);
	}
}
#endif
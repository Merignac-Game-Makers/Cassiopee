﻿using UnityEngine;
using static InventoryManager;

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
	public bool animate = true;

	public bool combinable = false;
	public Item combineWith;
	public Item obtain;

	public bool dropable = true;


	public InventoryEntry entry = null; // L'entrée d'inventaire lorsque l'objet a été ramassé

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

	/// <summary>
	/// L'entrée d'inventaire lorsque l'objet a été ramassé
	/// </summary>
	/// <returns></returns>
	public virtual InventoryEntry GetEntry() {
		return entry;
	}

	public Item Combine(Item other) {
		return (other == combineWith) ? obtain : null;
	}
}

#if UNITY_EDITOR
public class ItemEditor
{
	SerializedProperty pNameProperty;
	SerializedProperty pIconProperty;
	SerializedProperty pDescriptionProperty;
	SerializedProperty pWorldObjectPrefabProperty;
	SerializedProperty pCombinable;
	SerializedProperty pCombineWith;
	SerializedProperty pObtain;
	SerializedProperty pDropable;



	public void Init(SerializedObject target) {

		pNameProperty = target.FindProperty(nameof(Item.ItemName));
		pIconProperty = target.FindProperty(nameof(Item.ItemSprite));
		pDescriptionProperty = target.FindProperty(nameof(Item.Description));
		pWorldObjectPrefabProperty = target.FindProperty(nameof(Item.WorldObjectPrefab));
		pCombinable = target.FindProperty(nameof(Item.combinable));
		pCombineWith = target.FindProperty(nameof(Item.combineWith));
		pObtain = target.FindProperty(nameof(Item.obtain));
		pDropable = target.FindProperty(nameof(Item.dropable));
	}

	public void GUI(Item item) {

		EditorGUILayout.PropertyField(pIconProperty);
		EditorGUILayout.PropertyField(pNameProperty);
		EditorGUILayout.PropertyField(pDescriptionProperty, GUILayout.MinHeight(128));
		EditorGUILayout.PropertyField(pWorldObjectPrefabProperty);

		//EditorGUI.BeginChangeCheck();
		item.combinable = EditorGUILayout.Toggle("Combinable", pCombinable.boolValue);
		if (item.combinable) {
			EditorGUILayout.PropertyField(pCombineWith);
			EditorGUILayout.PropertyField(pObtain);
		} else {
			item.dropable = EditorGUILayout.Toggle("Déposable", pDropable.boolValue);
		}
	}
}
#endif
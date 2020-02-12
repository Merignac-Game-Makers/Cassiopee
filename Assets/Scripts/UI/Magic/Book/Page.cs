using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Page", menuName = "Custom/Spell book page", order = -999)]
public class Page : ScriptableObject
{
	public string title;
	public Sprite picture;
	public string text;
	public string constellation;
	public bool isAvailable = false;

	public bool hasHelp;
	public Sprite helpPicture;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Page))]
public class PageEditor : Editor
{
	SerializedProperty pTitle;
	SerializedProperty pPicture;
	SerializedProperty pText;
	SerializedProperty pConstellation;
	SerializedProperty pHasHelp;
	SerializedProperty pIsAvailable;
	SerializedProperty pHelpPicture;

	Page m_Page;

	public void OnEnable() {
		m_Page = (Page)target;

		pTitle = serializedObject.FindProperty(nameof(m_Page.title));
		pPicture = serializedObject.FindProperty(nameof(m_Page.picture));
		pText = serializedObject.FindProperty(nameof(m_Page.text));
		pConstellation = serializedObject.FindProperty(nameof(m_Page.constellation));
		pHasHelp = serializedObject.FindProperty(nameof(m_Page.hasHelp));
		pIsAvailable = serializedObject.FindProperty(nameof(m_Page.isAvailable));
		pHelpPicture = serializedObject.FindProperty(nameof(m_Page.helpPicture));

	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(pTitle);
		pPicture.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Constellation", "Constellation"), m_Page.picture, typeof(Sprite), false);

		EditorGUILayout.PropertyField(pHasHelp);
		if (m_Page.hasHelp)
			pHelpPicture.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Aide", "Aide"), m_Page.helpPicture, typeof(Sprite), false);


		var options = new GUILayoutOption[] { GUILayout.MinHeight(128), GUILayout.ExpandHeight(true)};	
		pText.stringValue = GUILayout.TextArea(pText.stringValue, options);

		EditorGUILayout.PropertyField(pConstellation);
		EditorGUILayout.PropertyField(pIsAvailable);

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

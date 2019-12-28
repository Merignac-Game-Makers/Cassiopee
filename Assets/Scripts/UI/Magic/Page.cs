using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Page", menuName = "Custom/Spell book page", order = -999)]
public class Page : ScriptableObject
{
	public string Title;
	public Sprite Picture;
	public string Text;
	public string constellation;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Page))]
public class PageEditor : Editor
{
	SerializedProperty m_TitleProperty;
	SerializedProperty m_PictureProperty;
	SerializedProperty m_TextProperty;
	SerializedProperty m_ConstellationProperty;

	Page m_Page;

	public void OnEnable() {
		m_TitleProperty = serializedObject.FindProperty("Title");
		m_PictureProperty = serializedObject.FindProperty("Picture");
		m_TextProperty = serializedObject.FindProperty("Text");
		m_ConstellationProperty = serializedObject.FindProperty("constellation");

		m_Page = (Page)target;
	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(m_TitleProperty);
		EditorGUILayout.PropertyField(m_PictureProperty);

		var options = new GUILayoutOption[] { GUILayout.MinHeight(128), GUILayout.ExpandHeight(true)};	
		m_TextProperty.stringValue = GUILayout.TextArea(m_TextProperty.stringValue, options);

		EditorGUILayout.PropertyField(m_ConstellationProperty);

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

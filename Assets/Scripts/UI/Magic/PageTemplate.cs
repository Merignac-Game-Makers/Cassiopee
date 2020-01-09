using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


//[CreateAssetMenu(fileName = "Page", menuName = "Custom/Spell book page", order = -999)]
public class PageTemplate : MonoBehaviour
{
	public Page page;
	public List<Constellation> constellations;
}

[Serializable]
public class Constellation
{
	public List<Activable> objects;
}

#if UNITY_EDITOR
[CustomEditor(typeof(PageTemplate))]
[CanEditMultipleObjects]
public class PageTemplateEditor : Editor
{
	SerializedProperty pageProperty;
	SerializedProperty constellationsProperty;

	PageTemplate m_Page;
	string cName;

	public void OnEnable() {
		m_Page = (PageTemplate)target;
		pageProperty = serializedObject.FindProperty(nameof(m_Page.page));
	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		if (m_Page.page != null) {
			cName = m_Page.page.constellation;
			constellationsProperty = serializedObject.FindProperty(nameof(m_Page.constellations));
		}

		EditorGUILayout.PropertyField(pageProperty);
		if (cName!=null && cName!="") {
			EditorGUILayout.LabelField(cName, EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(constellationsProperty, true);
		}


		serializedObject.ApplyModifiedProperties();
	}
}
#endif

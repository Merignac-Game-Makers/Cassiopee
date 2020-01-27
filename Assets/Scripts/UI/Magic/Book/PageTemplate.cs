using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
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
		if (cName != null && cName != "") {
			EditorGUILayout.LabelField(cName, EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(constellationsProperty, true);
		}


		serializedObject.ApplyModifiedProperties();
	}
}

//[CustomPropertyDrawer(typeof(Constellation))]
//public class ConstellationDrawerUIE : PropertyDrawer
//{
//	public override VisualElement CreatePropertyGUI(SerializedProperty property) {
//		// Create property container element.
//		var container = new VisualElement();

//		// Create property fields.
//		var objects = new PropertyField(property.FindPropertyRelative("objects"));

//		// Add fields to the container.
//		container.Add(objects);

//		return container;
//	}

//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

//		EditorGUI.BeginProperty(position, label, property);

//		//label
//		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//		// Don't make child fields be indented
//		var indent = EditorGUI.indentLevel;
//		EditorGUI.indentLevel = 0;
//		var fieldWidth = position.width / 2;

//		EditorGUI.PropertyField(
//		new Rect(position.x, position.y, fieldWidth, position.height),
//			property.FindPropertyRelative("objects"), GUIContent.none);

//		EditorGUI.EndProperty();
//	}
//}

#endif

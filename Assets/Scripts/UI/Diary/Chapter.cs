using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapitre de journal", menuName = "Custom/Chapitre", order = -999)]
public class Chapter : ScriptableObject
{
	public bool enabled;                        // ce chapitre est-il affiché ?
	public string title;                        // titre du chapitre
	public List<Paragraph> paragraphs;          // un chapitre peut compter plusieurs paragraphes dont certians ne seront pas affichés au départ

	public List<int> state = new List<int>();	// liste des index cournats des paragraphes

	public List<int> State() {
		state.Clear();
		foreach (Paragraph p in paragraphs) {
			state.Add(0); //p.idx
		}
		return state;
	}

	public void Reset() {
		foreach (Paragraph p in paragraphs) {
			p.Reset();
		}
	}
}


[CreateAssetMenu(fileName = "Paragraphe de journal", menuName = "Custom/Paragraphe", order = -999)]
public class Paragraph : ScriptableObject
{
	public bool enabled;								// ce paragraphe est-il affiché ?
	public List<string> text = new List<string>();      // chaque paragraphe peut avoir plusieurs versions selon l'avancement du sujet (rencontre, quête, événement...)
	public int idx = 0;									// index de la version active

	public string activeVersion => text[idx];

	public void Reset() {
		idx = 0;
	}

	public int Next() {
		idx = Math.Min(idx + 1, text.Count - 1);
		return idx;
	}

	public int Last() {
		idx = text.Count - 1;
		return idx;
	}

	public string Get(int version) {
		if (version >= 0 && version < text.Count)
			return text[version];
		else
			return null;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Paragraph))]
public class ParagraphEditor : Editor
{
	SerializedProperty pEnabled;
	SerializedProperty pText;
	SerializedProperty pIdx;

	Paragraph m_Paragraph;

	public void OnEnable() {
		m_Paragraph = (Paragraph)target;

		pEnabled = serializedObject.FindProperty(nameof(m_Paragraph.enabled));
		pText = serializedObject.FindProperty(nameof(m_Paragraph.text));
		pIdx = serializedObject.FindProperty(nameof(m_Paragraph.idx));
	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(pEnabled);

		var size = EditorGUILayout.IntField("Nbre de variantes", m_Paragraph.text.Count);
		if (size != m_Paragraph.text.Count && size!=0) {
			m_Paragraph.text.Resize(size, "");
		} else {
			var options = new GUILayoutOption[] { GUILayout.MinHeight(128), GUILayout.ExpandHeight(true) };
			for (int i = 0; i < size; i++) {
				m_Paragraph.text[i] = GUILayout.TextArea(m_Paragraph.text[i], options);
			}

		}

		EditorGUILayout.PropertyField(pIdx);

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DialogueEntryNode.QuestStatus;


[CreateAssetMenu(fileName = "DialogueEntryNode", menuName = "Custom/Dialogue Entry Node", order = -999)]
public class DialogueEntryNode : ScriptableObject
{
	public enum QuestStatus { NonAvailable, Available, Accepted, Refused, Done, Failed, Passed }

	public QuestBase quest;
	public Entry[] entries;

	public struct Entry
	{
		public QuestStatus status;
		public int node;
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(DialogueEntryNode))]
public class DialogueEntryNodeEditor : Editor
{
	SerializedProperty pQuest;

	DialogueEntryNode m_target;

	string[] statusStr;
	public void OnEnable() {
		m_target = (DialogueEntryNode)target;
		statusStr = new string[Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).Length];
		for (int i = 0; i < statusStr.Length; i++) {
			statusStr[i] = ((Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).GetValue(i))).ToString();
		}
		pQuest = serializedObject.FindProperty(nameof(m_target.quest));

	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(pQuest);
		if (m_target.entries != null && m_target.entries.Length > 0) {
			for (int i = 0; i < m_target.entries.Length; i++) {
				EditorGUILayout.BeginHorizontal();
				var idx = EditorGUILayout.Popup(i, statusStr);
				m_target.entries[i].status = (DialogueEntryNode.QuestStatus)(Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).GetValue(idx));
				m_target.entries[i].node = EditorGUILayout.IntField(m_target.entries[i].node);
				EditorGUILayout.EndHorizontal();
			}
		}


		serializedObject.ApplyModifiedProperties();
	}
}
#endif

using UnityEngine;
using System.Collections.Generic;
using System;
using static DialogueEntryNode.QuestStatus;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Non utilisée pour l'instant
/// En attente pour apprentissage d'un 'custom editor' qui gèrerait le 'multi objet'...
/// à chercher
/// </summary>


//[CreateAssetMenu(fileName = "DialogueDispatcher", menuName = "Custom/Dialogue Dispatcher", order = -999)]
public class DialogueDispatcher : MonoBehaviour
{
	public VIDE_Assign dialogueAssign;
	public List<DialogueEntryNode> dispatch;

	public string[] status { get; private set; }
	public DialogueEntryNode.QuestStatus denST;

	private void Start() {
		status = new string[Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).Length];
		for (int i = 0; i < status.Length; i++) {
			status[i] = Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).GetValue(i).ToString();
		}
	}
}


//#if UNITY_EDITOR
//[CustomEditor(typeof(DialogueDispatcher)), CanEditMultipleObjects]
//public class DialogueDispatcherEditor : Editor
//{
//	SerializedProperty pDialogueAssign;
//	SerializedProperty pDispatch;

//	DialogueDispatcher m_target;
//	string[] statusStr;

//	public void OnEnable() {
//		m_target = (DialogueDispatcher)target;

//		pDialogueAssign = serializedObject.FindProperty(nameof(m_target.dialogueAssign));
//		pDispatch = serializedObject.FindProperty(nameof(m_target.dispatch));

//		statusStr = new string[Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).Length];
//		for (int i = 0; i < statusStr.Length; i++) {
//			statusStr[i] = ((Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).GetValue(i))).ToString();
//		}
//	}

//	public override void OnInspectorGUI() {
//		EditorStyles.textField.wordWrap = true;
//		serializedObject.Update();

//		EditorGUILayout.PropertyField(pDialogueAssign);

//		if (m_target.dispatch != null && m_target.dispatch.Count > 0) {
//			for (int i = 0; i < m_target.dispatch.Count; i++) {
//				var den = m_target.dispatch[i] != null ? m_target.dispatch[i] : (DialogueEntryNode)CreateInstance(typeof(DialogueEntryNode));
//				den = CreateInstance(typeof(DialogueEntryNode)) as DialogueEntryNode;
//				var so = new SerializedObject(den);
//				so.Update();
//				var denQuest = so.FindProperty(nameof(den.quest));
//				EditorGUILayout.PropertyField(denQuest);
//				so.ApplyModifiedProperties();
//				den.quest = (QuestBase)denQuest.objectReferenceValue;
//				m_target.dispatch[i].quest = den.quest;
//				if (den.entries != null && den.entries.Length > 0) {
//					for (int j = 0; j < den.entries.Length; j++) {
//						EditorGUILayout.BeginHorizontal();
//						var idx = EditorGUILayout.Popup(j, statusStr);
//						den.entries[j].status = (DialogueEntryNode.QuestStatus)(Enum.GetValues(typeof(DialogueEntryNode.QuestStatus)).GetValue(idx));
//						den.entries[j].node = EditorGUILayout.IntField(den.entries[j].node);
//						EditorGUILayout.EndHorizontal();
//					}

//				}
//			}
//		}




//		serializedObject.ApplyModifiedProperties();
//	}
//}
//#endif

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static DialogueEntryNode.QuestStatus;



public class QuestManager : MonoBehaviour
{

	public static QuestManager Instance;

	public Affectation[] affectations;

	[HideInInspector]
	public List<QuestBase> quests;
	public void Init() {
		Instance = this;
	}

[Serializable]
public struct Affectation : IEquatable<Affectation>
{
	public PNJ pnj;
	public QuestBase quest;

	public Affectation(PNJ pnj, QuestBase quest) {
		this.pnj = pnj;
		this.quest = quest;
	}

	public bool Equals(Affectation other) {
		return pnj == other.pnj && quest == other.quest;
	}
}
	private void Start() {
		quests = new List<QuestBase>(GetComponentsInChildren<QuestBase>());
	}

	public void Add(QuestBase quest) {
		quests.Add(quest);
	}

	public List<PNJ> getOwners(QuestBase quest) {
		List<PNJ> result = new List<PNJ>();
		foreach(Affectation affectation in affectations) {
			if (affectation.quest == quest)
				result.Add(affectation.pnj);
		}
		return result;
	}
	public List<QuestBase> GetPending() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests) {
			if (quest.status == Accepted || quest.status == Failed)
				list.Add(quest);
		}
		return list;
	}
	public List<QuestBase> GetDone() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests) {
			if (quest.status == Done || quest.status == Passed)
				list.Add(quest);
		}
		return list;
	}

	public void SetStatus(QuestBase quest, DialogueEntryNode.QuestStatus status) {
		int idx = quests.IndexOf(quest);
		if (idx != -1) {
			quests[idx].status = status;
		}
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(QuestManager.Affectation))]
public class AffectationDrawerUIE : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property) {
		// Create property container element.
		var container = new VisualElement();

		// Create property fields.
		var pnj = new PropertyField(property.FindPropertyRelative("pnj"));
		var quest = new PropertyField(property.FindPropertyRelative("quest"));

		// Add fields to the container.
		container.Add(pnj);
		container.Add(quest);

		return container;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {


		EditorGUI.BeginProperty(position, label, property);



		//label
	    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		var fieldWidth = position.width / 2;

		EditorGUI.PropertyField(
		new Rect(position.x, position.y, fieldWidth, position.height),
			property.FindPropertyRelative("pnj"), GUIContent.none);
		EditorGUI.PropertyField(
		new Rect(position.x + fieldWidth, position.y, fieldWidth, position.height),
			property.FindPropertyRelative("quest"), GUIContent.none);


		//EditorGUILayout.LabelField(label);
		//EditorGUILayout.PropertyField(property.FindPropertyRelative("pnj"), GUIContent.none);
		//EditorGUILayout.PropertyField(property.FindPropertyRelative("quest"), GUIContent.none);

		//// Calculate rects
		//var pnjRect = new Rect(position.x, position.y, 30, position.height);
		//var questRect = new Rect(position.x + 35, position.y, 50, position.height);

		//// Draw fields - passs GUIContent.none to each so they are drawn without labels
		//EditorGUI.PropertyField(pnjRect, property.FindPropertyRelative("pnj"), GUIContent.none);
		//EditorGUI.PropertyField(questRect, property.FindPropertyRelative("quest"), GUIContent.none);

		// Set indent back to what it was
		//EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
#endif

//#if UNITY_EDITOR
//[CustomEditor(typeof(QuestManager)), CanEditMultipleObjects]
//public class QuestManagerEditor : Editor
//{
//	SerializedProperty pAffectations;
//	SerializedProperty ps;

//	QuestManager m_target;

//	List<QuestManager.Affectation> affectations;
//	int size;

//	public void OnEnable() {
//		m_target = (QuestManager)target;
//		pAffectations = serializedObject.FindProperty(nameof(m_target.affectations));
//		ps = serializedObject.FindProperty(nameof(m_target.s));
//		size = pAffectations.arraySize;
//		affectations = new List<QuestManager.Affectation>(m_target.affectations);

//	}

//	public override void OnInspectorGUI() {
//		EditorStyles.textField.wordWrap = true;
//		serializedObject.Update();


//		size = EditorGUILayout.IntField("Size", size);

//		EditorGUI.BeginChangeCheck();
//		for (int i = 0; i < pAffectations.arraySize; i++) {
//			GUILayout.BeginHorizontal();
//			GUILayout.BeginVertical();
//			pAffectations.GetArrayElementAtIndex(i).FindPropertyRelative("pnj").objectReferenceValue =
//				EditorGUILayout.ObjectField(pAffectations.GetArrayElementAtIndex(i).FindPropertyRelative("pnj").objectReferenceValue, typeof(PNJ), true);
//			//EditorGUILayout.ObjectField(pAffectations.GetArrayElementAtIndex(i).FindPropertyRelative("pnj").objectReferenceValue, typeof(PNJ), true);
//			GUILayout.EndVertical();
//			GUILayout.EndHorizontal();
//		}
//		EditorGUI.EndChangeCheck();



//		if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) { //&& Event.current.isKey && Event.current.keyCode == KeyCode.Return   && 
//			var enter = Event.current.isKey && Event.current.keyCode == KeyCode.Return;
//			var ev = Event.current.type;
//			EditorGUI.BeginChangeCheck();
//			pAffectations.arraySize = size;
//		}
//		serializedObject.ApplyModifiedProperties();
//	}
//}